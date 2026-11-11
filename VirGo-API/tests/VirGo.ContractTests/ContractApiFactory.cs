using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;

namespace VirGo.ContractTests;

public sealed class ContractApiFactory : WebApplicationFactory<Program>
{
    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder.UseEnvironment("Testing");
        builder.UseSetting("RateLimiting:PermitLimit", "1000");
        builder.UseSetting("Jwt:SigningKey", "TestingOnlySigningKeyMustBeAtLeast32Characters");
    }
}
