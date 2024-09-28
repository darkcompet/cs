#pragma warning disable IDE0130 // Namespace がフォルダー構造と一致しません
namespace Tool.Compet.Core;

using System.Runtime.CompilerServices;

/// <summary>
/// Extension for string.
/// </summary>
public static class StringExt {
	/// <summary>
	/// Compare 2 strings with `Ordinal` comparision (byte-level).
	/// </summary>
	/// <param name="me"></param>
	/// <param name="other"></param>
	/// <returns></returns>
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static bool EqualsDk(this string? me, string? other) {
		return me == null ? other == null : me.Equals(other, StringComparison.Ordinal);
	}

	/// <summary>
	/// Compare 2 strings with `OrdinalIgnoreCase` comparision (byte-level).
	/// </summary>
	/// <param name="me"></param>
	/// <param name="other"></param>
	/// <returns></returns>
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static bool EqualsIgnoreCaseDk(this string? me, string? other) {
		return me == null ? other == null : me.Equals(other, StringComparison.OrdinalIgnoreCase);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static bool IsEmptyDk(this string? me) {
		return me == null || me.Length == 0;
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static bool IsNotEmptyDk(this string? me) {
		return me != null && me.Length > 0;
	}

	/// <summary>
	/// Check this string starts with given `value` by comparing as `Ordinal` (byte-level).
	/// </summary>
	/// <param name="me"></param>
	/// <param name="value"></param>
	/// <returns></returns>
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static bool StartsWithDk(this string me, string value) {
		return me.StartsWith(value, StringComparison.Ordinal);
	}

	/// <summary>
	/// Check this string ends with given `value` by comparing as `Ordinal` (byte-level).
	/// </summary>
	/// <param name="me"></param>
	/// <param name="value"></param>
	/// <returns></returns>
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static bool EndsWithDk(this string me, string value) {
		return me.EndsWith(value, StringComparison.Ordinal);
	}

	/// <summary>
	/// @param startIndex: Inclusive.
	/// </summary>
	/// <param name="me"></param>
	/// <param name="startIndex"></param>
	/// <returns></returns>
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static string SubstringDk(this string me, int startIndex) {
		return me[startIndex..];
	}

	/// <summary>
	/// </summary>
	/// <param name="me"></param>
	/// <param name="startIndex">Inclusive</param>
	/// <param name="endIndex">Exclusive</param>
	/// <returns></returns>
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static string SubstringDk(this string me, int startIndex, int endIndex) {
		return me[startIndex..endIndex];
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static bool ParseBooleanDk(this string? me, bool defaultValue = false) {
		if (me.EqualsDk("1") || me.EqualsIgnoreCaseDk("true")) {
			return true;
		}
		return defaultValue;
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static byte ParseByteDk(this string? me, byte defaultValue = 0) {
		if (byte.TryParse(me, out var result)) {
			return result;
		}
		return defaultValue;
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static short ParseShortDk(this string? me, short defaultValue = 0) {
		if (short.TryParse(me, out var result)) {
			return result;
		}
		return defaultValue;
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static int ParseIntDk(this string? me, int defaultValue = 0) {
		if (int.TryParse(me, out var result)) {
			return result;
		}
		return defaultValue;
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static long ParseLongDk(this string? me, long defaultValue = 0L) {
		if (long.TryParse(me, out var result)) {
			return result;
		}
		return defaultValue;
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static decimal ParseDecimalDk(this string? me, decimal defaultValue = 0m) {
		if (decimal.TryParse(me, out var result)) {
			return result;
		}
		return defaultValue;
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static float ParseFloatDk(this string? me, float defaultValue = 0F) {
		if (float.TryParse(me, out var result)) {
			return result;
		}
		return defaultValue;
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static double ParseDoubleDk(this string? me, double defaultValue = 0D) {
		if (double.TryParse(me, out var result)) {
			return result;
		}
		return defaultValue;
	}

	/// Get first index of given `element` in the array. Returns -1 if not found.
	public static int IndexOfDk(this string[] arr, string? element) {
		for (int index = 0, N = arr.Length; index < N; ++index) {
			if (arr[index].Equals(element, StringComparison.Ordinal)) {
				return index;
			}
		}
		return -1;
	}

	/// <summary>
	/// Get last index of given `element` in the array. Returns -1 if not found.
	/// </summary>
	/// <param name="arr"></param>
	/// <param name="element"></param>
	/// <returns></returns>
	public static int LastIndexOfDk(this string[] arr, string? element) {
		for (var index = arr.Length - 1; index >= 0; --index) {
			if (arr[index].Equals(element, StringComparison.Ordinal)) {
				return index;
			}
		}
		return -1;
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static Guid? ParseGuidDk(this string? me) {
		return me is null ? null : Guid.Parse(me);
	}
}
