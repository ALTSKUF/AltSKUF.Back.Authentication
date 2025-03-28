﻿using System.Text.Json;

namespace AltSKUF.Back.Authentication.Domain
{
    public class Configuration
    {
        public static Configuration Singleton { get; set; } = new();

        public string AccessExpirationTimeInMinutes { get; set; } = string.Empty;
        public string RefreshExpirationTimeInMinutes { get; set; } = string.Empty;

        public string ServicesSercret { get; set; } = string.Empty;
        public string ServicesExpirationTimeInMinutes { get; set; } = string.Empty;

        public string UserServiceUrl { get; set;} = string.Empty;
    }
}
