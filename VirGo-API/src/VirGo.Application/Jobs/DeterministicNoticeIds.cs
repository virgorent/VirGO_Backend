using System.Security.Cryptography;
using System.Text;

namespace VirGo.Application.Jobs;

public static class DeterministicNoticeIds
{
    public static readonly Guid Namespace = Guid.Parse("6ba7b810-9dad-11d1-80b4-00c04fd430c8");
    public const string BookingCreatedKeyPrefix = "booking.created:";

    public static Guid ForBookingCreated(string bookingId) =>
        FromName(Namespace, BookingCreatedKeyPrefix + bookingId);

    public static Guid FromName(Guid namespaceId, string name)
    {
        var namespaceBytes = namespaceId.ToByteArray();
        SwapByteOrder(namespaceBytes);

        var nameBytes = Encoding.UTF8.GetBytes(name);
        var buffer = new byte[namespaceBytes.Length + nameBytes.Length];
        Buffer.BlockCopy(namespaceBytes, 0, buffer, 0, namespaceBytes.Length);
        Buffer.BlockCopy(nameBytes, 0, buffer, namespaceBytes.Length, nameBytes.Length);

#pragma warning disable CA5350
        var hash = SHA1.HashData(buffer);
#pragma warning restore CA5350
        var result = new byte[16];
        Buffer.BlockCopy(hash, 0, result, 0, result.Length);
        result[6] = (byte)((result[6] & 0x0f) | 0x50);
        result[8] = (byte)((result[8] & 0x3f) | 0x80);
        SwapByteOrder(result);

        return new Guid(result);
    }

    private static void SwapByteOrder(byte[] bytes)
    {
        Swap(bytes, 0, 3);
        Swap(bytes, 1, 2);
        Swap(bytes, 4, 5);
        Swap(bytes, 6, 7);
    }

    private static void Swap(byte[] bytes, int left, int right) =>
        (bytes[left], bytes[right]) = (bytes[right], bytes[left]);
}
