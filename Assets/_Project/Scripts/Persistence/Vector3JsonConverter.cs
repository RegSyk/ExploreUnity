using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using UnityEngine;

namespace Explore.Persistence
{
    public class Vector3JsonConverter : JsonConverter
    {
        public override bool CanConvert(Type objectType) => objectType == typeof(Vector3);

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            Vector3 vector = (Vector3)value;
            writer.WriteStartObject();
            writer.WritePropertyName("X");
            writer.WriteValue(vector.x);
            writer.WritePropertyName("Y");
            writer.WriteValue(vector.y);
            writer.WritePropertyName("Z");
            writer.WriteValue(vector.z);
            writer.WriteEndObject();
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            JObject obj = JObject.Load(reader);
            return new Vector3(obj["X"].Value<float>(), obj["Y"].Value<float>(), obj["Z"].Value<float>());
        }
    }
}
