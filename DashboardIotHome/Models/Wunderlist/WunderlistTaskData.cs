using Newtonsoft.Json;

namespace DashboardIotHome.Models.Wunderlist
{
    public class WunderlistTaskData
    {
        [JsonProperty("id")]
        public long Id { get; set; }

        [JsonProperty("list_id")]
        public int ListId { get; set; }

        [JsonProperty("title")]
        public string Name { get; set; }

        [JsonProperty("type")]
        public string Type { get; set; }
    }
}
