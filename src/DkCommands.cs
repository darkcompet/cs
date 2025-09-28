#pragma warning disable IDE0130 // Namespace がフォルダー構造と一致しません
namespace Tool.Compet.Core;

using System.Diagnostics;
using System.Runtime.InteropServices;

public class DkCommands {
	/// <summary>
	/// Run command in single process.
	/// </summary>
	/// <param name="command">Can make to multiple by concat commands. For eg,. cd ~/test/com && ls -la</param>
	/// <param name="workingDirPath">Specify which directory that command should be executed in. Default is null which inherits working directory of parent process.</param>
	/// <param name="filePath">By default, we use cmd.exe or /bin/bash as file to run the command.</param>
	/// <param name="cancellationToken"></param>
	/// <returns>Tupple of (Result, Error)</returns>
	public static async Task<(string, string)> RunCommandAsync(string command, string? workingDirPath = null, string? filePath = null, CancellationToken cancellationToken = default) {
		// Detect OS and set the correct shell
		if (string.IsNullOrEmpty(filePath)) {
			filePath = RuntimeInformation.IsOSPlatform(OSPlatform.Windows) ? "cmd.exe" : "/bin/bash";
		}

		var process = new Process() {
			StartInfo = new ProcessStartInfo {
				FileName = filePath,
				UseShellExecute = false,
				WorkingDirectory = workingDirPath,
				CreateNoWindow = true,
				RedirectStandardInput = true,
				// Read stream asynchronously using an event handler
				RedirectStandardOutput = true,
				RedirectStandardError = true
			}
		};

		// After start, the process will execute our input (commands)
		process.Start();

		// Write the command directly to StandardInput
		await process.StandardInput.WriteLineAsync(command);
		await process.StandardInput.FlushAsync(cancellationToken);
		process.StandardInput.Close();

		// Read output
		var output = await process.StandardOutput.ReadToEndAsync(cancellationToken);
		var error = await process.StandardError.ReadToEndAsync(cancellationToken);

		// Wait until the process end (output was handled)
		await process.WaitForExitAsync(cancellationToken);

		// Release process resource
		process.Close();

		Console.WriteLine($"---> Executed command: {command}. Output: {output}, Error: {error}");

		return (output, error);
	}

	/// <summary>
	/// Run multiple commands continuously in single process.
	/// Note: The result of previous command does not affect to execution of next command.
	/// Ref: https://learn.microsoft.com/en-us/dotnet/api/system.diagnostics.process.beginoutputreadline?view=net-8.0
	/// </summary>
	/// <param name="commands"></param>
	/// <param name="workingDirPath">Specify which directory that command should be executed in. Default is null which inherits working directory of parent process.</param>
	/// <param name="filePath"></param>
	/// <param name="cancellationToken"></param>
	/// <returns>(outputs, errors)</returns>
	public static async Task<(List<string>, List<string>)> RunBatchCommandsAsync(IEnumerable<string> commands, string? workingDirPath = null, string? filePath = null, CancellationToken cancellationToken = default) {
		// Detect OS and set the correct shell
		if (string.IsNullOrEmpty(filePath)) {
			filePath = RuntimeInformation.IsOSPlatform(OSPlatform.Windows) ? "cmd.exe" : "/bin/bash";
		}

		var process = new Process {
			StartInfo = new ProcessStartInfo {
				FileName = filePath,
				UseShellExecute = false,
				WorkingDirectory = workingDirPath,
				RedirectStandardInput = true,
				// Read stream asynchronously using an event handler
				RedirectStandardOutput = true,
				RedirectStandardError = true,
			}
		};

		// Listen to output event
		var outputs = new List<string>();
		var errors = new List<string>();
		process.OutputDataReceived += (sendingProcess, outLine) => {
			outputs.Add(outLine.Data ?? string.Empty);
			Console.WriteLine($"---> Run bulk output: {outLine.Data}");
		};
		process.ErrorDataReceived += (sendingProcess, outLine) => {
			errors.Add(outLine.Data ?? string.Empty);
			Console.WriteLine($"---> Error run bulk: {outLine.Data}");
		};

		// After start, the process will execute our input (commands)
		process.Start();

		// Start the asynchronous read of the output stream.
		process.BeginOutputReadLine();
		process.BeginErrorReadLine();

		// Write commands into input stream and close to execute.
		foreach (var cmd in commands) {
			Console.WriteLine($"---> Run bulk commands: {cmd}");
			await process.StandardInput.WriteLineAsync(cmd);
		}
		process.StandardInput.Close();

		// Wait until the process end (output was handled)
		await process.WaitForExitAsync(cancellationToken);

		// Release process resource
		process.Close();

		return (outputs, errors);
	}
}
