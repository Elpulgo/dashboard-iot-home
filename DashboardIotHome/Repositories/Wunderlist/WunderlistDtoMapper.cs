using DashboardIotHome.Models.Wunderlist;
using System.Collections.Generic;
using System.Linq;

namespace DashboardIotHome.Repositories.Wunderlist
{
    public static class WunderlistDtoMapper
    {
        public static WunderlistViewModel MapToList(string name, List<WunderlistTaskData> tasks)
        {
            var viewModel = new WunderlistViewModel(name);

            if (!tasks.Any())
                return viewModel;

            foreach (var task in tasks)
            {
                viewModel.Tasks.Add(new WunderlistTaskViewModel(task.Name, task.Type));
            }

            return viewModel;
        }
    }
}
