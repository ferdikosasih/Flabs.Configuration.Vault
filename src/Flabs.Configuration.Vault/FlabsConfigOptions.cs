namespace Flabs.Configuration.VaultSharp.Extensions
{

    public class FlabsConfigOptions
    {
        public string VaultAddress { get; set; }
        public string? VaultToken { get; set; }
        public string VaultMountPoint { get; set; }
        public bool IsNeedReload {  get; set; }
        public int ReloadTimeMinute { get; set; }

        public FlabsConfigOptions(
            string? vaultToken
            , string vaultAddress
            , string mountPoint = "flabs.kv"
            , bool isNeedReload = true
            , int reloadTimeMinute = 60)
        {
            VaultToken = vaultToken;
            VaultAddress = vaultAddress;
            VaultMountPoint = mountPoint;
            IsNeedReload = isNeedReload;
            ReloadTimeMinute = reloadTimeMinute;
        }
    }
}
