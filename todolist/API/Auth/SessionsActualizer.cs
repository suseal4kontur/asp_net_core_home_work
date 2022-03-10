using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Hosting;

namespace API.Auth
{
    internal class SessionsActualizer : IHostedService, IDisposable
    {
        private const long PeriodInSeconds = 60;
        private IAuthenticator authenticator;
        private Timer timer;

        public SessionsActualizer(IAuthenticator authenticator)
        {
            this.authenticator = authenticator;
        }

        public Task StartAsync(CancellationToken cancellationToken)
        {
            timer = new Timer(this.DeleteExpiredSessions, null, TimeSpan.Zero,
                TimeSpan.FromSeconds(PeriodInSeconds));

            return Task.CompletedTask;
        }

        private void DeleteExpiredSessions(object state)
        {
            authenticator.DeleteExpiredSessions();
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            timer?.Change(Timeout.Infinite, 0);

            return Task.CompletedTask;
        }

        public void Dispose()
        {
            timer?.Dispose();
        }
    }
}
