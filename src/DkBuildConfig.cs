#pragma warning disable IDE0130 // Namespace がフォルダー構造と一致しません
namespace Tool.Compet.Core;

/// Common build-config which be shared between all projects/libraries.
public class DkBuildConfig {
	/// The app/library should provide variable `DEBUG` to enable this constant.
#if DEBUG
	public const bool DEBUG = true;
#else
		public const bool DEBUG = false;
#endif
}
