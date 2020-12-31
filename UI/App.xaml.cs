using System.Windows;
using Core.Identity;
using Core.Logger;
using Microsoft.Extensions.Configuration;
using Serilog;
using UI.Configuration;

namespace UI
{
	/// <summary>
	/// Interaction logic for App.xaml
	/// </summary>
	public partial class App : Application
	{
		protected override void OnStartup(StartupEventArgs e)
		{
			base.OnStartup(e);

			LoggerHolder.Configuration = BuildLoggerConfig();
			var config = ConfigHolder.Configuration = new AppConfig(BuildConfig());

			if (!config.CheckForAdminPermissions)
			{
				return;
			}

			if (IdentityHelper.IsRunAsAdmin())
			{
				return;
			}

			var logger = LoggerHolder.Logger;
			logger.Fatal("program is not run as administrator");

			new RunAsAdminAlert().Open();
			Current.Shutdown();
		}

		private static LoggerConfiguration BuildLoggerConfig()
		{
			return new LoggerConfiguration()
				.MinimumLevel.Debug()
				.WriteTo.File("blocker.log")
				.WriteTo.Console();
		}

		private static IConfiguration BuildConfig()
		{
			return new ConfigurationBuilder()
				.AddJsonFile("config.json", optional: false)
				.AddEnvironmentVariables()
				.Build();
		}
	}
}
