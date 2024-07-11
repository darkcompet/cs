#pragma warning disable IDE0130 // Namespace がフォルダー構造と一致しません
namespace Tool.Compet.Core;

public class DkHashMap<TKey, TValue> : Dictionary<TKey, TValue> where TKey : notnull {
	/// <summary>
	/// Null/Default value means the key may not exist.
	/// </summary>
	/// <param name="key"></param>
	/// <returns></returns>
	public new TValue? this[TKey key] {
		get => this.GetValueOrDefault(key, default);
		set => base[key] = value!;
	}

	public TValue GetOrSet(TKey key, TValue initValue) {
		if (this.TryGetValue(key, out var value)) {
			return value;
		}
		base[key] = initValue;
		return initValue;
	}
}
