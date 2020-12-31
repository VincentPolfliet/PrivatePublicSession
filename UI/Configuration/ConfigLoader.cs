using System.IO;
using Newtonsoft.Json;

namespace UI.Configuration
{
	public class ConfigLoader
	{
		public AppConfig Load()
		{
			var path = Path.Combine(Directory.GetCurrentDirectory(), "config.json");
			return JsonConvert.DeserializeObject<AppConfig>(File.ReadAllText(path));
		}
	}
}
