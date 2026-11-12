using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using VirGo.Infrastructure;

namespace VirGo.Worker;

/// <summary>Builds the worker services with the same configuration as the API: environment variables plus SSM.</summary>
public static class WorkerStartup
{
    public static ServiceProvider BuildServices()
    {
        var builder = new ConfigurationBuilder().AddEnvironmentVariables();
        var parameterPrefix = builder.Build()["Aws:ParameterPrefix"];
        if (!string.IsNullOrWhiteSpace(parameterPrefix))
        {
            builder.AddSystemsManager(parameterPrefix);
        }

        var configuration = builder.Build();
        var services = new ServiceCollection();
        services.AddSingleton<IConfiguration>(configuration);
        services.AddLogging(logging => logging.AddJsonConsole());
        services.AddInfrastructure(configuration);
        return services.BuildServiceProvider();
    }
}
