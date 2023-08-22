using BugBustersHR.ENTITY.Enums;
using Newtonsoft.Json;

namespace BugBustersHR.UI.Models
{
    public class DovizModel
    {
        public DovizModel()
        {
            DovizList = new Dictionary<Currency, Doviz>();
        }

        [JsonProperty("Update_Date")]
        public string UpdateDate { get; set; }


        [JsonProperty("USD")]
        public Doviz USD { get; set; }

        [JsonProperty("EUR")]
        public Doviz EUR { get; set; }

        public Dictionary<Currency, Doviz> DovizList { get; set; }
    }
}
