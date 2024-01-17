using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using System;
using VaultSharp;
using VaultSharp.V1.AuthMethods.Token;

namespace Flabs.Configuration.VaultSharp.Extensions
{
    public static class FlabsConfigurationExtensions
    {
        public static IServiceCollection AddFlabsConfig(
            this IServiceCollection services
            , VaultOptions options)
        {
            services
                .AddSingleton<INameProvider, NameProvider>()
                .AddSingleton(options)
                .AddSingleton<IVaultClient>(o =>
                {
                    var token = new TokenAuthMethodInfo(options.VaultToken);
                    var setting = new VaultClientSettings(options.VaultAddress, token);
                    return new VaultClient(setting);
                })
                .AddSingleton<IConfigProvider, ConfigProvider>();
            return services;
        }
        public static IServiceCollection AddFlabsConfig(this IServiceCollection services)
        {
            string? vaultToken = Environment.GetEnvironmentVariable(VaultEnvironmentName.VAULT_TOKEN);
            string? vaultAddress = Environment.GetEnvironmentVariable(VaultEnvironmentName.VAULT_ADDR);
            if (vaultToken == null)
            {
                throw new InvalidOperationException($"Unable to get vault environment variable {VaultEnvironmentName.VAULT_TOKEN} ");
            }
            if (vaultAddress == null)
            {
                throw new InvalidOperationException($"Unable to get vault environment variable {VaultEnvironmentName.VAULT_ADDR} ");
            }
            VaultOptions options = new VaultOptions(vaultToken,vaultAddress);
            return AddFlabsConfig(services,options);
        }
        public static IServiceCollection AddConfigOptions<TConfig>(
        this IServiceCollection services)
        where TConfig : class, IConfigurationSet, new()
        {
            services.TryAddSingleton<TConfig>(sp =>
            {
                var configProvider = sp.GetRequiredService<IConfigProvider>();
                return configProvider.GetConfiguration<TConfig>().GetAwaiter().GetResult();
            });
            return services;
        }
        public static T GetConfig<T>(this IServiceProvider serviceProvider) where T : IConfigurationSet
        {
            var config = serviceProvider.GetRequiredService<T>();
            return config;
        }
    }
}
