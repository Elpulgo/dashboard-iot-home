using Microsoft.Extensions.Options;
using Q42.HueApi;
using Q42.HueApi.Models.Bridge;
using Serilog;
using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace DashboardIotHome.Repositories.PhilipsHue
{
    public class HueRegistry : IHueRegistry
    {
        private readonly ICredentials m_Credentials;

        public HueRegistry(
            IOptions<ApplicationOptions> options,
            ICredentials credentials)
        {
            Options = options;
            m_Credentials = credentials;
        }

        private IOptions<ApplicationOptions> Options { get; }

        public async Task<LocalHueClient> ConnectAsync()
        {
            LocalHueClient client = null;

            if (string.IsNullOrEmpty(Options.Value?.PhilipsHue?.BridgeIp))
            {
                Log.Error("Failed to conect to Philips Hue bridge, ip is null or empty, please register in your docker-compose file.");
                return null;
            }

            var (appKey, appName, deviceName) = await m_Credentials.GetHueCredentialsAsync();

            if (string.IsNullOrEmpty(appKey))
            {
                Log.Information("Failed to load app key for Philips Hue bridge, will try and register ...");

                client = await RegisterAsync(appName, deviceName);
            }

            try
            {
                if (client == null)
                {
                    client = new LocalHueClient(Options.Value?.PhilipsHue?.BridgeIp);
                }

                (appKey, appName, deviceName) = await m_Credentials.GetHueCredentialsAsync();

                client.Initialize(appKey);

                return client;
            }
            catch (Exception exception)
            {
                Log.Error(exception, $"Failed to connect to Philips Hue client with ip: '{Options.Value.PhilipsHue.BridgeIp}', '{exception.Message}'");
                return null;
            }
        }

        private async Task<LocatedBridge> LocateAsync()
        {
            if (string.IsNullOrEmpty(Options.Value?.PhilipsHue?.BridgeIp))
            {
                Log.Error("Failed to find Philips Hue bridge, ip is null or empty, please register in your docker-compose file.");
                return null;
            }

            var bridgeIP = Options.Value.PhilipsHue.BridgeIp;

            var locator = new HttpBridgeLocator();
            var bridgeIPs = (await locator.LocateBridgesAsync(TimeSpan.FromSeconds(10)))
                .ToList();

            return bridgeIPs
                .FirstOrDefault(bridge => bridge.IpAddress.Equals(bridgeIP, StringComparison.InvariantCultureIgnoreCase));
        }

        private async Task<LocalHueClient> RegisterAsync(string appName, string deviceName)
        {
            try
            {
                var locatedBridge = await LocateAsync();

                if (locatedBridge == null)
                {
                    Log.Error($"Failed to locate bridge with ip '{Options.Value.PhilipsHue.BridgeIp}' ... won't connect.");
                    return null;
                }

                var client = new LocalHueClient(locatedBridge.IpAddress);
                var appKey = await client.RegisterAsync(appName, deviceName);

                if (!m_Credentials.TryPersistHueAppKey(appKey))
                    return null;

                return client;
            }
            catch (LinkButtonNotPressedException linkButtonNotPressedException)
            {
                Log.Error(linkButtonNotPressedException, $"You must press button on bridge first time before connecting!");
                return null;
            }
            catch (Exception exception)
            {
                Log.Error(exception, $"Failed to register app to bridge '{exception.Message}'");
                return null;
            }
        }
    }
}
