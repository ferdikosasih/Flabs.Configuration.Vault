using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Flabs.Configuration.VaultSharp.Extensions
{
    public interface IConfigProvider
    {
        Task<bool> LoadFromVaultOrDefaultAsync();
        Task<TConfig> GetConfiguration<TConfig>(CancellationToken cancellationToken = default)
            where TConfig : class, IConfigurationSet, new();
    }
}
