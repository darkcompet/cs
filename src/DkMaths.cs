#pragma warning disable IDE0161 // 範囲指定されたファイルが設定された namespace に変換
namespace Tool.Compet.Core {
	public class DkMaths {
		/// <summary>
		/// Fast pow.
		/// Time: O(logN)
		/// Method 1: x^9 = x^0 * x^1 * x^0 * x^0 * x^8. Note: 9 = 1001.
		/// Method 2: x^9 = x^4 * x^4 * x
		/// </summary>
		/// <param name="x">Base number</param>
		/// <param name="n">Exponential number. It should not be negative</param>
		/// <returns></returns>
		public static long Pow(long x, uint n) {
			var result = 1L;
			while (n > 0) {
				// Mul if meet bit 1
				if ((n & 1) == 1) {
					result *= x;
				}
				// Down n and Up x
				n >>= 1;
				x *= x;
			}

			return result;
		}

		/// <summary>
		/// Fast pow with modulo.
		/// Time: O(logN)
		/// Method 1: x^9 = x^0 * x^1 * x^0 * x^0 * x^8. Note: 9 = 1001.
		/// Method 2: x^9 = x^4 * x^4 * x
		/// </summary>
		/// <param name="x">Base number</param>
		/// <param name="n">Exponential number. It should not be negative</param>
		/// <returns></returns>
		public static long Pow(long x, uint n, int mod) {
			var result = 1L;
			while (n > 0) {
				// Mul if meet bit 1
				if ((n & 1) == 1) {
					result = result * x % mod;
				}
				// Down n and Up x
				n >>= 1;
				x = x * x % mod;
			}

			return result;
		}

		/// <summary>
		/// For positive integer N, this finds largest number k such that: 2^k <= N.
		/// Time: O(logN).
		/// </summary>
		/// <param name="n">Must be >= 1</param>
		/// <returns></returns>
		public static int FloorLog2(long n) {
			var k = 0;
			while ((1L << k) <= n) {
				++k;
			}
			return k - 1;
		}

		/// <summary>
		/// Find largest k such that: X^k <= n.
		/// Time: O(logN)
		/// </summary>
		/// <param name="n"></param>
		/// <param name="x"></param>
		/// <returns></returns>
		public static int FloorLogX(long n, long x) {
			var k = 0;
			var mul = 1L;
			while (mul <= n) {
				++k;
				mul *= x;
			}
			return k - 1;
		}

		/// <summary>
		/// For any integer N, this finds lowest number k such that: 2^k >= N.
		/// Time: O(logN).
		/// </summary>
		/// <param name="n">Should be >= 0</param>
		/// <returns></returns>
		public static int CeilLog2(long n) {
			var k = 0;
			while ((1L << k) < n) {
				++k;
			}
			return k;
		}

		/// <summary>
		/// Find lowest k such that: X^k >= n
		/// Time: O(logN)
		/// </summary>
		/// <param name="n"></param>
		/// <param name="x"></param>
		/// <returns></returns>
		public static int CeilLogX(long n, long x) {
			var k = 0;
			var mul = 1L;
			while (mul < n) {
				++k;
				mul *= x;
			}
			return k;
		}

		/// <summary>
		/// For positive integer N, this checks given N is power of 2 or not.
		/// Time: O(1).
		/// </summary>
		/// <param name="n">Any</param>
		/// <returns></returns>
		public static bool IsPowOf2(long n) {
			return n > 0 && (n & (n - 1)) == 0;
		}

		/// <summary>
		/// Calculate value of polynomial function. For eg,. f(x) = c + x * (b + x * (a + x * 0))
		/// </summary>
		/// <param name="arr">Store value of a_0, a_1, ..., a_n</param>
		/// <param name="x">Base number</param>
		/// <returns></returns>
		public static long Poly(long[] arr, long x) {
			var result = 0L;
			var N = arr.Length;
			for (var index = 0; index < N; ++index) {
				result = arr[index] + (x * result);
			}
			return result;
		}

		public static bool IsDigit(char ch) {
			return ch is >= '0' and <= '9';
		}

		public static int Max(params int[] arr) {
			var ans = arr[0];
			for (var i = arr.Length - 1; i >= 0; --i) {
				if (ans < arr[i]) {
					ans = arr[i];
				}
			}
			return ans;
		}

		public static int Min(params int[] arr) {
			var ans = arr[0];
			for (var i = arr.Length - 1; i >= 0; --i) {
				if (ans > arr[i]) {
					ans = arr[i];
				}
			}
			return ans;
		}
	}
}
