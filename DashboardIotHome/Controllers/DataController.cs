using DashboardIotHome.Repositories.Netatmo;
using DashboardIotHome.Repositories.PhilipsHue;
using DashboardIotHome.Repositories.Wunderlist;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using System;
using System.Threading.Tasks;

namespace DashboardIotHome.Controllers
{
    [Route("api/v1/{controller}")]
    public class DataController : Controller
    {
        private readonly INetatmoRepository m_NetatmoRepository;
        private readonly IWunderlistRepository m_WunderlistRepository;
        private readonly IHueRepository m_HueRepository;

        public DataController(
            INetatmoRepository netatmoRepository,
            IWunderlistRepository wunderlistRepository,
            IHueRepository hueRepository)
        {
            m_NetatmoRepository = netatmoRepository;
            m_WunderlistRepository = wunderlistRepository;
            m_HueRepository = hueRepository;
        }

        public IOptions<ApplicationOptions> Options { get; }

        [HttpGet("netatmo/current")]
        public async Task<IActionResult> GetNetatmoCurrentAsync()
        {
            try
            {
                var currentData = await m_NetatmoRepository.GetCurrentDataAsync();
                return Ok(currentData);
            }
            catch (Exception exception)
            {
                return BadRequest($"Failed to get current Netatmo data. Check logs for more details ... '{exception.Message}'");
            }
        }

        [HttpGet("netatmo/series")]
        public async Task<IActionResult> GetNetatmoSeriesAsync()
        {
            try
            {
                var historicData = await m_NetatmoRepository.GetHistoricDataAsync(DateTime.Now.AddDays(-3), DateTime.Now);
                return Ok(historicData);
            }
            catch (Exception exception)
            {
                return BadRequest($"Failed to get Netatmo series. Check logs for more details ... '{exception.Message}'");
            }
        }

        [HttpGet("wunderlist")]
        public async Task<IActionResult> GetWunderlistAsync()
        {
            try
            {
                var wunderLists = await m_WunderlistRepository.GetDataAsync();
                return Ok(wunderLists);
            }
            catch (Exception exception)
            {
                return BadRequest($"Failed to get Wunderlist data. Check logs for more details ... '{exception.Message}'");
            }
        }

        [HttpGet("hue")]
        public async Task<IActionResult> GetHueAsync()
        {
            try
            {
                var hue = await m_HueRepository.GetLightsAsync();
                return Ok(hue);
            }
            catch (Exception exception)
            {
                return BadRequest($"Failed to get Philips Hue data. Check logs for more details ... '{exception.Message}'");
            }
        }
    }
}
