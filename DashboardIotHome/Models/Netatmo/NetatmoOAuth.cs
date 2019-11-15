using Newtonsoft.Json;
using System;

namespace DashboardIotHome.Models
{
    public class NetatmoOAuth
    {
        [JsonProperty("access_token")]
        public string AccessToken { get; set; }

        [JsonProperty("refresh_token")]
        public string RefreshToken { get; set; }

        [JsonProperty("scope")]
        public string[] Scope { get; set; }

        [JsonProperty("expires_in")]
        public int ExpiresIn { get; set; }

        [JsonIgnore]
        public DateTime Expires => DateTime.UtcNow.AddSeconds(TimeSpan.FromSeconds(ExpiresIn).TotalSeconds);
    }
}
