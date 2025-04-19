#pragma warning disable IDE0130 // Namespace がフォルダー構造と一致しません
namespace Tool.Compet.Core;

public class DkDisjoinSet {
	/// <summary>
	/// Element count.
	/// </summary>
	private readonly int elementCount;

	/// <summary>
	/// Opt: compress path.
	/// </summary>
	private readonly int[] set;

	/// <summary>
	/// Opt: when merge 2 sets.
	/// </summary>
	private readonly int[] rank;

	public DkDisjoinSet(int elementCount) {
		this.elementCount = elementCount;
		var set = this.set = new int[elementCount];
		this.rank = new int[elementCount];

		// Add default set (root) for each element
		for (var v = 0; v < elementCount; ++v) {
			set[v] = v;
		}
	}

	// private void AddSet(int v) {
	// 	this.set[v] = v;
	// }

	/// <summary>
	/// Merge 2 sets that contains given u, v.
	/// This is known as union action.
	/// </summary>
	/// <param name="u">Element 1 (must smaller than N)</param>
	/// <param name="v">Element 2 (must smaller than N)</param>
	public void MergeSets(int u, int v) {
		var pu = this.FindSet(u);
		var pv = this.FindSet(v);
		if (pu != pv) {
			// Opt: Only attach lower rank node to higher rank node to make tree height small as possible.
			var rank = this.rank;
			if (rank[pu] > rank[pv]) {
				(pu, pv) = (pv, pu);
			}
			// Attach set u to set v
			this.set[pu] = pv;
			if (rank[pv] == rank[pu]) {
				++rank[pv];
			}
		}
	}

	/// <summary>
	/// Find index of set (parent element) that contains the value.
	/// </summary>
	/// <param name="v">Find the set that element belongs to (must smaller than N)</param>
	/// <returns>Index of set that contains the element</returns>
	public int FindSet(int v) {
		var parent = this.set;
		if (parent[v] == v) {
			return v;
		}
		// Opt: Compress path by remember highest parent of the element.
		return parent[v] = this.FindSet(parent[v]);
	}
}
