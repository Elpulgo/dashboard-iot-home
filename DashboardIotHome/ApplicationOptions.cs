using System.Collections.Generic;

namespace DashboardIotHome
{
    public class ApplicationOptions
    {
        public ApplicationOptions()
        {
        }

        public Netatmo Netatmo { get; set; }

        public Wunderlist Wunderlist { get; set; }

        public PhilipsHue PhilipsHue { get; set; }
    }

    public class Netatmo
    {
        public string ClientId { get; set; }

        public string ClientSecret { get; set; }

        public string DeviceId { get; set; }

        public string UserName { get; set; }

        public string Password { get; set; }

        public string OutdoorModuleId { get; set; }
    }

    public class Wunderlist
    {
        public string ClientId { get; set; }

        public string AccessToken { get; set; }

        public string ListFirst { get; set; }

        public string ListSecond { get; set; }

        public string ListThird { get; set; }

        public string ListFourth{ get; set; }

        public string ListFifth { get; set; }

        /// <summary>
        /// Docker env doesn't support nested lists so using this approach to be able to show many lists
        /// </summary>
        public List<string> Lists => new List<string>() { ListFirst, ListSecond, ListThird, ListFourth, ListFifth };
    }

    public class PhilipsHue
    {
        public string BridgeIp { get; set; }
    }
}
