using DashboardIotHome.Models;
using DashboardIotHome.Models.Netatmo;
using Serilog;
using System;
using System.Collections.Generic;
using System.Linq;

namespace DashboardIotHome.Repositories.Netatmo
{
    public static class NetatmoDtoMapper
    {
        public static List<NetatmoSerieViewModel> MapToSeries(List<NetatmoHistoricData> historicDatas)
        {
            var series = new List<NetatmoSerieViewModel>();

            foreach (var historicData in historicDatas)
            {
                if (historicData == null)
                    continue;

                series.Add(
                    new NetatmoSerieViewModel($"{historicData.Name} Temperature", SeriesType.Temperature)
                    {
                        Values = historicData.Steps
                    .SelectMany(step => step.Values
                        .Select(value => new NetatmoValueViewModel()
                        {
                            Timestamp = DateTimeOffset.FromUnixTimeSeconds(step.Start).DateTime,
                            Value = value.FirstOrDefault()
                        }))
                    .Distinct()
                    .ToList()
                    });

                series.Add(
                    new NetatmoSerieViewModel($"{historicData.Name} Humidity", SeriesType.Humidity)
                    {
                        Values = historicData.Steps
                     .SelectMany(step => step.Values
                         .Select(value => new NetatmoValueViewModel()
                         {
                             Timestamp = DateTimeOffset.FromUnixTimeSeconds(step.Start).DateTime,
                             Value = value.LastOrDefault()
                         }))
                     .Distinct()
                     .ToList()
                    });
            }

            return series;
        }

        public static List<NetatmoCurrentViewModel> MapToCurrent(NetatmoCurrentData data)
        {
            var models = new List<NetatmoCurrentViewModel>();

            var indoorModuleDevice = data.CurrentData.Devices
                .FirstOrDefault();

            if (indoorModuleDevice == null)
            {
                Log.Error("Failed to map netatmo current data for indoor module...");
                return models;
            }

            models.Add(
                new NetatmoCurrentViewModel("Indoor")
                {
                    AbsolutePressure = indoorModuleDevice.DashboardData.AbsolutePressure,
                    CO2 = indoorModuleDevice.DashboardData.CO2,
                    Humidity = indoorModuleDevice.DashboardData.Humidity,
                    MaxTemp = indoorModuleDevice.DashboardData.MaxTemp,
                    MinTemp = indoorModuleDevice.DashboardData.MinTemp,
                    Pressure = indoorModuleDevice.DashboardData.Pressure,
                    PressureTrend = indoorModuleDevice.DashboardData.PressureTrend,
                    Temperature = indoorModuleDevice.DashboardData.Temperature,
                    TempTrend = indoorModuleDevice.DashboardData.TempTrend
                });

            var outdoorModuleDevice = data.CurrentData.Devices
                .FirstOrDefault()?.Modules
                .FirstOrDefault();

            if (outdoorModuleDevice == null)
            {
                Log.Error("Failed to map netatmo current data for outdoor module...");
                return models;
            }

            models.Add(
                new NetatmoCurrentViewModel("Outdoor")
                {
                    AbsolutePressure = outdoorModuleDevice.DashboardData.AbsolutePressure,
                    CO2 = outdoorModuleDevice.DashboardData.CO2,
                    Humidity = outdoorModuleDevice.DashboardData.Humidity,
                    MaxTemp = outdoorModuleDevice.DashboardData.MaxTemp,
                    MinTemp = outdoorModuleDevice.DashboardData.MinTemp,
                    Pressure = outdoorModuleDevice.DashboardData.Pressure,
                    PressureTrend = outdoorModuleDevice.DashboardData.PressureTrend,
                    Temperature = outdoorModuleDevice.DashboardData.Temperature,
                    TempTrend = outdoorModuleDevice.DashboardData.TempTrend
                });

            return models;
        }
    }
}
