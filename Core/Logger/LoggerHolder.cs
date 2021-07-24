using Serilog;
using Serilog.Sinks.SystemConsole.Themes;
using static Serilog.Core.Logger;

namespace Core.Logger
{
	public static class LoggerHolder
	{
		public static ILogger Logger { get; } =
			new LoggerConfiguration()
				.MinimumLevel.Debug()
				.WriteTo.Console()
				.CreateLogger();
	}
}
