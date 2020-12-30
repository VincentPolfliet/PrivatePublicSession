using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using Core.Logger;
using Serilog;

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

			LoggerHolder.Configuration = new LoggerConfiguration()
				.MinimumLevel.Debug()
				.WriteTo.File("logs\\portlblocker.log", rollingInterval: RollingInterval.Day)
				.WriteTo.Console();
		}
	}
}
