using DashboardIotHome.Models.PhilipsHue;
using Q42.HueApi;
using Q42.HueApi.ColorConverters.Original;
using System.Collections.Generic;
using System.Linq;

namespace DashboardIotHome.Repositories.PhilipsHue
{
    public static class HueDtoMapper
    {
        public static PhilipsHueViewModel Map(IEnumerable<Light> lights)
        {
            if (!lights.Any())
                return new PhilipsHueViewModel();

            return new PhilipsHueViewModel()
            {
                Lights = lights
                    .Select(light => new HueLight()
                    {
                        Name = light.Name,
                        HexColor = light.State.ToHex(),
                        Saturation = light.State.Saturation,
                        On = light.State.On
                    })
                    .ToList()
            };
        }
    }
}
