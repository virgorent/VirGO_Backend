using System.Diagnostics;
using System.Text.Json;
using Xunit.Abstractions;

namespace VirGo.PerformanceTests;

// Mide una línea base en proceso. No fija umbrales: el Art. VI.4 pide medir primero.
public sealed class BaselineTests(PerformanceApiFactory factory, ITestOutputHelper output) : IClassFixture<PerformanceApiFactory>
{
    private const int Requests = 300;
    private const int Concurrency = 10;

    [Theory]
    [InlineData("/health")]
    [InlineData("/api/v1/venues")]
    [InlineData("/api/v1/venues?zone=Centro")]
    public async Task MeasuresLatency(string path)
    {
        var client = factory.CreateClient();
        await client.GetAsync(path);
        var samples = new double[Requests];
        var failures = 0;
        var next = -1;

        await Task.WhenAll(Enumerable.Range(0, Concurrency).Select(async _ =>
        {
            int index;
            while ((index = Interlocked.Increment(ref next)) < Requests)
            {
                var watch = Stopwatch.StartNew();
                using var response = await client.GetAsync(path);
                samples[index] = watch.Elapsed.TotalMilliseconds;
                if (!response.IsSuccessStatusCode)
                {
                    Interlocked.Increment(ref failures);
                }
            }
        }));

        Array.Sort(samples);
        var result = new { path, Requests, Concurrency, p50 = samples[Requests / 2], p95 = samples[Requests * 95 / 100], max = samples[^1], failures };
        output.WriteLine(JsonSerializer.Serialize(result));
        Assert.Equal(0, failures);
    }
}
