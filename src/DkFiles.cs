#pragma warning disable IDE0130 // Namespace がフォルダー構造と一致しません
namespace Tool.Compet.Core;

using System.IO;
using System.Threading.Tasks;

public static class DkFiles {
	/// <summary>
	/// It is useful for copy very large file with non-blocking IO approach (so don't consume/block thread pool thread).
	/// </summary>
	/// <param name="srcFilePath"></param>
	/// <param name="dstFilePath"></param>
	/// <param name="bufferSize"></param>
	/// <returns></returns>
	public static async Task CopyFileAsync(string srcFilePath, string dstFilePath, int bufferSize = 81920) {
		// Ensure the destination directory exists
		Directory.CreateDirectory(Path.GetDirectoryName(dstFilePath)!);

		await using var srcStream = new FileStream(
			srcFilePath, FileMode.Open, FileAccess.Read, FileShare.Read,
			bufferSize: bufferSize, useAsync: true);

		await using var dstStream = new FileStream(
			dstFilePath, FileMode.Create, FileAccess.Write, FileShare.None,
			bufferSize: bufferSize, useAsync: true);

		await srcStream.CopyToAsync(dstStream, bufferSize);
	}

	/// <summary>
	/// Copy files (also files in child folders) from src to dst.
	/// </summary>
	/// <param name="srcDirPath"></param>
	/// <param name="dstDirPath"></param>
	/// <param name="overwrite"></param>
	public static void CopyDirectory(string srcDirPath, string dstDirPath, bool overwrite = true) {
		// Ensure destination exists
		Directory.CreateDirectory(dstDirPath);

		// Copy all files
		foreach (var filePath in Directory.GetFiles(srcDirPath)) {
			var fileName = Path.GetFileName(filePath);
			var destFile = Path.Combine(dstDirPath, fileName);

			File.Copy(filePath, destFile, overwrite);
		}

		// Recursively copy sub-directories
		foreach (var dirPath in Directory.GetDirectories(srcDirPath)) {
			var dirName = Path.GetFileName(dirPath);
			var destSubDir = Path.Combine(dstDirPath, dirName);

			CopyDirectory(dirPath, destSubDir, overwrite);
		}
	}
}
