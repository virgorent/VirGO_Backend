using Amazon.Lambda.SQSEvents;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using VirGo.Application.Abstractions;
using VirGo.Application.Email;
using VirGo.Application.Jobs;
using VirGo.Domain.Marketplace;
using VirGo.Infrastructure;
using VirGo.Infrastructure.Jobs;
using VirGo.Worker;

namespace VirGo.UnitTests.Jobs;

/// <summary>Spec 099 (queue and worker), 903 (email) and 905 (scheduled quote expiry).</summary>
public sealed class JobTests
{
    private static readonly DateTime Now = new(2026, 11, 12, 3, 0, 0, DateTimeKind.Utc);

    private static JobMessage BookingCreated(string bookingId = "b-1") => new(
        JobTypes.BookingCreated,
        new Dictionary<string, string>
        {
            ["bookingId"] = bookingId,
            ["clientId"] = "client-1",
            ["folio"] = "VG-000123",
            ["date"] = "2026-12-05"
        },
        "corr-1");

    [Fact]
    public void JobMessageRoundTripsThroughJson()
    {
        var parsed = JobMessage.Parse(BookingCreated().ToJson());

        Assert.Equal(JobTypes.BookingCreated, parsed.Type);
        Assert.Equal("corr-1", parsed.CorrelationId);
        Assert.Equal("VG-000123", parsed.Get("folio"));
    }

    [Theory]
    [InlineData("not json")]
    [InlineData("{}")]
    [InlineData("{\"type\":\"\"}")]
    public void MalformedMessagesAreRejected(string body) =>
        Assert.Throws<InvalidJobMessageException>(() => JobMessage.Parse(body));

    [Fact]
    public async Task BookingCreatedCreatesAnUnreadNoticeFor90Days()
    {
        var notices = new InMemoryNoticeRepository();
        await new BookingCreatedHandler(notices, new FixedClock(Now)).HandleAsync(BookingCreated());

        var notice = Assert.Single(notices.Notices);
        Assert.Equal("booking-created-b-1", notice.Id);
        Assert.Equal("client-1", notice.UserId);
        Assert.Equal("booking", notice.Kind);
        Assert.False(notice.Read);
        Assert.Contains("VG-000123", notice.Body, StringComparison.Ordinal);
        Assert.Equal(Now.AddDays(90), notice.PurgeAt);
    }

    [Fact]
    public async Task BookingCreatedTwiceKeepsASingleNotice()
    {
        var notices = new InMemoryNoticeRepository();
        var handler = new BookingCreatedHandler(notices, new FixedClock(Now));

        await handler.HandleAsync(BookingCreated());
        await handler.HandleAsync(BookingCreated());

        Assert.Single(notices.Notices);
    }

    [Fact]
    public async Task DispatcherDiscardsUnknownTypesAndMalformedBodies()
    {
        var notices = new InMemoryNoticeRepository();
        var dispatcher = Dispatcher(notices);

        await dispatcher.DispatchAsync(new JobMessage("does.not.exist").ToJson());
        await dispatcher.DispatchAsync("{ broken");
        await dispatcher.DispatchAsync(new JobMessage(JobTypes.BookingCreated, new Dictionary<string, string>()).ToJson());

        Assert.Empty(notices.Notices);
    }

    [Fact]
    public async Task DispatcherRunsTheHandlerForTheType()
    {
        var notices = new InMemoryNoticeRepository();

        await Dispatcher(notices).DispatchAsync(BookingCreated().ToJson());

        Assert.Single(notices.Notices);
    }

    [Fact]
    public async Task DispatcherLetsHandlerFailuresThroughSoTheMessageIsRetried()
    {
        var sender = new CapturingEmailSender { Fail = true };
        var dispatcher = new JobDispatcher(
            [new EmailSendHandler(sender, NullLogger<EmailSendHandler>.Instance)],
            NullLogger<JobDispatcher>.Instance);

        await Assert.ThrowsAsync<InvalidOperationException>(() => dispatcher.DispatchAsync(EmailSend().ToJson()));
    }

    [Theory]
    [InlineData(EmailTemplates.PasswordReset, "contraseña")]
    [InlineData(EmailTemplates.EmailVerification, "Verifica")]
    public async Task EmailSendBuildsTheTemplateWithTheCode(string template, string subjectFragment)
    {
        var sender = new CapturingEmailSender();

        await new EmailSendHandler(sender, NullLogger<EmailSendHandler>.Instance).HandleAsync(EmailSend(template));

        var email = Assert.Single(sender.Sent);
        Assert.Equal("ana.perez@gmail.com", email.To);
        Assert.Contains(subjectFragment, email.Subject, StringComparison.Ordinal);
        Assert.Contains("482913", email.TextBody, StringComparison.Ordinal);
        Assert.Contains("482913", email.HtmlBody, StringComparison.Ordinal);
        Assert.Contains("15 minutos", email.TextBody, StringComparison.Ordinal);
    }

    [Fact]
    public async Task EmailSendRejectsUnknownTemplates()
    {
        var sender = new CapturingEmailSender();

        await Assert.ThrowsAsync<InvalidJobMessageException>(() =>
            new EmailSendHandler(sender, NullLogger<EmailSendHandler>.Instance).HandleAsync(EmailSend("marketing")));

        Assert.Empty(sender.Sent);
    }

    [Fact]
    public async Task EmailLogsNeverContainTheCodeOrTheFullAddress()
    {
        var provider = new CapturingLoggerProvider();
        using var factory = LoggerFactory.Create(builder => builder.AddProvider(provider));

        await new EmailSendHandler(new CapturingEmailSender(), factory.CreateLogger<EmailSendHandler>()).HandleAsync(EmailSend());

        var line = Assert.Single(provider.Lines);
        Assert.DoesNotContain("482913", line, StringComparison.Ordinal);
        Assert.DoesNotContain("ana.perez", line, StringComparison.Ordinal);
        Assert.Contains("a***@gmail.com", line, StringComparison.Ordinal);
    }

    [Theory]
    [InlineData("2026-11-12T03:00:00Z", "2026-11-11")]
    [InlineData("2026-11-12T06:00:00Z", "2026-11-12")]
    public void MexicoCityTodayUsesUtcMinusSix(string utc, string expected) =>
        Assert.Equal(
            DateOnly.Parse(expected, System.Globalization.CultureInfo.InvariantCulture),
            QuotesExpireHandler.MexicoCityToday(DateTime.Parse(utc, System.Globalization.CultureInfo.InvariantCulture, System.Globalization.DateTimeStyles.AdjustToUniversal)));

    [Fact]
    public async Task QuotesExpireOnlyTouchesOpenQuotesWithAPastEventDate()
    {
        var quotes = new InMemoryQuoteRepository();
        await quotes.AddAsync(Quote("past-sent", "sent", "2026-11-10"));
        await quotes.AddAsync(Quote("past-answered", "answered", "2026-11-01"));
        await quotes.AddAsync(Quote("past-accepted", "accepted", "2026-11-10"));
        await quotes.AddAsync(Quote("today-sent", "sent", "2026-11-11"));
        await quotes.AddAsync(Quote("future-sent", "sent", "2027-01-01"));
        var handler = new QuotesExpireHandler(quotes, new FixedClock(Now), NullLogger<QuotesExpireHandler>.Instance);

        await handler.HandleAsync(new JobMessage(JobTypes.QuotesExpire));

        var status = quotes.Snapshot().ToDictionary(quote => quote.Id, quote => quote.Status);
        Assert.Equal("expired", status["past-sent"]);
        Assert.Equal("expired", status["past-answered"]);
        Assert.Equal("accepted", status["past-accepted"]);
        Assert.Equal("sent", status["today-sent"]);
        Assert.Equal("sent", status["future-sent"]);
        Assert.Equal(0, await quotes.ExpireBeforeAsync(QuotesExpireHandler.MexicoCityToday(Now), Now));
    }

    [Fact]
    public async Task SqsPublisherIsFailOpen()
    {
        var sqs = new FailingSqsClient();
        var publisher = new SqsJobPublisher(sqs, "https://sqs.us-east-1.amazonaws.com/000000000000/virgo-jobs", NullLogger<SqsJobPublisher>.Instance);

        await publisher.PublishAsync(BookingCreated());

        Assert.Equal(1, sqs.Attempts);
    }

    [Fact]
    public async Task SqsPublisherSendsTheTypeAndCorrelationAsAttributes()
    {
        var sqs = new RecordingSqsClient();
        var publisher = new SqsJobPublisher(sqs, "https://queue", NullLogger<SqsJobPublisher>.Instance);

        await publisher.PublishAsync(BookingCreated());

        var request = Assert.Single(sqs.Requests);
        Assert.Equal("https://queue", request.QueueUrl);
        Assert.Equal(JobTypes.BookingCreated, request.MessageAttributes["type"].StringValue);
        Assert.Equal("corr-1", request.MessageAttributes["correlationId"].StringValue);
        Assert.Equal("b-1", JobMessage.Parse(request.MessageBody).Get("bookingId"));
    }

    [Fact]
    public async Task WorkerReportsOnlyTheFailedMessages()
    {
        var sender = new CapturingEmailSender { Fail = true };
        await using var services = WorkerServices(sender);
        var function = new JobsFunction(services);
        var sqsEvent = new SQSEvent
        {
            Records =
            [
                new SQSEvent.SQSMessage { MessageId = "ok", Body = BookingCreated().ToJson() },
                new SQSEvent.SQSMessage { MessageId = "unknown", Body = new JobMessage("does.not.exist").ToJson() },
                new SQSEvent.SQSMessage { MessageId = "fails", Body = EmailSend().ToJson() }
            ]
        };

        var response = await function.FunctionHandler(sqsEvent, null!);

        var failure = Assert.Single(response.BatchItemFailures);
        Assert.Equal("fails", failure.ItemIdentifier);
        Assert.Single(services.GetRequiredService<InMemoryNoticeRepository>().Notices);
    }

    private static JobDispatcher Dispatcher(InMemoryNoticeRepository notices) => new(
        [new BookingCreatedHandler(notices, new FixedClock(Now))],
        NullLogger<JobDispatcher>.Instance);

    private static JobMessage EmailSend(string template = EmailTemplates.PasswordReset) => new(
        JobTypes.EmailSend,
        new Dictionary<string, string>
        {
            ["template"] = template,
            ["to"] = "ana.perez@gmail.com",
            ["code"] = "482913",
            ["expiresInMinutes"] = "15"
        },
        "corr-2");

    private static Quote Quote(string id, string status, string eventDate) => new()
    {
        Id = id,
        ClientId = "client-1",
        ClientName = "Ana",
        TargetType = "venue",
        TargetId = "venue-1",
        TargetName = "Salón",
        EventType = "Boda",
        EventDate = eventDate,
        Guests = 100,
        RequestedServices = [],
        Status = status,
        CreatedAt = Now.AddDays(-30),
        UpdatedAt = Now.AddDays(-30)
    };

    private static ServiceProvider WorkerServices(IEmailSender sender)
    {
        var configuration = new ConfigurationBuilder()
            .AddInMemoryCollection(new Dictionary<string, string?> { ["Persistence:Provider"] = "InMemory" })
            .Build();
        var services = new ServiceCollection();
        services.AddLogging();
        services.AddInfrastructure(configuration);
        services.AddSingleton<InMemoryNoticeRepository>();
        services.AddSingleton<INoticeRepository>(provider => provider.GetRequiredService<InMemoryNoticeRepository>());
        services.AddSingleton(sender);
        services.AddSingleton<IClock>(new FixedClock(Now));
        return services.BuildServiceProvider();
    }
}
