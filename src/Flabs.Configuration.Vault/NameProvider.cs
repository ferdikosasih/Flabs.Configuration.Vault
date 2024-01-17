using System;
using System.Reflection;

namespace Flabs.Configuration.VaultSharp.Extensions
{
    internal class NameProvider : INameProvider
    {
        private readonly string _name;
        public string Name { get { return _name; } }
        public NameProvider()
        {
            string? name = Assembly.GetEntryAssembly()?.GetName().Name;
            if (name == null)
            {
                throw new InvalidOperationException("Unable to get service name");
            }
            _name = name;
        }
    }
}
