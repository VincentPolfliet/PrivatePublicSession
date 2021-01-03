using System;
using System.Windows.Media;
using Newtonsoft.Json;

namespace UI.Theme
{
	internal class HexColorJsonConverter : JsonConverter<Color?>
	{
		public override void WriteJson(JsonWriter writer, Color? value, JsonSerializer serializer) => throw new NotImplementedException();

		public override Color? ReadJson(JsonReader reader, Type objectType, Color? existingValue, bool hasExistingValue,
			JsonSerializer serializer)
		{
			var obj = serializer.Deserialize<string>(reader);
			return HexColor.Parse(obj);
		}

		public override bool CanRead => true;
		public override bool CanWrite => false;
	}
}
