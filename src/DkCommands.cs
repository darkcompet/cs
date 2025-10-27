#pragma warning disable IDE0130 // Namespace がフォルダー構造と一致しません
namespace Tool.Compet.Core;

using System.Diagnostics;

public class DkCommands {
	/// <summary>
	/// Runs a process asynchronously and captures its full stdout/stderr as strings.
	/// This is best for short-lived commands with limited output.
	/// </summary>
	public static async Task<CommandResult> RunAsync(
		string fileName,
		IEnumerable<string> arguments,
		string? workingDirPath = null,
		string? stdin = null,
		CancellationToken cancellationToken = default
	) {
		using var process = new Process();
		process.StartInfo = new ProcessStartInfo {
			FileName = fileName,
			UseShellExecute = false, // required for redirection
			RedirectStandardInput = stdin != null,
			RedirectStandardOutput = true,
			RedirectStandardError = true,
			CreateNoWindow = true,
			WorkingDirectory = workingDirPath ?? Environment.CurrentDirectory
		};

		// Add arguments safely (prevents injection)
		foreach (var arg in arguments) {
			process.StartInfo.ArgumentList.Add(arg);
		}

		if (!process.Start()) {
			throw new InvalidOperationException($"Failed to start process: {fileName}");
		}

		// If stdin is provided, write it and close
		if (stdin != null) {
			await process.StandardInput.WriteAsync(stdin.AsMemory(), cancellationToken).ConfigureAwait(false);
			process.StandardInput.Close();
		}

		// Read stdout and stderr fully in parallel
		var outputTask = process.StandardOutput.ReadToEndAsync(cancellationToken);
		var errorTask = process.StandardError.ReadToEndAsync(cancellationToken);

		try {
			await process.WaitForExitAsync(cancellationToken).ConfigureAwait(false);
		}
		catch (OperationCanceledException) {
			// Kill the process if cancelled
			try {
				if (!process.HasExited) {
					process.Kill(entireProcessTree: true);
				}
			}
			catch {
				// ignored
			}
			throw;
		}

		// Ensure both streams are drained
		await Task.WhenAll(outputTask, errorTask).ConfigureAwait(false);

		return new CommandResult(
			process.ExitCode,
			outputTask.Result,
			errorTask.Result
		);
	}

	/// <summary>
	/// Runs a process asynchronously, streaming stdout/stderr line-by-line
	/// while also capturing the full output for the final result.
	/// This is best for long-running commands with realtime output response.
	/// </summary>
	/// <param name="fileName">Executable to run (e.g. "dotnet", "git").</param>
	/// <param name="arguments">Arguments passed safely via ArgumentList.</param>
	/// <param name="workingDirPath">Optional working directory (defaults to current).</param>
	/// <param name="stdin">Optional text to write into stdin.</param>
	/// <param name="onOutput">Optional callback invoked for each stdout line.</param>
	/// <param name="onError">Optional callback invoked for each stderr line.</param>
	/// <param name="cancellationToken">Cancellation token to stop process.</param>
	/// <returns>ExitCode: 0 (succeed), others (failed)</returns>
	public static async Task<int> RunStreamingAsync(
		string fileName,
		IEnumerable<string> arguments,
		string? workingDirPath = null,
		string? stdin = null,
		Action<string>? onOutput = null,
		Action<string>? onError = null,
		CancellationToken cancellationToken = default
	) {
		// Configure the process with safe defaults:
		// - UseShellExecute = false → required for redirection
		// - RedirectStandardInput/Output/Error → so we can interact with streams
		// - CreateNoWindow = true → prevents creating a console window
		using var process = new Process();
		process.StartInfo = new ProcessStartInfo {
			FileName = fileName,
			UseShellExecute = false,
			RedirectStandardInput = stdin != null,
			RedirectStandardOutput = true,
			RedirectStandardError = true,
			CreateNoWindow = true,
			WorkingDirectory = workingDirPath ?? Environment.CurrentDirectory
		};
		process.EnableRaisingEvents = true; // required for async output events

		// Add arguments safely via ArgumentList (prevents injection issues)
		foreach (var arg in arguments) {
			process.StartInfo.ArgumentList.Add(arg);
		}

		// TaskCompletionSources signal when stdout/stderr streams are fully drained.
		var outputTcs = new TaskCompletionSource<bool>(TaskCreationOptions.RunContinuationsAsynchronously);
		var errorTcs = new TaskCompletionSource<bool>(TaskCreationOptions.RunContinuationsAsynchronously);

		// Handlers capture each line of stdout/stderr as it arrives.
		// They append to the StringBuilder and optionally forward to the caller.
		DataReceivedEventHandler outputHandler = (_, e) => {
			if (e.Data is null) {
				outputTcs.TrySetResult(true); // end of stdout
			}
			else {
				onOutput?.Invoke(e.Data);
			}
		};

		DataReceivedEventHandler errorHandler = (_, e) => {
			if (e.Data is null) {
				errorTcs.TrySetResult(true); // end of stderr
			}
			else {
				onError?.Invoke(e.Data);
			}
		};

		// Attach handlers before starting the process
		process.OutputDataReceived += outputHandler;
		process.ErrorDataReceived += errorHandler;

		// Start the process
		if (!process.Start()) {
			throw new InvalidOperationException($"Failed to start process: {fileName}");
		}

		// Begin asynchronous reading of stdout/stderr
		process.BeginOutputReadLine();
		process.BeginErrorReadLine();

		// If stdin content is provided, write it and close the stream
		if (stdin != null) {
			await process.StandardInput.WriteAsync(stdin.AsMemory(), cancellationToken).ConfigureAwait(false);
			process.StandardInput.Close(); // Close flushes automatically
		}

		// Wait for process exit, respecting cancellation
		try {
			await process.WaitForExitAsync(cancellationToken).ConfigureAwait(false);
		}
		catch (OperationCanceledException) {
			// If cancelled, kill the process (and its children) to avoid zombies
			try {
				if (!process.HasExited) {
					process.Kill(entireProcessTree: true);
				}
			}
			catch {
				// Ignore errors during kill
			}

			// Rethrow so the caller sees the cancellation
			throw;
		}

		// Ensure both stdout and stderr streams are fully drained
		await Task.WhenAll(outputTcs.Task, errorTcs.Task).ConfigureAwait(false);

		// Detach handlers to avoid memory leaks (good hygiene)
		process.OutputDataReceived -= outputHandler;
		process.ErrorDataReceived -= errorHandler;

		return process.ExitCode;
	}

	/// <summary>
	/// Executes a sequence of commands as a single script, optionally piping a stream to the process's standard input.
	/// This is the primary, most flexible method for running complex scripts.
	/// </summary>
	/// <param name="commands">The ordered list of command strings to execute as a script.</param>
	/// <param name="stdin">An optional stream to pipe to the process's standard input. Ideal for secrets or large files.</param>
	/// <param name="cancellationToken">A token to cancel the operation.</param>
	/// <returns>A single CommandResult containing the combined output and the final exit code.</returns>
	public static async Task<CommandResult> RunBulkAsync(
		IEnumerable<string> commands,
		string? workingDirPath = null,
		string? stdin = null,
		CancellationToken cancellationToken = default
	) {
		using var process = new Process();
		process.StartInfo = new ProcessStartInfo {
			FileName = OperatingSystem.IsWindows() ? "cmd.exe" : "/bin/bash",
			UseShellExecute = false,
			RedirectStandardInput = true,
			RedirectStandardOutput = true,
			RedirectStandardError = true,
			CreateNoWindow = true,
			WorkingDirectory = workingDirPath ?? Environment.CurrentDirectory
		};

		if (!process.Start()) {
			throw new InvalidOperationException($"---> Failed to start shell process: {process.StartInfo.FileName}");
		}

		// Start reading output/error immediately
		var outputTask = process.StandardOutput.ReadToEndAsync(cancellationToken);
		var errorTask = process.StandardError.ReadToEndAsync(cancellationToken);

		try {
			// Write commands into the shell
			if (process.StartInfo.RedirectStandardInput) {
				if (process.StartInfo.FileName.EndsWith("cmd.exe", StringComparison.OrdinalIgnoreCase)) {
					await process.StandardInput.WriteLineAsync("@echo off").ConfigureAwait(false);
				}

				foreach (var command in commands) {
					await process.StandardInput.WriteLineAsync(command).ConfigureAwait(false);
				}

				// If stdin content is provided, write it
				if (stdin != null) {
					await process.StandardInput.WriteAsync(stdin.AsMemory(), cancellationToken).ConfigureAwait(false);
				}

				// Important: tell the shell to exit after running commands
				await process.StandardInput.WriteLineAsync("exit").ConfigureAwait(false);
				process.StandardInput.Close();
			}

			// Wait for process exit or cancellation
			await process.WaitForExitAsync(cancellationToken).ConfigureAwait(false);
		}
		catch (OperationCanceledException) {
			// Kill process if cancelled
			try {
				if (!process.HasExited) {
					process.Kill(entireProcessTree: true);
				}
			}
			catch {
				// ignored
			}
			throw;
		}

		// Ensure output/error are fully read
		await Task.WhenAll(outputTask, errorTask).ConfigureAwait(false);

		return new CommandResult(
			process.ExitCode,
			outputTask.Result,
			errorTask.Result
		);
	}

	/// <summary>
	/// Runs multiple commands inside a shell process, streaming stdout/stderr line by line
	/// while also capturing the full output for the final result.
	/// </summary>
	/// <returns>ExitCode: 0 (succeed), others (failed)</returns>
	public static async Task<int> RunBulkStreamingAsync(
		IEnumerable<string> commands,
		string? workingDirPath = null,
		string? stdin = null,
		Action<string>? onOutput = null,
		Action<string>? onError = null,
		CancellationToken cancellationToken = default
	) {
		using var process = new Process();
		process.StartInfo = new ProcessStartInfo {
			FileName = OperatingSystem.IsWindows() ? "cmd.exe" : "/bin/bash",
			UseShellExecute = false,
			RedirectStandardInput = true,
			RedirectStandardOutput = true,
			RedirectStandardError = true,
			CreateNoWindow = true,
			WorkingDirectory = workingDirPath ?? Environment.CurrentDirectory
		};
		process.EnableRaisingEvents = true;

		// TaskCompletionSources signal when stdout/stderr streams are fully drained
		var outputTcs = new TaskCompletionSource<bool>(TaskCreationOptions.RunContinuationsAsynchronously);
		var errorTcs = new TaskCompletionSource<bool>(TaskCreationOptions.RunContinuationsAsynchronously);

		// Handlers for stdout/stderr events
		DataReceivedEventHandler outputHandler = (_, e) => {
			if (e.Data is null) {
				outputTcs.TrySetResult(true); // end of stdout
			}
			else {
				onOutput?.Invoke(e.Data);
			}
		};

		DataReceivedEventHandler errorHandler = (_, e) => {
			if (e.Data is null) {
				errorTcs.TrySetResult(true); // end of stderr
			}
			else {
				onError?.Invoke(e.Data);
			}
		};

		process.OutputDataReceived += outputHandler;
		process.ErrorDataReceived += errorHandler;

		if (!process.Start()) {
			throw new InvalidOperationException($"---> Failed to start shell process: {process.StartInfo.FileName}");
		}

		// Begin async reading of stdout/stderr
		process.BeginOutputReadLine();
		process.BeginErrorReadLine();

		try {
			// Feed commands into the shell
			if (process.StartInfo.FileName.EndsWith("cmd.exe", StringComparison.OrdinalIgnoreCase)) {
				await process.StandardInput.WriteLineAsync("@echo off").ConfigureAwait(false);
			}

			foreach (var command in commands) {
				await process.StandardInput.WriteLineAsync(command).ConfigureAwait(false);
			}

			// If stdin content is provided, write it
			if (stdin != null) {
				await process.StandardInput.WriteAsync(stdin.AsMemory(), cancellationToken).ConfigureAwait(false);
			}

			// Important: tell the shell to exit after running commands
			await process.StandardInput.WriteLineAsync("exit").ConfigureAwait(false);
			process.StandardInput.Close();

			// Wait for process exit or cancellation
			await process.WaitForExitAsync(cancellationToken).ConfigureAwait(false);
		}
		catch (OperationCanceledException) {
			// Kill process if cancelled
			try {
				if (!process.HasExited) {
					process.Kill(entireProcessTree: true);
				}
			}
			catch {
				// ignored
			}
			throw;
		}

		// Ensure both stdout and stderr are fully drained
		await Task.WhenAll(outputTcs.Task, errorTcs.Task).ConfigureAwait(false);

		// Detach handlers to avoid leaks
		process.OutputDataReceived -= outputHandler;
		process.ErrorDataReceived -= errorHandler;

		return process.ExitCode;
	}
}

public record CommandResult(int exitCode, string output, string error) {
	public bool Succeed => this.exitCode == 0;

	public void ThrowIfFailed(string msg) {
		if (this.exitCode != 0) {
			throw new InvalidOperationException($"{msg}. ExitCode: {this.exitCode}, Output: {this.output}, Error: {this.error}");
		}
	}
}
