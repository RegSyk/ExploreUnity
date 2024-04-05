using Newtonsoft.Json;
namespace Explore.Persistence 
{
    public class JsonAppDataSerializer : ISerializer 
    {
        private JsonSerializerSettings settings;

        public JsonAppDataSerializer()
        {
            settings = new JsonSerializerSettings();
            settings.Converters.Add(new Vector3JsonConverter());
            settings.Converters.Add(new QuaternionJsonConverter());
        }

        public string Serialize<T>(T obj) => JsonConvert.SerializeObject(obj, settings);
        public T Deserialize<T>(string json) => JsonConvert.DeserializeObject<T>(json, settings);
    }
}