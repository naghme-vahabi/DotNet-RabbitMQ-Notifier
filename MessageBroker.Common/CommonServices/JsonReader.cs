using MessageBroker.Common.Enums;
using Microsoft.Extensions.Configuration;
using System.Configuration;

namespace MessageBroker.Common.CommonServices
{
    public static class JsonReader
    {
        private static string _baseDirectory = AppDomain.CurrentDomain.BaseDirectory;

        /// <summary>
        /// Get Configuration Based on Parent And Child Section Name From Specific JsonFile
        /// </summary>
        /// <param name="_jsonFileName">JsonFile Name</param>
        /// <param name="_JsonType">Type Of Section (ConnectionString Or Other)</param>
        /// <param name="_parentName">Parent Section Name</param>
        /// <param name="_childName">Child Section Name</param>
        /// <returns></returns>
        /// <exception cref="ConfigurationErrorsException"></exception>
        public static string GetConfigs(string _jsonFileName, JsonType _JsonType, string _parentName, string? _childName = "")
        {
            try
            {
                var config = new ConfigurationBuilder()
                            .SetBasePath(_baseDirectory)
                            .AddJsonFile(_jsonFileName + ".json")
                            .Build();
                if (_JsonType == JsonType.ConnectionString)
                {
                    return config?.GetConnectionString(_parentName) ?? string.Empty;
                }
                else if (_JsonType == JsonType.Others && !string.IsNullOrEmpty(_childName))
                {
                    return config?.GetSection(_parentName)?.GetSection(_childName)?.Value ?? string.Empty;
                }
                return string.Empty;
            }
            catch (Exception ex)
            {
                throw new ConfigurationErrorsException($"Error Reading Configurtion {ex.Message}", ex);
            }
        }
        /// <summary>
        /// Get Configuration Based on Class From Specific JsonFile
        /// </summary>
        /// <typeparam name="TEntity">Entity</typeparam>
        /// <param name="_jsonFileName">JsonFile Name</param>
        /// <param name="_JsonType">Type Of Section (Entity)</param>
        /// <returns></returns>
        /// <exception cref="ArgumentException">If the Entity Class Not Exist</exception>
        /// <exception cref="ConfigurationErrorsException">If Configuration Doesn't Match To Entity Class</exception>

        public static TEntity GetConfigs<TEntity>(string _jsonFileName, JsonType _JsonType = JsonType.Entity)
        {
            try
            {
                string className = typeof(TEntity).Name;
                if (!string.IsNullOrEmpty(className))
                {
                    throw new ArgumentException("Invalid Entity Type");
                }
                TEntity obj = Activator.CreateInstance<TEntity>();
                var config = new ConfigurationBuilder().SetBasePath(_baseDirectory).AddJsonFile(_jsonFileName + ".json").Build();
                if (className is not null)
                {
                    config.GetSection(className).Bind(obj);
                }
                return obj;
            }
            catch (Exception ex)
            {
                throw new ConfigurationErrorsException($"Error Reading Configurtion {ex.Message}", ex);
            }
        }

    }
}
