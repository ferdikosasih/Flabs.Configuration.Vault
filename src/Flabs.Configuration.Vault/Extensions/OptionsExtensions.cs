using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;

namespace Flabs.Configuration.VaultSharp.Extensions
{
    public static class OptionsExtensions
    {
        public static TOptions GetOptions<TOptions>(this IServiceScope scope)
            where TOptions : class, IConfigurationSet
        {
            return scope.ServiceProvider.GetRequiredService<IOptions<TOptions>>().Value;
        }
        public static TOptions GetOptions<TOptions>(this IServiceProvider serviceProvider)
            where TOptions : class, IConfigurationSet
        {
            return serviceProvider.GetRequiredService<IOptions<TOptions>>().Value;
        }
        public static IEnumerable<TOptions> GetOptionsCollection<TOptions>(this IServiceProvider serviceProvider)
            where TOptions : class, IConfigurationSet
        {
            return serviceProvider.GetRequiredService<IEnumerable<TOptions>>();
        }
    }
}
