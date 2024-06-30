#pragma warning disable IDE0161 // 範囲指定されたファイルが設定された namespace に変換
namespace Tool.Compet.Core {
	public class HashMap<TKey, TValue> : Dictionary<TKey, TValue?> where TKey : notnull {
		public new TValue? this[TKey key] {
			get => this.GetValueOrDefault(key, default);
			set => base[key] = value;
		}
	}
}
