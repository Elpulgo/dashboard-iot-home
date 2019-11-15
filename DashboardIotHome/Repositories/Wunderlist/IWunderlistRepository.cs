using DashboardIotHome.Models.Wunderlist;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DashboardIotHome.Repositories.Wunderlist
{
    public interface IWunderlistRepository
    {
        Task<List<WunderlistViewModel>> GetDataAsync();
    }
}
