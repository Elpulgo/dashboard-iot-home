using Q42.HueApi;
using System.Threading.Tasks;

namespace DashboardIotHome.Repositories.PhilipsHue
{
    public interface IHueRegistry
    {
        Task<LocalHueClient> ConnectAsync();
    }
}
