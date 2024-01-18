using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Flabs.Configuration.VaultSharp.Extensions
{
    internal class ConfigLoadService : BackgroundService
    {
        private readonly IServiceProvider _serviceProvider;
        private readonly ILogger<ConfigLoadService> _logger;
        private readonly FlabsConfigOptions _options;
        private readonly SemaphoreSlim _semaphore = new SemaphoreSlim(1, 1);
        public ConfigLoadService(IServiceProvider serviceProvider, ILogger<ConfigLoadService> logger, FlabsConfigOptions options)
        {
            _serviceProvider = serviceProvider;
            _logger = logger;
            _options = options;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            var configProvider = _serviceProvider.GetRequiredService<IConfigProvider>();
            while (!stoppingToken.IsCancellationRequested)
            {
                _logger.LogInformation("Background task is running...");
                await _semaphore.WaitAsync(stoppingToken);
                try
                {
                    await configProvider.LoadFromVaultOrDefaultAsync();
                    if (!_options.IsNeedReload)
                    {
                        _logger.LogInformation("Background task is stopping...");
                        return;
                    }
                    
                }
                finally
                {
                    _semaphore.Release();
                }
                await Task.Delay(TimeSpan.FromMinutes(_options.ReloadTimeMinute), stoppingToken);
            }
        }
    }
}
