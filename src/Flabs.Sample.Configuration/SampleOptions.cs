using Flabs.Configuration.VaultSharp.Extensions;

namespace Flabs.Sample.Configuration
{
    public class SampleOptions : IConfigurationSet
    {
        public string SampleValue { get; set; } = string.Empty;
        public string SampleKey { get; set; } = string.Empty;
    }
}
