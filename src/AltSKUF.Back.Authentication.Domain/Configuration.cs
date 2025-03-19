namespace AltSKUF.Back.Authentication.Domain
{
    public class Configuration
    {
        public static Configuration Singleton { get; set; } = new();

        public string AccesExpirationTimeInMinutes { get; set; } = string.Empty;
        public string RefreshExpirationTimeInMinutes { get; set; } = string.Empty;

        public string ServicesSercret { get; set; } = string.Empty;
    }
}
