namespace SecuLink.RequestModels
{
    public class DeviceAuthForm
    {
        public string MAC { get; set; }
        public string MAC_enc { get; set; }
        public string Key { get; set; }
        public bool Role { get; set; }
    }
}
