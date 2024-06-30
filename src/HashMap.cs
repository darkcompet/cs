#pragma warning disable IDE0130 // Namespace がフォルダー構造と一致しません
namespace Tool.Compet.Core;

public class HashMap<TKey, TValue> : Dictionary<TKey, TValue?> where TKey : notnull {
	public new TValue? this[TKey key] {
		get => this.GetValueOrDefault(key, default);
		set => base[key] = value;
	}
}
