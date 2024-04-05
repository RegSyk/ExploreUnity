using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using UnityEngine;

namespace Explore.Persistence
{
    public class QuaternionJsonConverter : JsonConverter
    {
        public override bool CanConvert(Type objectType) => objectType == typeof(Quaternion);

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            Quaternion quaternion = (Quaternion)value;
            writer.WriteStartObject();
            writer.WritePropertyName("X");
            writer.WriteValue(quaternion.x);
            writer.WritePropertyName("Y");
            writer.WriteValue(quaternion.y);
            writer.WritePropertyName("Z");
            writer.WriteValue(quaternion.z);
            writer.WritePropertyName("W");
            writer.WriteValue(quaternion.w);
            writer.WriteEndObject();
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            JObject obj = JObject.Load(reader);
            return new Quaternion(obj["X"].Value<float>(), obj["Y"].Value<float>(), obj["Z"].Value<float>(), obj["W"].Value<float>());
        }
    }
}
