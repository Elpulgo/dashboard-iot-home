using DashboardIotHome.Models.PhilipsHue;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DashboardIotHome.Repositories.PhilipsHue
{
    public interface IHueRepository
    {
        Task<PhilipsHueViewModel> GetLightsAsync();
    }
}
