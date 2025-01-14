#pragma warning disable IDE0130 // Namespace がフォルダー構造と一致しません
namespace Tool.Compet.Core;

using System.Runtime.CompilerServices;

/// <summary>
/// Extension for path OS.
/// </summary>
public static class PathExt {
	/// <summary>
	/// Fix path separator by replace directory separator to match with current OS (for cross-platform purpose).
	/// </summary>
	/// <param name="path"></param>
	/// <returns></returns>
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static string FixPathSeparatorDk(this string path) {
		return path.Replace(Path.DirectorySeparatorChar == '/' ? '\\' : '/', Path.DirectorySeparatorChar);
	}
}
