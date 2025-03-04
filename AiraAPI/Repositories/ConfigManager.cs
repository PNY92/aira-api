
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace AiraAPI.Repositories
{
    public static class ConfigManager
    {
        private const string _filePath_dev = "config-dev.json";
        private const string _filePath = "config.json";

        public static JObject GetConfiguration()
        {
            string filePath = String.Empty;
            if (Path.Exists("config-dev.json"))
            {
                filePath = _filePath_dev;
            }
            else
            {
                filePath = _filePath;
            }
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
