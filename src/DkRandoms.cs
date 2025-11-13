using System.Security.Cryptography;

#pragma warning disable IDE0130 // Namespace がフォルダー構造と一致しません
namespace Tool.Compet.Core;

/// <summary>
/// Random value generator.
/// </summary>
public class DkRandoms {
	/// <summary>
	/// Gen Base62 random string at given length.
	/// It provides strong randomness from crypto RandomNumberGenerator class.
	/// </summary>
	public static string RandomBase62(int length = 12) {
		var key = new char[length];
		var bytes = new byte[length];

		RandomNumberGenerator.Fill(bytes);

		for (var index = 0; index < length; ++index) {
			key[index] = DkBase62.Base62Chars[bytes[index] % DkBase62.Base62Chars.Length];
		}

		return new string(key);
	}

	/// <summary>
	/// Gen Base64 random string at given length.
	/// Note: generated string can contain some non-alphabet chars, so when store at
	/// database, that chars maybe encoded to more bytes (not 1 byte) ! so take care length of it.
	/// </summary>
	/// <param name="length"></param>
	/// <returns></returns>
	public static string RandomBase64(int length = 8) {
		return Convert.ToBase64String(RandomNumberGenerator.GetBytes(length));
	}

	// private static string SecretKey;
	// private static readonly object LockObject = new();
	// private static long Counter = DateTime.UtcNow.Ticks;
	//
	// public static void ConfigureIdGenerator(AppSetting appSetting) {
	// 	SecretKey = "adsjkl1312j3xxjaskfjkasf92491204";
	// }
	//
	// private static string GenerateId_HMAC() {
	// 	lock (LockObject) {
	// 		// Increment counter for uniqueness in the same timestamp
	// 		Counter++;
	//
	// 		// Combine timestamp and counter for input
	// 		var timestamp = BitConverter.GetBytes(DateTime.UtcNow.Ticks);
	// 		var counterBytes = BitConverter.GetBytes(Counter);
	// 		var data = timestamp.Concat(counterBytes).ToArray();
	//
	// 		// Compute HMAC-SHA-256
	// 		using var hmac = new HMACSHA256(Encoding.UTF8.GetBytes(SecretKey));
	// 		var hashBytes = hmac.ComputeHash(data);
	//
	// 		return Base62Encode(hashBytes);
	// 	}
	// }
}
