using Newtonsoft.Json;

namespace DashboardIotHome.Models.Wunderlist
{
    public class WunderlistListData
    {
        [JsonProperty("title")]
        public string Name { get; set; }

        [JsonProperty("id")]
        public int Id { get; set; }
    }
}
