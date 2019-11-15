using System.Collections.Generic;

namespace DashboardIotHome.Models.PhilipsHue
{
    public class PhilipsHueViewModel
    {
        public List<HueLight> Lights { get; set; }
    }

    public class HueLight
    {
        public string Name { get; set; }

        public string HexColor { get; set; }

        public int? Saturation { get; set; }

        public bool On { get; set; }
    }
}
