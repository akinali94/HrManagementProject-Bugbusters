using Newtonsoft.Json;

namespace BugBustersHR.UI.Models
{
    public class Doviz
    {
        [JsonProperty("Alış")]
        public string Alış { get; set; }

        [JsonProperty("Satış")]
        public string Satış { get; set; }
    }
}
