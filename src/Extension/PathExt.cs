#pragma warning disable IDE0130 // Namespace がフォルダー構造と一致しません
namespace Tool.Compet.Core;

using System.Runtime.CompilerServices;

/// <summary>
/// Extension for path OS.
/// </summary>
public static class PathExt {
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static string FixPathSeparatorDk(this string path) {
		return path.Replace('/', Path.DirectorySeparatorChar).Replace('\\', Path.DirectorySeparatorChar);
	}
}
