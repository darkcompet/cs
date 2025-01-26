#pragma warning disable IDE0130 // Namespace がフォルダー構造と一致しません
namespace Tool.Compet.Core;

using System.Collections.ObjectModel;

public class DkCollection<T> {
	public static readonly ReadOnlyCollection<T> readonlyList = ReadOnlyCollection<T>.Empty;
}
