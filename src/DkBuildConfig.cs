#pragma warning disable IDE0161 // 範囲指定されたファイルが設定された namespace に変換
namespace Tool.Compet.Core {
	/// Common build-config which be shared between all projects/libraries.
	public class DkBuildConfig {
		/// The app/library should provide variable `DEBUG` to enable this constant.
#if DEBUG
		public const bool DEBUG = true;
#else
		public const bool DEBUG = false;
#endif
	}
}
