using Microsoft.Extensions.Configuration;
using System;

namespace Schedent.Common
{
    public static class Settings
    {
        private static IConfiguration _configuration;

        /// <summary>
        /// Setter for the configuration
        /// </summary>
        /// <param name="configuration"></param>
        public static void SetConfig(IConfiguration configuration)
        {
            if (_configuration == null)
            {
                _configuration = configuration;
            }
        }

        /// <summary>
        /// Get the version from the appsettings file
        /// </summary>
        public static string Version
        {
            get
            {
                var version = _configuration["AppSettings:Version"];

                if (!string.IsNullOrEmpty(version))
                {
                    return version;
                }

                return "Version not set";
            }
        }

        /// <summary>
        /// Get the connection string from the appsettings file
        /// </summary>
        public static string DatabaseConnectionString
        {
            get
            {
                var connectionString = _configuration.GetConnectionString("DatabaseConnectionString");

                if (!string.IsNullOrEmpty(connectionString))
                {
                    return connectionString;
                }

                throw new InvalidOperationException("Invalid configuration value for database connectiong string");
            }
        }

        /// <summary>
        /// Get the token secret from the appsettings file
        /// </summary>
        public static string TokenSecret
        {
            get
            {
                var tokenSecret = _configuration["AppSettings:TokenSecret"];

                if (!string.IsNullOrEmpty(tokenSecret))
                {
                    return tokenSecret;
                }

                throw new InvalidOperationException("Invalid configuration value for JWT Token Secret");
            }
        }

        /// <summary>
        /// Convert the token secret to byte array
        /// </summary>
        public static byte[] TokenSecretBytes
        {
            get
            {
                return System.Text.Encoding.ASCII.GetBytes(TokenSecret);
            }
        }

        /// <summary>
        /// Get the api url from the appsettings  file
        /// </summary>
        public static Uri ApiUrl
        {
            get
            {
                var urlString = _configuration["AppSettings:ApiUrl"];

                if (!string.IsNullOrEmpty(urlString))
                {
                    return new Uri(urlString);
                }

                throw new InvalidOperationException("Invalid configuration value for API url");
            }
        }
    }
}
