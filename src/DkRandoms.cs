#pragma warning disable IDE0130 // Namespace がフォルダー構造と一致しません
namespace Tool.Compet.Core;

using System.Security.Cryptography;

public class DkRandoms {
	/// <summary>
	/// At given timestamp (8 bytes), it merges with suffix as random string in Base62.
	/// </summary>
	/// <param name="randomLength"></param>
	/// <returns></returns>
	public static string GenAtTimestamp(long timestamp, int randomLength) {
		const int timestampLength = 8;
		Span<byte> data = stackalloc byte[timestampLength + randomLength];

		// Convert timestamp to bytes (Little Endian by default)
		Span<byte> timestampBytes = stackalloc byte[timestampLength];
		BitConverter.TryWriteBytes(timestampBytes, timestamp);

		// Only reverse if system is Little Endian
		if (BitConverter.IsLittleEndian) {
			timestampBytes.Reverse();
		}

		// Copy timestamp bytes to data
		timestampBytes.CopyTo(data);

		// 2. Generate random bytes
		RandomNumberGenerator.Fill(data.Slice(timestampLength, randomLength));

		// Span<byte> valueBytes = BitConverter.GetBytes(long.MaxValue);
		// Ensure the data span is large enough
		// Fill the Span at position 8 (index 7)
		// valueBytes.CopyTo(data.Slice(timestampLength, valueBytes.Length));

		// 3. Encode as base62
		return DkBase62.Encode(data.ToArray());
	}
}
