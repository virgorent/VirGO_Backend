using System.Text.Json;
using VirGo.Application.Jobs;

namespace VirGo.UnitTests.Jobs;

public sealed class DeterministicNoticeIdsTests
{
    private const string BookingId = "4bf8f8c2-1c5a-4e3b-8b2a-7f9f3d1c2e44";
    private const string OtherBookingId = "c6d2a8b4-9f10-4d7e-a3c5-8b1e6f2d4a90";

    [Fact]
    public void ImplementsTheRfc4122UuidV5Vector()
    {
        var value = DeterministicNoticeIds.FromName(DeterministicNoticeIds.Namespace, "www.example.com");

        Assert.Equal("2ed6657d-e927-568b-95e1-2665a8aea6a2", value.ToString("D"));
    }

    [Fact]
    public void BookingCreatedUsesTheCanonicalKeyAndFrozenNamespace()
    {
        var expected = DeterministicNoticeIds.FromName(
            DeterministicNoticeIds.Namespace,
            DeterministicNoticeIds.BookingCreatedKeyPrefix + BookingId);

        Assert.Equal(expected, DeterministicNoticeIds.ForBookingCreated(BookingId));
        Assert.Equal("238b0bd9-52c0-5976-86f0-b2b4a220b44a", DeterministicNoticeIds.ForBookingCreated(BookingId).ToString("D"));
    }

    [Fact]
    public void SameBookingIdProducesTheSameNoticeId()
    {
        Assert.Equal(
            DeterministicNoticeIds.ForBookingCreated(BookingId),
            DeterministicNoticeIds.ForBookingCreated(BookingId));
    }

    [Fact]
    public void DifferentBookingIdsProduceDifferentNoticeIds()
    {
        Assert.NotEqual(
            DeterministicNoticeIds.ForBookingCreated(BookingId),
            DeterministicNoticeIds.ForBookingCreated(OtherBookingId));
    }

    [Fact]
    public void CanonicalKeyContainsTheBookingCreatedPrefix()
    {
        Assert.Equal("booking.created:", DeterministicNoticeIds.BookingCreatedKeyPrefix);
        Assert.Equal(
            DeterministicNoticeIds.FromName(DeterministicNoticeIds.Namespace, $"booking.created:{BookingId}"),
            DeterministicNoticeIds.ForBookingCreated(BookingId));
    }

    [Fact]
    public void GeneratedIdMatchesTheNoticesSchemaPattern()
    {
        using var document = JsonDocument.Parse(File.ReadAllText(FindNoticesSchema()));
        var pattern = document.RootElement
            .GetProperty("validator")
            .GetProperty("$jsonSchema")
            .GetProperty("properties")
            .GetProperty("_id")
            .GetProperty("pattern")
            .GetString();

        Assert.False(string.IsNullOrWhiteSpace(pattern));
        Assert.Matches(pattern, DeterministicNoticeIds.ForBookingCreated(BookingId).ToString("D"));
    }

    [Fact]
    public void GeneratedIdHasUuidV5VersionAndRfcVariant()
    {
        var bytes = DeterministicNoticeIds.ForBookingCreated(BookingId).ToByteArray();

        Assert.Equal(5, bytes[7] >> 4);
        Assert.Equal(0x80, bytes[8] & 0xc0);
    }

    private static string FindNoticesSchema()
    {
        var directory = new DirectoryInfo(AppContext.BaseDirectory);
        while (directory is not null)
        {
            var candidate = Path.Combine(directory.FullName, "db", "schema", "notices.json");
            if (File.Exists(candidate))
            {
                return candidate;
            }

            directory = directory.Parent;
        }

        throw new FileNotFoundException("No se encontró db/schema/notices.json.");
    }
}
