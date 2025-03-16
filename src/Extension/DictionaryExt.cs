#pragma warning disable IDE0130 // Namespace がフォルダー構造と一致しません
namespace Tool.Compet.Core;

public static class DictionaryExt {
	/// <summary>
	/// Upsert pairs to the dictionary.
	/// </summary>
	/// <typeparam name="TKey"></typeparam>
	/// <typeparam name="TValue"></typeparam>
	/// <param name="me"></param>
	/// <param name="elements"></param>
	public static void PutElementsDk<TKey, TValue>(this Dictionary<TKey, TValue> me, Dictionary<TKey, TValue> elements) {
		foreach (var (key, value) in elements) {
			me[key] = value;
		}
	}

	/// <summary>
	/// Try add (skip over-write for existed entry) given pairs to the dictionary.
	/// </summary>
	/// <typeparam name="TKey"></typeparam>
	/// <typeparam name="TValue"></typeparam>
	/// <param name="me"></param>
	/// <param name="elements"></param>
	public static void TryAddElementsDk<TKey, TValue>(this Dictionary<TKey, TValue> me, Dictionary<TKey, TValue> elements) {
		foreach (var (key, value) in elements) {
			me.TryAdd(key, value);
		}
	}
}
