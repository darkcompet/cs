#pragma warning disable IDE0130 // Namespace がフォルダー構造と一致しません
namespace Tool.Compet.Core;

using System.Diagnostics;

public class DkCommands {
	/// <summary>
	/// Run command in single process.
	/// </summary>
	/// <param name="command">Can make to multiple by concat commands. For eg,. cd ~/test/com && ls -la</param>
	/// <param name="filePath">By default, we use /bin/bash as file to run the command.</param>
	/// <param name="cancellationToken"></param>
	/// <returns>Tupple of (Result, Error)</returns>
	public static async Task<(string, string)> RunCommandAsync(string command, string filePath = "bash", CancellationToken cancellationToken = default) {
		var escapedCommand = command.Replace("\"", "\\\"");

		var process = new Process() {
			StartInfo = new ProcessStartInfo {
				FileName = filePath,
				Arguments = $"-c \"{escapedCommand}\"",
				CreateNoWindow = true,
				UseShellExecute = false,
				RedirectStandardInput = true,
				// Read stream asynchronously using an event handler
				RedirectStandardOutput = true,
				RedirectStandardError = true
			}
		};

		// After start, the process will execute our input (commands)
		process.Start();

		// Read output
		var output = await process.StandardOutput.ReadToEndAsync(cancellationToken);
		var error = await process.StandardError.ReadToEndAsync(cancellationToken);

		// Wait until the process end (output was handled)
		await process.WaitForExitAsync(cancellationToken);

		// Release process resource
		process.Close();

		Console.WriteLine($"---> Run command: {command}. Output: {output}, Error: {error}");

		return (output, error);
	}

	/// <summary>
	/// Run multiple commands continuously in single process.
	/// Note: The result of previous command does not affect to execution of next command.
	/// Ref: https://learn.microsoft.com/en-us/dotnet/api/system.diagnostics.process.beginoutputreadline?view=net-8.0
	/// </summary>
	/// <param name="commands"></param>
	/// <param name="filePath"></param>
	/// <param name="cancellationToken"></param>
	/// <returns>(outputs, errors)</returns>
	public static async Task<(List<string?>, List<string?>)> RunCommandsAsync(IEnumerable<string> commands, string filePath = "bash", CancellationToken cancellationToken = default) {
		var process = new Process {
			StartInfo = new ProcessStartInfo {
				FileName = filePath,
				UseShellExecute = false,
				RedirectStandardInput = true,
				// Read stream asynchronously using an event handler
				RedirectStandardOutput = true,
				RedirectStandardError = true,
			}
		};

		// Listen to output event
		var outputs = new List<string?>();
		var errors = new List<string?>();
		process.OutputDataReceived += (object sendingProcess, DataReceivedEventArgs outLine) => {
			outputs.Add(outLine.Data);
			Console.WriteLine($"---> Run bulk output: {outLine.Data}");
		};
		process.ErrorDataReceived += (object sendingProcess, DataReceivedEventArgs outLine) => {
			errors.Add(outLine.Data);
			Console.WriteLine($"---> Run bulk error: {outLine.Data}");
		};

		// After start, the process will execute our input (commands)
		process.Start();

		// Start the asynchronous read of the output stream.
		process.BeginOutputReadLine();
		process.BeginErrorReadLine();

		// Write commands into input stream and close to execute.
		foreach (var cmd in commands) {
			Console.WriteLine($"---> Run bulk command: {cmd}");
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
