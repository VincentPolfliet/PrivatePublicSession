using Serilog;
using static Serilog.Core.Logger;

namespace Core.Logger
{
	public static class LoggerHolder
	{
		private static ILogger logger = None;

		public static LoggerConfiguration Configuration { get; set; }

		public static ILogger Logger
		{
			get
			{
				if (logger == None)
				{
					ConfigureLogger();
				}

				return logger;
			}
		}

		private static void ConfigureLogger()
		{
			if (Configuration != null)
			{
				logger = Configuration.CreateLogger();
			}
		}
	}
}
