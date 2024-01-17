namespace Flabs.Configuration.VaultSharp.Extensions
{

    public class VaultOptions
    {
        public string VaultAddress { get; set; }
        public string? VaultToken { get; set; }
        public string VaultMountPoint { get; set; }

        public VaultOptions(string? vaultToken, string vaultAddress, string mountPoint = "flabs.kv")
        {
            VaultToken = vaultToken;
            VaultAddress = vaultAddress;
            VaultMountPoint = mountPoint;
        }
    }
}
