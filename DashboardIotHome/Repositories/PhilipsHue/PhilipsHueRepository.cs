using DashboardIotHome.Models.PhilipsHue;
using Serilog;
using System;
using System.Threading.Tasks;

namespace DashboardIotHome.Repositories.PhilipsHue
{
    public class PhilipsHueRepository : IHueRepository
    {
        private readonly IHueRegistry m_HueRegistry;

        public PhilipsHueRepository(
            IHueRegistry hueRegistry)
        {
            m_HueRegistry = hueRegistry;
        }

        public async Task<PhilipsHueViewModel> GetLightsAsync()
        {
            try
            {
                var client = await m_HueRegistry.ConnectAsync();
                var lights = await client.GetLightsAsync();

                return HueDtoMapper.Map(lights);
            }
            catch (Exception exception)
            {
                Log.Error(exception, $"Failed to get lights from Philips Hue '{exception.Message}'");
            }

            return null;
        }
    }
}
