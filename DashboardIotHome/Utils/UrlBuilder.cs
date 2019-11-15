using System;

namespace DashboardIotHome.Utils
{
    public static class UrlBuilder
    {
        public static class Netatmo
        {
            private static string NetatmoBaseUrl = "https://api.netatmo.com";

            public static Uri BuildMeasureUrl(string accessToken, string deviceId, string moduleId, long start, long end) =>            
                new Uri($"{NetatmoBaseUrl}/api/getmeasure" +
                    $"?access_token={accessToken}" +
                    $"&device_id={deviceId}" +
                    $"&module_id={moduleId}" +
                    $"&date_begin={start}" +
                    $"&date_end={end}" +
                    $"&scale=max" +
                    $"&type=temperature,humidity");                

            public static Uri BuildStationUrl(string accessToken, string deviceId) =>
                new Uri($"{NetatmoBaseUrl}/api/getstationsdata" +
                    $"?access_token={accessToken}" +
                    $"&device_id={deviceId}");

            public static Uri BuildOauthTokenUrl() => 
                new Uri($"{NetatmoBaseUrl}/oauth2/token");
        }

        public static class Wunderlist
        {
            private static string WunderlistBaseUrl = "https://a.wunderlist.com/api/v1";

            public static Uri BuildListsUrl(string accessToken, string clientId) =>
                new Uri($"{WunderlistBaseUrl}/lists" +
                    $"?access_token={accessToken}" +
                    $"&client_id={clientId}");

            public static Uri BuildTasksUrl(string accessToken, string clientId, int listId) =>
                new Uri($"{WunderlistBaseUrl}/tasks" +
                    $"?access_token={accessToken}" +
                    $"&client_id={clientId}" +
                    $"&list_id={listId}");
        }
    }
}
