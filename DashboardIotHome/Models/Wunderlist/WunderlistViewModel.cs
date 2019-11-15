using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DashboardIotHome.Models.Wunderlist
{
    public class WunderlistViewModel
    {
        public WunderlistViewModel(string name)
        {
            Name = name;
            Tasks = new List<WunderlistTaskViewModel>();
        }

        public string Name { get; set; }

        public List<WunderlistTaskViewModel> Tasks { get; set; }
    }

    public class WunderlistTaskViewModel
    {
        public WunderlistTaskViewModel(string name, string type)
        {
            Name = name;
            Type = type;
        }

        public string Name { get; set; }

        public string Type { get; set; }
    }
}
