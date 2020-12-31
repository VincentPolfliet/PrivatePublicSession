using Microsoft.Extensions.Configuration;
using UI.Theme;

namespace UI.Configuration
{
	public class AppConfig
	{
		private readonly IConfiguration _config;

		public bool CheckForAdminPermissions => _config.GetValue<bool>("check_admin", true);

		public ITheme Theme
		{
			get
			{
				var theme = _config.GetValue<ConfigTheme>("theme");
				return theme ?? ITheme.Default;
			}
		}

		public AppConfig(IConfiguration config)
		{
			_config = config;
		}

		public string this[string key] => _config[key];
	}
}
