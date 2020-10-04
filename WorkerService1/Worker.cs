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
            // \\ Loggningsfunktion, som har en Dependency-injection med nedan Constructor.. Som loggar ut i Consolen eller filen. Namnet �r ILogger & som �r private-class.
        private readonly ILogger<Worker> _logger;
        
        private TemperaturAvl�sning result;
        private string Temperatur = xxx;
        private TemperaturStart client;
        
            // \\ Constructor f�r Loggningen. Som inject'ar i logger. Dependency-injection med raden ovan.
        public Worker(ILogger<Worker> logger)
        {
            _logger = logger;
        }
        // \\ Startar tj�nsten
        public override Task StartAsync(CancellationToken cancellationToken)
        {
            client = new TemperaturAvl�sning();
            _logger.LogInformation("<<Tj�nsten har startat!>>");

            return base.StartAsync(cancellationToken);
        }
        // \\ Stoppar tj�nsten
        public override Task StopAsync(CancellationToken cancellationToken)
        {
            return base.StopAsync(cancellationToken);
        }

        // \\ Metoden nedan �r Async & kommer fr�n Kommer fr�n "IHostBuilder"-delen fr�n program.cs
        // \\ override betyder att den �r ExecuteAsync fr�m BackgroundService. // Virtual beyder att den �r valbar att vara �ver-skrivningsbar

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                //_logger.LogInformation("Worker running at: {time}", DateTimeOffset.Now);
                
                try
                {

                
                result = await client.GetAsync("");

                if (result.IsSuccessStatusCode)
                    _logger.LogInformation($"Temperaturen �r f�r H�G! = {result.StatusCode}");

                else
                    _logger.LogInformation($"Temperaturen �r f�r L�G! = {result.StatusCode}");
                }
                catch (Exception ex)
                _logger.LogInformation($"{ex.Message}");


                await Task.Delay(60*1000, stoppingToken);
            }
        }
    }
}
