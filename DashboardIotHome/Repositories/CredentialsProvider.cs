using DashboardIotHome.Models;
using DashboardIotHome.Utils;
using Microsoft.Extensions.Options;
using Serilog;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

namespace DashboardIotHome.Repositories
{
    public class CredentialsProvider : ICredentials
    {
        private const string DeviceName = "b5c92462-aede-47de-bb54-48dfada69f59";
        private const string AppName = "IoTHomeDashboard";

        private readonly HttpWrapper m_HttpWrapper;

        private IOptions<ApplicationOptions> Options { get; }

        private NetatmoOAuth Token { get; set; }

        private string HueAppKey { get; set; }

        private string HueAppKeyPath => Path.Combine(Directory.GetCurrentDirectory(), "settings", "hueappkey.dat");

        public CredentialsProvider(
            IOptions<ApplicationOptions> options,
            HttpWrapper httpWrapper)
        {
            m_HttpWrapper = httpWrapper;
            Options = options;
        }

        public async Task<(string AppKey, string AppName, string DeviceName)> GetHueCredentialsAsync()
        {
            var appKey = await LoadAppKeyAsync();

            if (!string.IsNullOrEmpty(appKey))
            {
                HueAppKey = appKey;
            }

            return (appKey, AppName, DeviceName);
        }

        public async Task<NetatmoOAuth> GetNetatmoOAuthAsync()
        {
            if (Token != null && DateTime.UtcNow.AddMinutes(-5) <= Token.Expires)
                return Token;

            var body = new Dictionary<string, string>
            {
                {"grant_type", "password"},
                {"client_id", Options.Value.Netatmo.ClientId},
                {"client_secret", Options.Value.Netatmo.ClientSecret},
                {"username", Options.Value.Netatmo.UserName},
                {"password", Options.Value.Netatmo.Password}
            };

            var token = await m_HttpWrapper.PostFormUrlEncodedAsync<NetatmoOAuth>(body, UrlBuilder.Netatmo.BuildOauthTokenUrl());

            Token = token;
            return token;
        }

        public (string AccessToken, string ClientId) GetWunderlistCredentials()
        {
            if (string.IsNullOrEmpty(Options.Value?.Wunderlist?.AccessToken))
            {
                Log.Error($"Wunderlist API access token was not provided. Change your docker-compose file.");
                return (AccessToken: null, ClientId: null);
            }

            if (string.IsNullOrEmpty(Options.Value?.Wunderlist?.ClientId))
            {
                Log.Error($"Wunderlist API client id was not provided. Change your docker-compose file.");
                return (Options.Value.Wunderlist.AccessToken, ClientId: null);
            }

            return (Options.Value.Wunderlist.AccessToken, Options.Value.Wunderlist.ClientId);
        }

        public bool TryPersistHueAppKey(string appKey)
        {
            if (string.IsNullOrEmpty(appKey))
                return false;

            try
            {
                File.WriteAllText(HueAppKeyPath, appKey);

                HueAppKey = appKey;

                return true;
            }
            catch (Exception exception)
            {
                Log.Error(exception, $"Failed to persist app key for Philips Hue bridge '{exception.Message}'");
                return false;
            }
        }

        private async Task<string> LoadAppKeyAsync()
        {
            try
            {
                if (!string.IsNullOrEmpty(HueAppKey))
                    return HueAppKey;

                if (!File.Exists(HueAppKeyPath))
                    return string.Empty;

                var appKey = await File.ReadAllTextAsync(HueAppKeyPath);

                return appKey;
            }
            catch (Exception exception)
            {
                Log.Error(exception, $"Failed to load appkey '{exception.Message}'");
                return string.Empty;
            }
        }
    }
}
