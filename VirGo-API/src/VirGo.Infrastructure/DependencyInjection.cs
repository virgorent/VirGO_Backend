using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using MongoDB.Driver;
using VirGo.Application.Abstractions;
using VirGo.Application.Configuration;
using VirGo.Infrastructure.Persistence.MongoDb;
using VirGo.Infrastructure.Auth;
using VirGo.Application.Auth;
using VirGo.Application.Marketplace;
using VirGo.Infrastructure.Caching;
using Amazon.SimpleEmailV2;
using Amazon.SQS;
using Microsoft.Extensions.Logging;
using VirGo.Application.Email;
using VirGo.Application.Jobs;
using VirGo.Infrastructure.Email;
using VirGo.Infrastructure.Jobs;

namespace VirGo.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        services
            .AddOptions<PersistenceOptions>()
            .Bind(configuration.GetSection("Persistence"))
            .Validate(
                options => string.Equals(options.Provider, "InMemory", StringComparison.OrdinalIgnoreCase)
                    || (string.Equals(options.Provider, "MongoDb", StringComparison.OrdinalIgnoreCase)
                        && !string.IsNullOrWhiteSpace(options.ConnectionString)),
                "Persistence uses InMemory or MongoDb with a connection string.")
            .ValidateOnStart();

        services.AddSingleton<IClock, SystemClock>();
        services.AddSingleton<IPasswordHasher, IdentityPasswordHasher>();
        services.AddSingleton<IRefreshTokenGenerator, RefreshTokenGenerator>();
        services.AddSingleton<ITokenService, JwtTokenService>();
        services.AddScoped<LoginHandler>();
        services.AddScoped<RegisterHandler>();
        services.AddScoped<LogoutHandler>();
        services.AddScoped<QuoteHandler>();
        services.AddScoped<BookingHandler>();

        if (!string.Equals(configuration["Persistence:Provider"], "MongoDb", StringComparison.OrdinalIgnoreCase))
        {
            services.AddSingleton<IUserRepository, InMemoryUserRepository>();
            services.AddSingleton<ISessionRepository, InMemorySessionRepository>();
            services.AddSingleton<IVenueRepository, InMemoryVenueRepository>();
            services.AddSingleton<InMemoryQuoteRepository>();
            services.AddSingleton<IQuoteRepository>(serviceProvider => serviceProvider.GetRequiredService<InMemoryQuoteRepository>());
            services.AddSingleton<IQuoteExpiry>(serviceProvider => serviceProvider.GetRequiredService<InMemoryQuoteRepository>());
            services.AddSingleton<INoticeRepository, InMemoryNoticeRepository>();
            services.AddSingleton<IBookingRepository, InMemoryBookingRepository>();
        }
        else
        {
            services.AddSingleton<IUserRepository, MongoUserRepository>();
            services.AddSingleton<ISessionRepository, MongoSessionRepository>();
            services.AddSingleton<IVenueRepository, MongoVenueRepository>();
            services.AddSingleton<IQuoteRepository, MongoQuoteRepository>();
            services.AddSingleton<IQuoteExpiry, MongoQuoteExpiry>();
            services.AddSingleton<INoticeRepository, MongoNoticeRepository>();
            services.AddSingleton<IBookingRepository, MongoBookingRepository>();
        }

        if (string.Equals(configuration["Persistence:Provider"], "MongoDb", StringComparison.OrdinalIgnoreCase))
        {
            services.AddSingleton<IMongoClient>(serviceProvider =>
            {
                var options = serviceProvider.GetRequiredService<IOptions<PersistenceOptions>>().Value;
                var settings = MongoClientSettings.FromConnectionString(options.ConnectionString!);
                settings.ApplicationName = "VirGo-Api";
                settings.MaxConnectionPoolSize = 10;
                settings.ServerSelectionTimeout = TimeSpan.FromSeconds(10);
                return new MongoClient(settings);
            });

            services.AddSingleton(serviceProvider =>
            {
                var options = serviceProvider.GetRequiredService<IOptions<PersistenceOptions>>().Value;
                return serviceProvider.GetRequiredService<IMongoClient>().GetDatabase(options.DatabaseName);
            });
            services.AddSingleton<IMongoRepositoryFactory, MongoRepositoryFactory>();
        }

        AddCaching(services, configuration);
        AddJobs(services, configuration);

        return services;
    }

    /// <summary>Job queue, email sender and job handlers. Empty settings fall back to in-memory and logging implementations.</summary>
    private static void AddJobs(IServiceCollection services, IConfiguration configuration)
    {
        var queueUrl = configuration["Jobs:QueueUrl"];
        if (string.IsNullOrWhiteSpace(queueUrl))
        {
            services.AddSingleton<InMemoryJobPublisher>();
            services.AddSingleton<IJobPublisher>(serviceProvider => serviceProvider.GetRequiredService<InMemoryJobPublisher>());
        }
        else
        {
            services.AddSingleton<IAmazonSQS, AmazonSQSClient>();
            services.AddSingleton<IJobPublisher>(serviceProvider => new SqsJobPublisher(
                serviceProvider.GetRequiredService<IAmazonSQS>(),
                queueUrl,
                serviceProvider.GetRequiredService<ILogger<SqsJobPublisher>>()));
        }

        var senderAddress = configuration["Email:SenderAddress"];
        if (string.IsNullOrWhiteSpace(senderAddress))
        {
            services.AddSingleton<IEmailSender, LoggingEmailSender>();
        }
        else
        {
            services.AddSingleton<IAmazonSimpleEmailServiceV2, AmazonSimpleEmailServiceV2Client>();
            services.AddSingleton<IEmailSender>(serviceProvider => new SesEmailSender(
                serviceProvider.GetRequiredService<IAmazonSimpleEmailServiceV2>(),
                senderAddress));
        }

        services.AddScoped<IJobHandler, BookingCreatedHandler>();
        services.AddScoped<IJobHandler, EmailSendHandler>();
        services.AddScoped<IJobHandler, QuotesExpireHandler>();
        services.AddScoped<JobDispatcher>();
    }

    private static void AddCaching(IServiceCollection services, IConfiguration configuration)
    {
        var connectionString = configuration["Redis:ConnectionString"];
        if (string.IsNullOrWhiteSpace(connectionString))
        {
            services.AddDistributedMemoryCache();
        }
        else
        {
            var redis = StackExchange.Redis.ConfigurationOptions.Parse(connectionString);
            redis.AbortOnConnectFail = false;
            redis.ConnectTimeout = 2000;
            redis.SyncTimeout = 1000;
            redis.AsyncTimeout = 500;
            redis.ConnectRetry = 1;
            services.AddStackExchangeRedisCache(options =>
            {
                options.ConfigurationOptions = redis;
                options.InstanceName = "virgo:";
            });
        }

        services.AddSingleton<ICacheService, DistributedCacheService>();
        services.Decorate<IVenueRepository>((inner, serviceProvider) =>
            new CachedVenueRepository(inner, serviceProvider.GetRequiredService<ICacheService>()));
    }

    private sealed class SystemClock : IClock
    {
        public DateTime UtcNow => DateTime.UtcNow;
    }
}
