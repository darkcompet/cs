#pragma warning disable IDE0130 // Namespace がフォルダー構造と一致しません
namespace Tool.Compet.Core;

using System.Numerics;

public class DkBase62 {
	public const int BASE = 62;
	public static readonly string Base62Chars = "0123456789abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ";

	/// <summary>
	/// Encode given bytes to string in Base62 chars.
	/// </summary>
	/// <param name="data"></param>
	/// <returns></returns>
	public static string Encode(byte[] data) {
		var result = new List<char>(32);

		// Build number for given bytes
		var base62Number = new BigInteger(data, isBigEndian: true);

		// Convert each number's digit to char (build in reverse order)
		while (base62Number > 0) {
			base62Number = BigInteger.DivRem(base62Number, BASE, out var remainder);
			result.Add(Base62Chars[(int)remainder]);
		}

		// Reverse digits
		result.Reverse();

		return new string([.. result]);
	}
}
