using Serilog;

namespace Core.Logger
{
	public static class LoggerHolder
	{
		private static ILogger _logger;

		public static LoggerConfiguration Configuration { get; set; } = new LoggerConfiguration().WriteTo.Console();

		public static ILogger Logger
		{
			get
			{
				if (_logger == null)
				{
					ConfigureLogger();
				}

				return _logger;
			}
		}

		private static void ConfigureLogger() => _logger = Configuration.CreateLogger();
	}
}
