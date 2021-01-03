using System;
using Core.Network;
using Newtonsoft.Json;

namespace UI.Utils
{
	public class PortJsonConverter : JsonConverter<Port>
	{
		public override void WriteJson(JsonWriter writer, Port value, JsonSerializer serializer) =>
			throw new NotImplementedException();

		public override Port ReadJson(JsonReader reader, Type objectType, Port existingValue, bool hasExistingValue,
			JsonSerializer serializer)
		{
			var obj = serializer.Deserialize<ushort>(reader);
			return new Port(obj);
		}

		public override bool CanRead => true;
		public override bool CanWrite  => false;
	}
}
