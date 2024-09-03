#pragma warning disable IDE0130 // Namespace がフォルダー構造と一致しません
namespace Tool.Compet.Core;

using System.Globalization;

/// Extension for string.
public static class DkDateTimes {
	public const string FMT_DATE = "yyyy-MM-dd";
	public const string FMT_TIME = "HH:mm:ss";
	public const string FMT_DATETIME = "yyyy-MM-dd HH:mm:ss";

	/// <summary>
	/// Format datatime in given format.
	/// Ref: https://docs.microsoft.com/en-us/dotnet/standard/base-types/custom-date-and-time-format-strings
	/// Ref: https://www.c-sharpcorner.com/blogs/date-and-time-format-in-c-sharp-programming1
	/// </summary>
	/// <param name="me"></param>
	/// <param name="format">For eg,. yyyy-MM-dd HH:mm:ss</param>
	/// <returns></returns>
	public static string FormatDk(this DateTime me, string? format = FMT_DATETIME) {
		return me.ToString(format, CultureInfo.InvariantCulture);
	}

	/// <summary>
	/// Epoch time in milliseconds of now (number of milliseconds has elapsed from 1970-01-01T00:00:00.000Z).
	/// Ref: https://learn.microsoft.com/en-us/dotnet/api/system.datetimeoffset.tounixtimeseconds?view=net-7.0
	/// </summary>
	public static long currentUnixTimeInMillis => DateTimeOffset.Now.ToUnixTimeMilliseconds();

	/// <summary>
	/// Epoch time in seconds of now (number of seconds has elapsed from 1970-01-01T00:00:00.000Z).
	/// Ref: https://learn.microsoft.com/en-us/dotnet/api/system.datetimeoffset.tounixtimeseconds?view=net-7.0
	/// </summary>
	public static long currentUnixTimeInSeconds => DateTimeOffset.Now.ToUnixTimeSeconds();

	/// <summary>
	/// Epoch time in milliseconds of UTC-now (number of milliseconds has elapsed from 1970-01-01T00:00:00.000Z).
	/// Ref: https://learn.microsoft.com/en-us/dotnet/api/system.datetimeoffset.tounixtimeseconds?view=net-7.0
	/// </summary>
	public static long currentUnixUtcTimeInMillis => DateTimeOffset.UtcNow.ToUnixTimeMilliseconds();

	/// <summary>
	/// Epoch time in seconds of UTC-now (number of seconds has elapsed from 1970-01-01T00:00:00.000Z).
	/// Ref: https://learn.microsoft.com/en-us/dotnet/api/system.datetimeoffset.tounixtimeseconds?view=net-7.0
	/// </summary>
	public static long currentUnixUtcTimeInSeconds => DateTimeOffset.UtcNow.ToUnixTimeSeconds();

	/// <summary>
	/// Calculate datetime of unix time (in seconds) that elapsed from epoch.
	/// </summary>
	/// <param name="seconds"></param>
	/// <returns></returns>
	public static DateTime ConvertUnixTimeSecondsToUtcDatetime(long seconds) {
		return DateTimeOffset.FromUnixTimeSeconds(seconds).UtcDateTime;
	}

	/// <summary>
	/// Calculate datetime of unix time (in milliseconds) that elapsed from epoch.
	/// </summary>
	/// <param name="millis"></param>
	/// <returns></returns>
	public static DateTime ConvertUnixTimeMillisecondsToUtcDatetime(long millis) {
		return DateTimeOffset.FromUnixTimeMilliseconds(millis).UtcDateTime;
	}
}
