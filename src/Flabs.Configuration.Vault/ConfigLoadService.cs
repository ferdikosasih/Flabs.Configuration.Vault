using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
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
        private readonly List<IConfigurationSet> configurationSets = new List<IConfigurationSet>();
        public ConfigLoadService(IServiceProvider serviceProvider, ILogger<ConfigLoadService> logger, FlabsConfigOptions options)
        {
            _serviceProvider = serviceProvider;
            _logger = logger;
            _options = options;
            InitializeConfigurationSets();
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
                    await configProvider.LoadFromVaultOrDefaultAsync(configurationSets);
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
        private void InitializeConfigurationSets()
        {
            var assembly = Assembly.GetEntryAssembly();
            var types = assembly?.GetTypes();
            if (types is null)
            {
                throw new InvalidOperationException("Unable to get assembly entry");
            }

            var configurationTypeSets = types
                .Where(type => typeof(IConfigurationSet).IsAssignableFrom(type) && type.IsClass);

            foreach (var item in configurationTypeSets)
            {
                IConfigurationSet configurationSet = (IConfigurationSet)_serviceProvider.GetRequiredService(item);
                configurationSets.Add(configurationSet!);
            }
        }
    }
}
