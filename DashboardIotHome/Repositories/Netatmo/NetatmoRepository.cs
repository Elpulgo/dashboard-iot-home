using DashboardIotHome.Models;
using DashboardIotHome.Models.Netatmo;
using DashboardIotHome.Repositories.Netatmo;
using DashboardIotHome.Utils;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using Serilog;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DashboardIotHome.Repositories.Netatmo
{
    public class NetatmoRepository : INetatmoRepository
    {
        private readonly HttpWrapper m_HttpWrapper;
        private readonly ICredentials m_Credentials;

        public NetatmoRepository(
            ICredentials credentials,
            IOptions<ApplicationOptions> options,
            HttpWrapper httpWrapper)
        {
            Options = options;
            m_HttpWrapper = httpWrapper;
            m_Credentials = credentials;
        }

        private IOptions<ApplicationOptions> Options { get; }

        public async Task<List<NetatmoCurrentViewModel>> GetCurrentDataAsync()
        {
            try
            {
                var token = await m_Credentials.GetNetatmoOAuthAsync();

                if (string.IsNullOrEmpty(token.AccessToken) || string.IsNullOrEmpty(token.RefreshToken) || string.IsNullOrEmpty(Options.Value?.Netatmo?.DeviceId))
                {
                    Log.Error($"Failed to get current data from Netatmo API with accesstoken: '{token?.AccessToken}' and deviceid: '{Options.Value?.Netatmo?.DeviceId}'");
                    return null;
                }

                var response = await m_HttpWrapper.GetAsync(
                    UrlBuilder.Netatmo.BuildStationUrl(token.AccessToken, Options.Value.Netatmo.DeviceId));

                var currentData = JsonConvert.DeserializeObject<NetatmoCurrentData>(response);

                return NetatmoDtoMapper.MapToCurrent(currentData);
            }
            catch (Exception exception)
            {
                Log.Error(exception, $"Failed to get current data from Netatmo: '{exception.Message}'");
                throw;
            }
        }

        public async Task<List<NetatmoSerieViewModel>> GetHistoricDataAsync(DateTime start, DateTime end)
        {
            try
            {
                var token = await m_Credentials.GetNetatmoOAuthAsync();

                if (string.IsNullOrEmpty(token.AccessToken) || string.IsNullOrEmpty(token.RefreshToken) || string.IsNullOrEmpty(Options.Value?.Netatmo?.DeviceId))
                {
                    Log.Error($"Failed to get historic data from Netatmo API with accesstoken: '{token?.AccessToken}' and deviceid: '{Options.Value?.Netatmo?.DeviceId}'");
                    return null;
                }

                var unixStart = ((DateTimeOffset)start).ToUnixTimeSeconds();
                var unixEnd = ((DateTimeOffset)end).ToUnixTimeSeconds();

                var responseIndoor = await m_HttpWrapper.GetAsync(
                    UrlBuilder.Netatmo.BuildMeasureUrl(token.AccessToken, Options.Value.Netatmo.DeviceId, null, unixStart, unixEnd));

                var historicDataIndoor = JsonConvert.DeserializeObject<NetatmoHistoricData>(responseIndoor);
                historicDataIndoor.Name = "Indoor";

                NetatmoHistoricData historicDataOutdoor = null;

                if (!string.IsNullOrEmpty(Options.Value?.Netatmo?.OutdoorModuleId))
                {
                    var responseOutdoor = await m_HttpWrapper.GetAsync(
                        UrlBuilder.Netatmo.BuildMeasureUrl(token.AccessToken, Options.Value.Netatmo.DeviceId, Options.Value.Netatmo.OutdoorModuleId, unixStart, unixEnd));

                    historicDataOutdoor = JsonConvert.DeserializeObject<NetatmoHistoricData>(responseOutdoor);
                    historicDataOutdoor.Name = "Outdoor";
                }

                return NetatmoDtoMapper.MapToSeries(new List<NetatmoHistoricData>() { historicDataIndoor, historicDataOutdoor });
            }
            catch (Exception exception)
            {
                Log.Error(exception, $"Failed to get historic data from Netatmo: '{exception.Message}'");
                throw;
            }
        }
    }
}
