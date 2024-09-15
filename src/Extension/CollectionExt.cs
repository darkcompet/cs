#pragma warning disable IDE0130 // Namespace がフォルダー構造と一致しません
namespace Tool.Compet.Core;

/// Extension for collection (list, set, map,...).
public static class CollectionExt {
	/// <summary>
	/// Check the collection is not null and not empty.
	/// </summary>
	/// <typeparam name="T"></typeparam>
	/// <param name="list"></param>
	/// <returns></returns>
	public static bool IsEmptyDk<T>(this List<T>? list) {
		return list == null || list.Count == 0;
	}

	/// <summary>
	/// Remove element at given index without re-arrange elements in [index, lastIndex].
	/// This is faster than normal remove, but should only use if you don't care about element's order after the index.
	/// </summary>
	/// <typeparam name="T"></typeparam>
	/// <param name="list"></param>
	/// <param name="index"></param>
	public static void FastRemoveDk<T>(this List<T> list, int index) {
		var lastIndex = list.Count - 1;
		if (index >= 0) {
			if (index == lastIndex) {
				list.RemoveAt(index);
			}
			else if (index < lastIndex) {
				list[index] = list[lastIndex];
				list.RemoveAt(lastIndex);
			}
		}
	}

	/// <summary>
	/// Remove last element from given list.
	/// The list must contain at least one element. Otherwise throw exception.
	/// </summary>
	/// <typeparam name="T"></typeparam>
	/// <param name="list"></param>
	/// <returns></returns>
	public static T RemoveLastDk<T>(this List<T> list) {
		var last = list[^1];
		list.RemoveAt(list.Count - 1);
		return last;
	}

	/// <summary>
	/// Convert collection `values` to map of: key, values.
	/// </summary>
	/// <typeparam name="TKey"></typeparam>
	/// <typeparam name="TValue"></typeparam>
	/// <param name="values"></param>
	/// <param name="CalcKey">Calculate key that maps with the value</param>
	/// <returns></returns>
	public static DkHashMap<TKey, List<TValue>> GroupByDk<TKey, TValue>(this IEnumerable<TValue> values, Func<TValue, TKey> CalcKey) where TKey : notnull {
		var key2values = new DkHashMap<TKey, List<TValue>>();
		foreach (var item in values) {
			var key = CalcKey(item);
			var list = key2values[key];
			if (list != null) {
				list.Add(item);
			}
			else {
				key2values[key] = [item];
			}
		}
		return key2values;
	}
}
