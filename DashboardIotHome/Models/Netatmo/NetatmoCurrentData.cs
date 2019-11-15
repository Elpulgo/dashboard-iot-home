using Newtonsoft.Json;

namespace DashboardIotHome.Models.Netatmo
{
    public class NetatmoCurrentData
    {
        [JsonProperty("body")]
        public CurrentData CurrentData { get; set; }
    }

    public class CurrentData
    {
        [JsonProperty("devices")]
        public Device[] Devices { get; set; }
    }

    public class Device
    {
        [JsonProperty("dashboard_data")]
        public DashboardData DashboardData { get; set; }

        [JsonProperty("modules")]
        public Module[] Modules { get; set; }
    }

    public class Module
    {
        [JsonProperty("dashboard_data")]
        public DashboardData DashboardData { get; set; }
    }

    public class DashboardData
    {
        [JsonProperty("temperature")]
        public float Temperature { get; set; }

        [JsonProperty("co2")]
        public float CO2 { get; set; }
        
        [JsonProperty("humidity")]
        public int Humidity { get; set; }

        [JsonProperty("pressure")]
        public float Pressure { get; set; }

        [JsonProperty("absolutePressure")]
        public float AbsolutePressure { get; set; }

        [JsonProperty("min_temp")]
        public float MinTemp { get; set; }

        [JsonProperty("max_temp")]
        public float MaxTemp { get; set; }

        [JsonProperty("temp_trend")]
        public string TempTrend { get; set; }

        [JsonProperty("pressure_trend")]
        public string PressureTrend { get; set; }
    }
}
