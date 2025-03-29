#pragma warning disable IDE0130 // Namespace がフォルダー構造と一致しません
namespace Tool.Compet.Core;

using System.Numerics;

public class DkBase62 {
	public const int BASE_NUM = 62;
	public static readonly string Base62Chars = "0123456789abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ";

	/// <summary>
	/// Precompute lookup table for finding index of Base62 char.
	/// It does support only ASCII range.
	/// </summary>
	private static readonly int[] Base62Indices = new int[128];

	static DkBase62() {
		for (var index = Base62Chars.Length - 1; index >= 0; --index) {
			Base62Indices[Base62Chars[index]] = index;
		}
	}

	/// <summary>
	/// Encode given bytes to string in Base62 chars.
	/// It takes about 0.9 micro-seconds !
	/// </summary>
	/// <param name="data"></param>
	/// <returns></returns>
	public static string Encode(byte[] data, int initCapacity = 22, bool isUnsigned = false, bool isBigEndian = false) {
		var base62 = new List<char>(initCapacity);

		// Build number for given bytes
		var base62Number = BigInteger.Abs(new BigInteger(data, isUnsigned: isUnsigned, isBigEndian: isBigEndian));

		// Handle zero case
		if (base62Number == 0) {
			return "0";
		}

		// Convert each number's digit to char (build in reverse order)
		while (base62Number > 0) {
			base62Number = BigInteger.DivRem(base62Number, BASE_NUM, out var remainder);
			base62.Add(Base62Chars[(int)remainder]);
		}

		// Reverse digits
		base62.Reverse();

		return new string([.. base62]);
	}

	/// <summary>
	/// Convert Base62 back to 16-byte array.
	/// It takes about 0.7 micro-seconds !
	/// </summary>
	/// <param name="base62"></param>
	/// <param name="fixedLength">Fixed length of output</param>
	/// <returns></returns>
	public static byte[] Decode(string base62, int fixedLength, bool isUnsigned = false, bool isBigEndian = false) {
		var num = new BigInteger(0);
		foreach (var c in base62) {
			num = (num * BASE_NUM) + Base62Indices[c];
		}

		// Convert to bytes (Big-Endian order)
		var bytes = num.ToByteArray(isUnsigned: isUnsigned, isBigEndian: isBigEndian).AsSpan();

		// Ensure exactly `fixedLength` bytes by right-aligning
		Span<byte> buffer = stackalloc byte[fixedLength];

		var offset = fixedLength - bytes.Length;
		if (offset >= 0) {
			bytes.CopyTo(buffer[offset..]);
		}
		else {
			bytes.Slice(-offset, fixedLength).CopyTo(buffer);
		}

		return buffer.ToArray();
	}
}
