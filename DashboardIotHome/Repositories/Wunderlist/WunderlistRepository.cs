using DashboardIotHome.Models.Wunderlist;
using DashboardIotHome.Repositories.Wunderlist;
using DashboardIotHome.Utils;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using Serilog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DashboardIotHome.Repositories.Wunderlist
{
    public class WunderlistRepository : IWunderlistRepository
    {
        private readonly HttpWrapper m_HttpWrapper;
        private readonly ICredentials m_Credentials;

        public WunderlistRepository(
            IOptions<ApplicationOptions> options,
            HttpWrapper httpWrapper,
            ICredentials credentials)
        {
            Options = options;
            m_HttpWrapper = httpWrapper;
            m_Credentials = credentials;
        }

        private IOptions<ApplicationOptions> Options { get; }


        public async Task<List<WunderlistViewModel>> GetDataAsync()
        {
            try
            {
                var (accessToken, clientId) = m_Credentials.GetWunderlistCredentials();

                if (!Options.Value.Wunderlist.Lists.Any())
                {
                    Log.Error("No lists specified for Wunderlist API. Update your env variable 'Wunderlist__Lists'.");
                    return null;
                }

                var response = await m_HttpWrapper.GetAsync(
                    UrlBuilder.Wunderlist.BuildListsUrl(accessToken, clientId));

                var wunderlistItems = new List<WunderlistViewModel>();

                var tasks = new List<Task>();

                foreach (var data in FilterData(response))
                {
                    tasks.Add(Task.Run(async () =>
                    {
                        var foundTasks = await GetTaskItemsForListAsync(data.Id);
                        wunderlistItems.Add(WunderlistDtoMapper.MapToList(data.Name, foundTasks));
                    }));
                }

                await Task.WhenAll(tasks);

                return wunderlistItems;
            }
            catch (Exception exception)
            {
                Log.Error(exception, $"Failed to get lists from Wunderlist: '{exception.Message}'");
            }

            return null;
        }

        private List<WunderlistListData> FilterData(string response)
        {
            if (string.IsNullOrEmpty(response))
                return null;

            var wunderlistData = JsonConvert.DeserializeObject<List<WunderlistListData>>(response);

            return wunderlistData
                .Where(listItem => Options.Value.Wunderlist.Lists.Contains(listItem.Name))
                .ToList();
        }

        private async Task<List<WunderlistTaskData>> GetTaskItemsForListAsync(int id)
        {
            try
            {
                var (accessToken, clientId) = m_Credentials.GetWunderlistCredentials();

                var response = await m_HttpWrapper.GetAsync(
                    UrlBuilder.Wunderlist.BuildTasksUrl(accessToken, clientId, id));

                var tasks = JsonConvert.DeserializeObject<List<WunderlistTaskData>>(response);
                return tasks;
            }
            catch (Exception exception)
            {
                Log.Error(exception, $"Failed to get tasks from Wunderlist for list id '{id}': '{exception.Message}'");
            }

            return null;
        }
    }
}
