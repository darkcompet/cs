#pragma warning disable IDE0130 // Namespace がフォルダー構造と一致しません
namespace Tool.Compet.Core;

using System.Runtime.CompilerServices;

/// <summary>
/// Extension for GUID.
/// </summary>
public static class GuidExt {
	/// <summary>
	/// Express GUID in natural string (without hyphen).
	/// </summary>
	/// <param name="me"></param>
	/// <returns></returns>
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static string ToStringDk(this Guid me) {
		return me.ToString("N");
	}
}
