using System.Windows;
using Core.Logger;
using Serilog;

namespace UI
{
	public class RunAsAdminAlert
	{
		private ILogger _logger = LoggerHolder.Logger;

		public void Open()
		{
			MessageBox.Show("Please run this program as administrator!", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
		}
	}
}
