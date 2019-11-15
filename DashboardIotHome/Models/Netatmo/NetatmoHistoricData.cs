using Newtonsoft.Json;

namespace DashboardIotHome.Models
{
    public class NetatmoHistoricData
    {
        [JsonProperty("body")]
        public Step[] Steps { get; set; }

        [JsonIgnore]
        public string Name { get; set; }
    }

    public class Step
    {
        [JsonProperty("beg_time")]
        public int Start { get; set; }

        [JsonProperty("step_time")]
        public int Duration { get; set; }

        [JsonProperty("value")]
        public float[][] Values { get; set; }
    }
}
