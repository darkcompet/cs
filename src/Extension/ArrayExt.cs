#pragma warning disable IDE0130 // Namespace がフォルダー構造と一致しません
namespace Tool.Compet.Core;

/// <summary>
/// Extension for array.
/// </summary>
public static class ArrayExt {
	/// <summary>
	/// Check the array is not null and not empty.
	/// </summary>
	/// <typeparam name="T"></typeparam>
	/// <param name="arr"></param>
	/// <returns></returns>
	public static bool IsEmptyDk<T>(this T[]? arr) {
		return arr == null || arr.Length == 0;
	}
}
