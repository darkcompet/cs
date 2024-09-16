#pragma warning disable IDE0130 // Namespace がフォルダー構造と一致しません
namespace Tool.Compet.Core;

public class DkArrays {
	/// <summary>
	/// Fill range in given array to the value.
	/// </summary>
	/// <param name="arr"></param>
	/// <param name="value"></param>
	/// <param name="startIndex">Inclusive</param>
	/// <param name="endIndex">Exclusive</param>
	public static void Fill(int[] arr, int value, int startIndex, int endIndex) {
		Array.Fill(arr, value, startIndex, endIndex - startIndex);
	}
}
