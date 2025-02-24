#pragma warning disable IDE0130 // Namespace がフォルダー構造と一致しません
namespace Tool.Compet.Core;

using System.Buffers.Binary;
using System.Security.Cryptography;

public class DkRandoms {
	private const int TimestampByteCount = 8;

	/// <summary>
	/// Generates a unique identifier using a given timestamp (8 bytes)
	/// and a random suffix of the specified length.
	/// </summary>
	/// <param name="timestamp">The timestamp or sequential number (8 bytes).</param>
	/// <param name="randomLength">The number of random bytes to append.</param>
	/// <returns>A byte array containing the 8-byte timestamp followed by random bytes.</returns>
	public static byte[] GenRandomWithTimestamp(long timestamp, int randomLength) {
		if (randomLength < 0) {
			throw new ArgumentOutOfRangeException(nameof(randomLength), "Random length cannot be negative.");
		}

		var totalLength = TimestampByteCount + randomLength;
		Span<byte> data = stackalloc byte[totalLength];

		// Write timestamp directly in Big-Endian format
		BinaryPrimitives.WriteInt64BigEndian(data, timestamp);

		// Generate random bytes
		RandomNumberGenerator.Fill(data.Slice(TimestampByteCount, randomLength));

		return data.ToArray();
	}
}
