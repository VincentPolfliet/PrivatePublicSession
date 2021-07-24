using System.Runtime.InteropServices;
using System.Windows;
using Core.Identity;
using Core.Logger;
using UI.Configuration;

namespace UI
{
	/// <summary>
	/// Interaction logic for App.xaml
	/// </summary>
	public partial class App : Application
	{
		private const int AttachParentProcess = -1;

		[DllImport("kernel32.dll")]
		private static extern bool AttachConsole(int dwProcessId);
		protected override void OnStartup(StartupEventArgs e)
		{
			base.OnStartup(e);

			// SeriLog doesn't print to console in the latest Rider version (24/7/2021)
			// https://youtrack.jetbrains.com/issue/RIDER-15637#focus=Comments-27-4857606.0-0
			AttachToParentConsole();

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

		/// <summary>
		///     Redirects the console output of the current process to the parent process.
		/// </summary>
		/// <remarks>
		///     Must be called before calls to <see cref="Console.WriteLine()" />.
		/// </remarks>
		private static void AttachToParentConsole()
		{
			AttachConsole(AttachParentProcess);
		}
	}
}
