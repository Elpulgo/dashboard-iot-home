using Microsoft.Extensions.Hosting;
using Serilog;
using System.Threading;
using System.Threading.Tasks;

namespace DashboardIotHome.Repositories
{
    public class LifetimeDataHostedService : IHostedService
    {
        private readonly IHostApplicationLifetime m_HostAppLifetime;

        public LifetimeDataHostedService(
            IHostApplicationLifetime hostAppLifetime)
        {
            m_HostAppLifetime = hostAppLifetime;
        }

        public Task StartAsync(CancellationToken cancellationToken)
        {
            m_HostAppLifetime.ApplicationStarted.Register(OnStarted);
            m_HostAppLifetime.ApplicationStopping.Register(OnStopping);
            m_HostAppLifetime.ApplicationStopped.Register(OnStopped);
            
            return Task.CompletedTask;
        }

        private void OnStopped()
        {
            Log.Information("Application stopped.");
        }

        private void OnStopping()
        {
            Log.Information("Application stopping...");
        }

        private void OnStarted()
        {
            Log.Information("Application started.");
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            return Task.CompletedTask;
        }
    }
}
