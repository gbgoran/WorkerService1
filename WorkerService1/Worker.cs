using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace WorkerService1
{
    public class Worker : BackgroundService
    {
            // \\ Loggningsfunktion, som har en Dependency-injection med nedan Constructor.. Som loggar ut i Consolen eller filen. Namnet är ILogger & som är private-class.
        private readonly ILogger<Worker> _logger;
        
        private TemperaturAvläsning result;
        private string Temperatur = xxx;
        private TemperaturStart client;
        
            // \\ Constructor för Loggningen. Som inject'ar i logger. Dependency-injection med raden ovan.
        public Worker(ILogger<Worker> logger)
        {
            _logger = logger;
        }
        // \\ Startar tjänsten
        public override Task StartAsync(CancellationToken cancellationToken)
        {
            client = new TemperaturAvläsning();
            _logger.LogInformation("<<Tjänsten har startat!>>");

            return base.StartAsync(cancellationToken);
        }
        // \\ Stoppar tjänsten
        public override Task StopAsync(CancellationToken cancellationToken)
        {
            return base.StopAsync(cancellationToken);
        }

        // \\ Metoden nedan är Async & kommer från Kommer från "IHostBuilder"-delen från program.cs
        // \\ override betyder att den är ExecuteAsync fråm BackgroundService. // Virtual beyder att den är valbar att vara över-skrivningsbar

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                //_logger.LogInformation("Worker running at: {time}", DateTimeOffset.Now);
                
                try
                {

                
                result = await client.GetAsync("");

                if (result.IsSuccessStatusCode)
                    _logger.LogInformation($"Temperaturen är för HÖG! = {result.StatusCode}");

                else
                    _logger.LogInformation($"Temperaturen är för LÅG! = {result.StatusCode}");
                }
                catch (Exception ex)
                _logger.LogInformation($"{ex.Message}");


                await Task.Delay(60*1000, stoppingToken);
            }
        }
    }
}
