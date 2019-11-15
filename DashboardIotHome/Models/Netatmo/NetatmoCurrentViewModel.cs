using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DashboardIotHome.Models.Netatmo
{
    public class NetatmoCurrentViewModel
    {
        public NetatmoCurrentViewModel(string name)
        {
            Id = Guid.NewGuid();
            Name = name;
        }

        public Guid Id { get; set; }

        public string Name { get; set; }

        public float Temperature { get; set; }

        public float CO2 { get; set; }

        public int Humidity { get; set; }

        public float Pressure { get; set; }

        public float AbsolutePressure { get; set; }

        public float MinTemp { get; set; }

        public float MaxTemp { get; set; }

        public string TempTrend { get; set; }

        public string PressureTrend { get; set; }
    }
}
