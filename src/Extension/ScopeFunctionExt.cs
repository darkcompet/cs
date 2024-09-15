#pragma warning disable IDE0130 // Namespace がフォルダー構造と一致しません
namespace Tool.Compet.Core;

/// <summary>
/// Cleaner coding style with scope functions.
/// Reference: Kotlin's also(), let(),...
/// </summary>
public static class ScopeFunctionExt {
	/// <summary>
	/// Pass upstream object to caller, and return the object to downstream.
	/// </summary>
	/// <typeparam name="T"></typeparam>
	/// <param name="self"></param>
	/// <param name="block"></param>
	/// <returns></returns>
	public static T ThenDk<T>(this T self, System.Action<T> block) {
		block(self);
		return self;
	}

	/// <summary>
	/// Pass upstream object to caller, and pass transformed object to downstream.
	/// </summary>
	/// <typeparam name="T"></typeparam>
	/// <typeparam name="R"></typeparam>
	/// <param name="self"></param>
	/// <param name="mapper"></param>
	/// <returns></returns>
	public static R MapDk<T, R>(this T self, System.Func<T, R> mapper) {
		return mapper(self);
	}
}
