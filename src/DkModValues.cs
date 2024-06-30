#pragma warning disable IDE0130 // Namespace がフォルダー構造と一致しません
namespace Tool.Compet.Core;

/// Ref: https://learn.microsoft.com/en-us/dotnet/csharp/language-reference/operators/operator-overloading
public struct DkModInt {
	private static int modulo;
	public int value;

	public DkModInt(int mod = 1000_000_007) {
		modulo = mod;
	}

	public DkModInt(int value, int mod = 1000_000_007) {
		this.value = value;
		modulo = mod;
	}

	public static DkModInt operator +(DkModInt a, DkModInt b) {
		return new DkModInt((a.value + b.value) % modulo);
	}

	public static DkModInt operator -(DkModInt a, DkModInt b) {
		return new DkModInt((a.value - b.value) % modulo);
	}

	public static DkModInt operator *(DkModInt a, DkModInt b) {
		return new DkModInt(a.value * b.value % modulo);
	}

	public static DkModInt operator /(DkModInt a, DkModInt b) {
		return new DkModInt(a.value / b.value % modulo);
	}
}

public struct DkModLong {
	private static long modulo;
	public long value;

	public DkModLong(long mod = 1000_000_007) {
		modulo = mod;
	}

	public DkModLong(long value, long mod = 1000_000_007) {
		this.value = value;
		modulo = mod;
	}

	public static DkModLong operator +(DkModLong a, DkModLong b) {
		return new DkModLong((a.value + b.value) % modulo);
	}

	public static DkModLong operator -(DkModLong a, DkModLong b) {
		return new DkModLong((a.value - b.value) % modulo);
	}

	public static DkModLong operator *(DkModLong a, DkModLong b) {
		return new DkModLong(a.value * b.value % modulo);
	}

	public static DkModLong operator /(DkModLong a, DkModLong b) {
		return new DkModLong(a.value / b.value % modulo);
	}
}
