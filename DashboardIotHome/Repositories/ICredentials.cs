using DashboardIotHome.Models;
using System.Threading.Tasks;

namespace DashboardIotHome.Repositories
{
    public interface ICredentials
    {
        Task<NetatmoOAuth> GetNetatmoOAuthAsync();

        (string AccessToken, string ClientId) GetWunderlistCredentials();

        Task<(string AppKey, string AppName, string DeviceName)> GetHueCredentialsAsync();

        bool TryPersistHueAppKey(string appKey);
    }
}
