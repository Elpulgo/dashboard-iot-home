using DashboardIotHome.Models.Netatmo;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DashboardIotHome.Repositories.Netatmo
{
    public interface INetatmoRepository
    {
        Task<List<NetatmoCurrentViewModel>> GetCurrentDataAsync();

        Task<List<NetatmoSerieViewModel>> GetHistoricDataAsync(DateTime start, DateTime end);
    }
}
