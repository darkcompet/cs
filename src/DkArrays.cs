#pragma warning disable IDE0161 // 範囲指定されたファイルが設定された namespace に変換
namespace Tool.Compet.Core {
	public class DkArrays {
		public static void Fill(int[] arr, int value, int startIndex, int endIndex) {
			Array.Fill(arr, value, startIndex, endIndex - startIndex + 1);
		}
	}
}
