using MassTransit;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Northwind.WebUI.Infrastructure
{
    public class MassTransitHostedService : IHostedService
    {
        private readonly IBusControl _bus;

        public MassTransitHostedService(IBusControl bus)
        {
            _bus = bus;
        }    

        public async Task StartAsync(CancellationToken cancellationToken)
        {
            await _bus.StartAsync(cancellationToken);
        }

        public async Task StopAsync(CancellationToken cancellationToken)
        {
            await _bus.StopAsync(TimeSpan.FromSeconds(10));
        }
    }
}
