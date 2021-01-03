
using System.Collections.Generic;

using Core.Network;

using Newtonsoft.Json;
using UI.Theme;

namespace UI.Configuration
{
	public class AppConfig
	{
		[JsonProperty("check_admin")] public bool CheckForAdminPermissions { get; set; }

		[JsonProperty("theme")] public ConfigTheme Theme { get; set;  }

		[JsonProperty("ports")] public IEnumerable<ushort> Ports { get; set; }
	}
}
