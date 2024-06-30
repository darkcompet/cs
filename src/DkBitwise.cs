#pragma warning disable IDE0130 // Namespace がフォルダー構造と一致しません
namespace Tool.Compet.Core;

public class DkBitwise {
	/// <summary>
	/// Check bit (is 1) at index of given number.
	/// </summary>
	/// <param name="num">The number</param>
	/// <param name="index">From right -> left. At rightmost, it is 0.</param>
	/// <returns></returns>
	public static bool HasBitAt(int num, int index) {
		return ((num >> index) & 1) == 1;
	}

	/// <summary>
	/// Calculate number of bit 1 (pop count) in given number.
	/// Ref:
	/// https://github.com/dotnet/corert/blob/master/src/System.Private.CoreLib/shared/System/Numerics/BitOperations.cs
	/// https://gist.github.com/Plutor/2002457
	/// </summary>
	/// <param name="num"></param>
	/// <returns></returns>
	public static int BitCount(uint num) {
		return System.Numerics.BitOperations.PopCount(num);
	}

	/// <summary>
	/// Set bit (set to 1) at given index.
	/// </summary>
	/// <param name="num">The number</param>
	/// <param name="index">Rightmost index</param>
	public static void SetBitAt(ref int num, int index) {
		num |= 1 << index;
	}

	/// <summary>
	/// Unset bit (set to 0) at given index.
	/// </summary>
	/// <param name="num">The number</param>
	/// <param name="index">Rightmost index</param>
	public static void ClearBitAt(ref int num, int index) {
		num &= ~(1 << index);
	}

	/// <summary>
	/// Toggle bit (change 1 -> 0, 0 -> 1) at given index.
	/// </summary>
	/// <param name="num">The number</param>
	/// <param name="index">Rightmost index</param>
	public static void ToggleBitAt(ref int num, int index) {
		num ^= 1 << index;
	}
}
