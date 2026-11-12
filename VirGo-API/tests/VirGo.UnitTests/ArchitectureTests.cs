using System.Reflection;

namespace VirGo.UnitTests;

public sealed class ArchitectureTests
{
    [Fact]
    public void DomainHasNoOutwardProjectDependencies() => Assert.Empty(ProjectReferences("VirGo.Domain"));

    [Fact]
    public void ApplicationDependsOnlyOnDomain() =>
        Assert.Equal(["VirGo.Domain"], ProjectReferences("VirGo.Application"));

    [Fact]
    public void InfrastructureDoesNotDependOnTheApi() =>
        Assert.DoesNotContain("VirGo.Api", ProjectReferences("VirGo.Infrastructure"));

    [Theory]
    [InlineData("VirGo.Domain")]
    [InlineData("VirGo.Application")]
    public void InnerLayersDoNotKnowTheDatabaseOrCache(string assemblyName)
    {
        var forbidden = Assembly.Load(assemblyName)
            .GetReferencedAssemblies()
            .Select(assembly => assembly.Name ?? string.Empty)
            .Where(name => name.StartsWith("MongoDB", StringComparison.Ordinal)
                || name.StartsWith("StackExchange", StringComparison.Ordinal)
                || name.StartsWith("Amazon", StringComparison.Ordinal));

        Assert.Empty(forbidden);
    }

    private static string[] ProjectReferences(string assemblyName) =>
        Assembly.Load(assemblyName)
            .GetReferencedAssemblies()
            .Select(assembly => assembly.Name!)
            .Where(name => name.StartsWith("VirGo.", StringComparison.Ordinal))
            .ToArray();
}
