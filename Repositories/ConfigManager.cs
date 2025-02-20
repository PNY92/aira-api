
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace AiraAPI.Repositories
{
    public static class ConfigManager
    {
        private const string _filePath = "config.json";

        public static JObject GetConfiguration()
        {
            string filePath = _filePath;
            string jsonString = File.ReadAllText(filePath);
            JObject? jObject = JsonConvert.DeserializeObject<dynamic>(jsonString);

            if (jObject == null)
            {
                throw new Exception("JSON Object is not deserialized or empty");
            }

            return jObject;
        }
    }
}
