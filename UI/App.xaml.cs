using System.Windows;
using Core.Identity;
using Core.Logger;
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

			var appConfig = ConfigHolder.Configuration = BuildConfig();

			if (!appConfig.CheckForAdminPermissions)
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

		private AppConfig BuildConfig()
		{
			return new ConfigLoader().Load();
		}

		private static LoggerConfiguration BuildLoggerConfig()
		{
			return new LoggerConfiguration()
				.MinimumLevel.Debug()
				.WriteTo.File("blocker.log")
				.WriteTo.Console();
		}
	}
}
