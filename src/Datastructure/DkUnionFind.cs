#pragma warning disable IDE0130 // Namespace がフォルダー構造と一致しません
namespace Tool.Compet.Core;

public class DkUnionFind {
	/// <summary>
	/// Element count.
	/// </summary>
	private readonly int elementCount;

	/// <summary>
	/// Opt: compress path.
	/// </summary>
	private readonly int[] parent;

	/// <summary>
	/// Opt: when merge 2 sets.
	/// </summary>
	private readonly int[] rank;

	public DkUnionFind(int elementCount) {
		this.elementCount = elementCount;
		this.parent = new int[elementCount];
		this.rank = new int[elementCount];

		// Add default set (root) for each element
		var parent = this.parent;
		for (var v = 0; v < elementCount; ++v) {
			parent[v] = v;
		}
	}

	// public bool Add(int v) {
	// 	if (this.parent[v] != v) {
	// 		this.parent[v] = v;
	// 	}
	// 	return true;
	// }

	/// <summary>
	/// Merge 2 sets that contains given u, v.
	/// This is known as union action.
	/// </summary>
	/// <param name="u">Element 1 (must smaller than N)</param>
	/// <param name="v">Element 2 (must smaller than N)</param>
	/// <returns>False: Skip merge since cycle detected</returns>
	public bool Union(int u, int v) {
		var pu = this.Find(u);
		var pv = this.Find(v);
		if (pu == pv) {
			// Cycle detected so just skip
			return false;
		}
		// Opt: Only attach lower rank node to higher rank node to make tree height small as possible.
		var rank = this.rank;
		if (rank[pu] > rank[pv]) {
			(pu, pv) = (pv, pu);
		}
		// Attach set u to set v
		this.parent[pu] = pv;
		if (rank[pv] == rank[pu]) {
			++rank[pv];
		}
		return true;
	}

	/// <summary>
	/// Find index of set (parent element) which contains given value.
	/// </summary>
	/// <param name="v">Find the set that element belongs to (must smaller than N)</param>
	/// <returns>Index of set that contains the element</returns>
	public int Find(int v) {
		var parent = this.parent;
		if (parent[v] == v) {
			return v;
		}
		// Opt: Compress path by remember highest parent of the element.
		return parent[v] = this.Find(parent[v]);
	}

	public int CountRoots() {
		var count = 0;
		foreach (var v in this.parent) {
			if (v == this.Find(v)) {
				++count;
			}
		}
		return count;
	}
}
