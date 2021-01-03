using System.Windows.Media;
using Newtonsoft.Json;

namespace UI.Theme
{
	public class ConfigTheme: ITheme
	{
		[JsonProperty("background")]
		[JsonConverter(typeof(HexColorJsonConverter))]
		public Color Background { get; set; }

		[JsonProperty("rules_active")]
		[JsonConverter(typeof(HexColorJsonConverter))]
		public Color RulesActive { get; set; }

		[JsonProperty("rules_notactive")]
		[JsonConverter(typeof(HexColorJsonConverter))]
		public Color RulesInactive { get; set; }

	}
}
