#pragma warning disable IDE0130 // Namespace がフォルダー構造と一致しません
namespace Tool.Compet.Core;

using System.Globalization;

/// <summary>
/// Extension for DateTime.
/// </summary>
public static class DateTimeExt {
	/// <summary>
	/// Format datatime in given format.
	/// Ref: https://docs.microsoft.com/en-us/dotnet/standard/base-types/custom-date-and-time-format-strings
	/// Ref: https://www.c-sharpcorner.com/blogs/date-and-time-format-in-c-sharp-programming1
	/// </summary>
	/// <param name="me"></param>
	/// <param name="format">For eg,. yyyy-MM-dd HH:mm:ss</param>
	/// <returns></returns>
	public static string FormatDk(this DateTime me, string? format = DkDateTimes.FMT_DATETIME) {
		return me.ToString(format, CultureInfo.InvariantCulture);
	}
}
