using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Flabs.Configuration.VaultSharp.Extensions
{
    public interface INameProvider
    {
        string Name { get; }
    }
}
