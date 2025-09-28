#pragma warning disable IDE0130 // Namespace がフォルダー構造と一致しません
namespace Tool.Compet.Core;

using System.IO;
using System.IO.Compression;
using System.Text.RegularExpressions;

public static class DkCompression {
	public static async Task<string> ZipAsFileAsync(string srcDirPath, string dstFilePath, string[]? ignorePatterns = null) {
		if (!Directory.Exists(srcDirPath)) {
			throw new DirectoryNotFoundException($"Source directory not found: {srcDirPath}");
		}

		var outputDir = Path.GetDirectoryName(dstFilePath);
		if (!string.IsNullOrEmpty(outputDir)) {
			Directory.CreateDirectory(outputDir);
		}

		await using var fileStream = new FileStream(dstFilePath, FileMode.Create, FileAccess.Write, FileShare.None, bufferSize: 81920, useAsync: true);
		using var archive = new ZipArchive(fileStream, ZipArchiveMode.Create, leaveOpen: false);

		var ignoreRegexes = CompileIgnorePatterns(ignorePatterns ?? Array.Empty<string>());
		AddDirectoryToZip(srcDirPath, archive, srcDirPath, ignoreRegexes);

		return dstFilePath;
	}

	public static Task<MemoryStream> ZipAsStreamAsync(string srcDirPath, string[]? ignorePatterns = null) {
		if (!Directory.Exists(srcDirPath)) {
			throw new DirectoryNotFoundException($"Source directory not found: {srcDirPath}");
		}

		var memoryStream = new MemoryStream();
		using (var archive = new ZipArchive(memoryStream, ZipArchiveMode.Create, leaveOpen: true)) {
			var ignoreRegexes = CompileIgnorePatterns(ignorePatterns ?? Array.Empty<string>());
			AddDirectoryToZip(srcDirPath, archive, srcDirPath, ignoreRegexes);
		}

		memoryStream.Position = 0;

		return Task.FromResult(memoryStream);
	}

	private static void AddDirectoryToZip(string currentDir, ZipArchive archive, string rootDir, List<Regex> ignoreRegexes) {
		foreach (var filePath in Directory.EnumerateFiles(currentDir)) {
			var relativePath = Path.GetRelativePath(rootDir, filePath).Replace('\\', '/');
			var entry = archive.CreateEntry(relativePath, CompressionLevel.Optimal);

			using var entryStream = entry.Open();
			using var fileStream = new FileStream(filePath, FileMode.Open, FileAccess.Read, FileShare.Read, bufferSize: 81920, useAsync: false);
			fileStream.CopyTo(entryStream);
		}

		foreach (var subDir in Directory.EnumerateDirectories(currentDir)) {
			var relativeSubDir = Path.GetRelativePath(rootDir, subDir).Replace('\\', '/');
			if (ignoreRegexes.Any(rx => rx.IsMatch(relativeSubDir))) {
				continue;
			}

			AddDirectoryToZip(subDir, archive, rootDir, ignoreRegexes);
		}
	}

	private static List<Regex> CompileIgnorePatterns(string[] patterns) {
		var regexes = new List<Regex>();
		foreach (var pattern in patterns) {
			var normalized = pattern.Replace('\\', '/').Trim('/');
			var regexPattern = "^" + Regex.Escape(normalized).Replace(@"\*\*", ".*").Replace(@"\*", "[^/]*") + "$";

			regexes.Add(new Regex(regexPattern, RegexOptions.IgnoreCase | RegexOptions.Compiled));
		}
		return regexes;
	}
}
