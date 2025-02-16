#pragma warning disable IDE0130 // Namespace がフォルダー構造と一致しません
namespace Tool.Compet.Core;

/// Extension for string.
public static class DkDateTimes {
	public const string FMT_DATE = "yyyy-MM-dd";
	public const string FMT_TIME = "HH:mm:ss";
	public const string FMT_DATETIME = "yyyy-MM-dd HH:mm:ss";

	/// <summary>
	/// Epoch time in milliseconds of now (number of milliseconds has elapsed from 1970-01-01T00:00:00.000Z).
	/// Ref: https://learn.microsoft.com/en-us/dotnet/api/system.datetimeoffset.tounixtimeseconds?view=net-7.0
	/// </summary>
	public static long currentUnixTimeInMilliseconds => DateTimeOffset.Now.ToUnixTimeMilliseconds();

	/// <summary>
	/// Epoch time in seconds of now (number of seconds has elapsed from 1970-01-01T00:00:00.000Z).
	/// Ref: https://learn.microsoft.com/en-us/dotnet/api/system.datetimeoffset.tounixtimeseconds?view=net-7.0
	/// </summary>
	public static long currentUnixTimeInSeconds => DateTimeOffset.Now.ToUnixTimeSeconds();

	/// <summary>
	/// Epoch time in milliseconds of UTC-now (number of milliseconds has elapsed from 1970-01-01T00:00:00.000Z).
	/// Ref: https://learn.microsoft.com/en-us/dotnet/api/system.datetimeoffset.tounixtimeseconds?view=net-7.0
	/// </summary>
	public static long currentUnixUtcTimeInMilliseconds => DateTimeOffset.UtcNow.ToUnixTimeMilliseconds();

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

	public static long CalcTimestampMilliseconds(DateTime end, DateTime start) {
		return (long)(end - start).TotalMilliseconds;
	}

	public static long CalcTimestampMilliseconds(DateTimeOffset end, DateTimeOffset start) {
		return (long)(end - start).TotalMilliseconds;
	}

	public static long CalcTimestampMicroseconds(DateTime end, DateTime start) {
		return (long)(end - start).TotalMicroseconds;
	}

	public static long CalcTimestampMicroseconds(DateTimeOffset end, DateTimeOffset start) {
		return (long)(end - start).TotalMicroseconds;
	}

	public static long CalcTimestampNanoseconds(DateTime end, DateTime start) {
		return (long)(end - start).TotalNanoseconds;
	}

	public static long CalcTimestampNanoseconds(DateTimeOffset end, DateTimeOffset start) {
		return (long)(end - start).TotalNanoseconds;
	}
}
