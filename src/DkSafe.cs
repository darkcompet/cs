#pragma warning disable IDE0130 // Namespace がフォルダー構造と一致しません
namespace Tool.Compet.Core;

using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

public static class DkSafe {
	/// <summary>
	/// Run an action safely (no throw)
	/// </summary>
	public static void Run(Action action) {
		try {
			action();
		}
		catch (Exception e) {
			Console.WriteLine($"[DkSafe] Safe with error: {e.Message}");
		}
	}

	/// <summary>
	/// Run an async action safely (no throw)
	/// </summary>
	public static async Task RunAsync(Func<Task> action) {
		try {
			await action();
		}
		catch (Exception e) {
			Console.WriteLine($"[DkSafe] Safe with error: {e.Message}");
		}
	}

	/// <summary>
	/// Run a function safely and return result or fallback value
	/// </summary>
	public static T? Run<T>(Func<T> func, T? fallback = default) {
		try {
			return func();
		}
		catch (Exception e) {
			Console.WriteLine($"[DkSafe] Safe with error: {e.Message}");
			return fallback;
		}
	}

	/// <summary>
	/// Run async function safely and return result or fallback
	/// </summary>
	public static async Task<T?> RunAsync<T>(Func<Task<T>> func, T? fallback = default) {
		try {
			return await func();
		}
		catch (Exception e) {
			Console.WriteLine($"[DkSafe] Safe with error: {e.Message}");
			return fallback;
		}
	}

	/// <summary>
	/// Retry an async action multiple times before giving up
	/// </summary>
	public static async Task RunWithRetryAsync(Func<Task> action, int maxRetries = 3, int delayMs = 1000, CancellationToken token = default) {
		for (var attempt = 1; attempt <= maxRetries; ++attempt) {
			try {
				await action();
				return;
			}
			catch (Exception e) {
				Console.WriteLine($"[DkSafe] Safe retry {attempt}/{maxRetries}, error: {e.Message}");
				if (attempt < maxRetries && !token.IsCancellationRequested) {
					await Task.Delay(delayMs, token);
				}
			}
		}
		Console.WriteLine("[DkSafe] Safe max retries reached!");
	}

	/// <summary>
	/// Retry an async function returning a result.
	/// </summary>
	public static async Task<T?> RunWithRetryAsync<T>(Func<Task<T>> func,
		T? fallback = default,
		int maxRetries = 3,
		int delayMs = 1000,
		CancellationToken token = default
	) {
		for (var attempt = 1; attempt <= maxRetries; ++attempt) {
			try {
				return await func();
			}
			catch (Exception e) {
				Console.WriteLine($"[DkSafe] Safe retry {attempt}/{maxRetries}, error: {e.Message}");
				if (attempt < maxRetries && !token.IsCancellationRequested) {
					await Task.Delay(delayMs, token);
				}
			}
		}
		Console.WriteLine("[DkSafe] Safe max retries reached!");
		return fallback;
	}

	/// <summary>
	/// Safe disposal of IDisposable objects
	/// </summary>
	public static void Dispose(IDisposable? disposable) {
		if (disposable != null) {
			try {
				disposable.Dispose();
			}
			catch (Exception e) {
				Console.WriteLine($"[DkSafe] Safe dispose failed: {e.Message}");
			}
		}
	}

	/// <summary>
	/// Safe async disposal of IAsyncDisposable objects
	/// </summary>
	public static async Task DisposeAsync(IAsyncDisposable? disposable) {
		if (disposable != null) {
			try {
				await disposable.DisposeAsync();
			}
			catch (Exception e) {
				Console.WriteLine($"[DkSafe] Safe async dispose failed: {e.Message}");
			}
		}
	}

	/// <summary>
	/// Run multiple actions and collect any errors
	/// </summary>
	public static List<Exception> RunAll(params Action[] actions) {
		var errors = new List<Exception>();
		foreach (var action in actions) {
			try {
				action();
			}
			catch (Exception e) {
				errors.Add(e);
				Console.WriteLine($"[DkSafe] Safe batch error: {e.Message}");
			}
		}
		if (errors.Count > 0) {
			Console.WriteLine($"[DkSafe] Completed with {errors.Count} error(s).");
		}
		return errors;
	}
}
