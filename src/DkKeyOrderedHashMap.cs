#pragma warning disable IDE0130 // Namespace がフォルダー構造と一致しません
namespace Tool.Compet.Core;

/// Default Dictionary will change (sort) keys, so it does not preserve order of keys.
/// This class keep tracking order of keys when add, remove, ... the entry.
public class DkKeyOrderedHashMap<T>(int capacity = 10) {
	private readonly List<string> _keys = new(capacity);
	private readonly Dictionary<string, T> _map = new(capacity);

	/// Define the indexer to allow client code to use [] notation.
	public T this[string key] {
		get => this._map[key];
		set => this._map[key] = value;
	}

	// public T this[int index] {
	// 	get { var key = this.list[index]; return this.map[key]; }
	// 	private set { }
	// }

	public T? GetValueOrDefault(string key) {
		return this._map.GetValueOrDefault(key);
	}

	public void Add(string key, T value) {
		this._map.Add(key, value);
		this._keys.Add(key);
	}

	public bool TryAdd(string key, T value) {
		if (this._map.TryAdd(key, value)) {
			this._keys.Add(key);
			return true;
		}
		return false;
	}

	public int Count => this._keys.Count;
	public long LongCount => this._keys.Count;

	public string[] Keys {
		get {
			var arr = new string[this._keys.Count];
			for (var index = this._keys.Count - 1; index >= 0; --index) {
				arr[index] = this._keys[index];
			}
			return arr;
		}
		private set { }
	}

	public List<string> InternalKeys => this._keys;

	public void Clear() {
		this._map.Clear();
		this._keys.Clear();
	}
}
