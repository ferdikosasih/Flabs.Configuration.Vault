using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using VaultSharp;
using VaultSharp.Core;
using VaultSharp.V1.Commons;

namespace Flabs.Configuration.VaultSharp.Extensions
{
    internal class ConfigProvider : IConfigProvider
    {
        private readonly IVaultClient _vaultClient;
        private readonly ILogger<ConfigProvider> _logger;
        private readonly INameProvider _nameProvider;
        private readonly FlabsConfigOptions _vaultOptions;
        public ConfigProvider(
            INameProvider nameProvider
            , ILogger<ConfigProvider> logger
            , IVaultClient vaultClient
            , FlabsConfigOptions vaultOptions)
        {
            _nameProvider = nameProvider;
            _logger = logger;
            _vaultClient = vaultClient;
            _vaultOptions = vaultOptions;
        }

        public async Task<bool> LoadFromVaultOrDefaultAsync(IEnumerable<IConfigurationSet> configurationSets)
        {
            foreach (var config in configurationSets)
            {
                _logger.LogInformation($"Loads config from vault : {config.GetType().Name}");
                await LoadFromVault(config);
            }
            return true;
        }
        public async Task<TConfig> GetConfiguration<TConfig>(
        CancellationToken cancellationToken = default)
        where TConfig : class, IConfigurationSet, new()
        {
            TConfig config = Activator.CreateInstance<TConfig>();
            return await LoadConfig<TConfig>(config);
        }
        private async Task<TConfig> LoadConfig<TConfig>(TConfig config)
        where TConfig : class, IConfigurationSet, new()
        {
            string vaultPath = $"{_nameProvider.Name}/{config.GetType().Name}";
            _logger.LogInformation($"Loads config from vault : {vaultPath}");
            try
            {
                var secret = await _vaultClient.V1.Secrets.KeyValue.V2.ReadSecretAsync(
                path: vaultPath
                , mountPoint: _vaultOptions.VaultMountPoint);

                SetConfig(config, secret);
            }
            catch (VaultApiException ex)
            {
                if (ex.HttpStatusCode is System.Net.HttpStatusCode.NotFound)
                {
                    await CreateDefaultAsync(config, vaultPath);
                }
                else
                {
                    throw new InvalidOperationException($"Error loading {config.GetType().Name} configuration from Vault.", ex);
                }
            }
            return config;
        }

        private async Task<IConfigurationSet> LoadFromVault(IConfigurationSet config)
        {
            string vaultPath = $"{_nameProvider.Name}/{config.GetType().Name}";
            try
            {
                var secret = await _vaultClient.V1.Secrets.KeyValue.V2.ReadSecretAsync(
                path: vaultPath
                , mountPoint: _vaultOptions.VaultMountPoint);

                SetConfig(config, secret);
            }
            catch (VaultApiException ex)
            {
                if (ex.HttpStatusCode is System.Net.HttpStatusCode.NotFound)
                {
                    await CreateDefaultAsync(config, vaultPath);
                }
                else
                {
                    throw new InvalidOperationException($"Error loading {config.GetType().Name} configuration from Vault.", ex);
                }
            }
            return config;
        }
        private async Task<Secret<CurrentSecretMetadata>> CreateDefaultAsync(IConfigurationSet config, string vaultPath)
        {
            return await _vaultClient.V1.Secrets.KeyValue.V2.WriteSecretAsync(
                path: vaultPath
                , config
                , mountPoint: _vaultOptions.VaultMountPoint);
        }
        private void SetConfig<T>(T config, Secret<SecretData> secret) where T : IConfigurationSet
        {
            var properties = config.GetType().GetProperties(BindingFlags.IgnoreCase | BindingFlags.Public | BindingFlags.Instance);
            foreach (var prop in properties)
            {
                object? value;
                secret.Data.Data.TryGetValue(prop.Name, out value);
                SetProperty<T>(config, prop, value?.ToString());
            }
        }
        private static void SetProperty<T>(T config, PropertyInfo? property, object? value) where T : IConfigurationSet
        {
            if (property is null)
            {
                throw new InvalidOperationException(nameof(property));
            }

            if (property.CanWrite)
            {
                Type propertyType = property.PropertyType;

                // Convert the value to the property type
                object? convertedValue = Convert.ChangeType(value, propertyType);

                // Set the property value
                property.SetValue(config, convertedValue);
            }
        }
    }
}
