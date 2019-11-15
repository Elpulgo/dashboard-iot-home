using System;
using System.Collections.Generic;

namespace DashboardIotHome.Models.Netatmo
{
    public class NetatmoSerieViewModel
    {
        public NetatmoSerieViewModel(string name, SeriesType type)
        {
            Id = Guid.NewGuid();
            Name = name;
            Type = type;
        }

        public Guid Id { get; set; }

        public string Name { get; set; }

        public SeriesType Type { get; set; }

        public List<NetatmoValueViewModel> Values { get; set; }
    }

    public class NetatmoValueViewModel
    {
        public float Value { get; set; }

        public DateTime Timestamp { get; set; }

        public override bool Equals(object obj) => obj is NetatmoValueViewModel other && this.Timestamp == other.Timestamp;

        public override int GetHashCode() => (Timestamp, Value).GetHashCode();
    }

    public enum SeriesType
    {
        Temperature,
        Humidity
    }
}
