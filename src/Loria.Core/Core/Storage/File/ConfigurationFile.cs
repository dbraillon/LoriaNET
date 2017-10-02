using System.Configuration;
using System.Globalization;
using System.Linq;

namespace LoriaNET.Storage.File
{
    /// <summary>
    /// This is where Loria stores it's basic configuration.
    /// </summary>
    public class ConfigurationFile
    {
        public string CultureKey => "core::Culture";

        /// <summary>
        /// Write a key/value pair in the configuration file.
        /// </summary>
        public void Set(string key, object value) => ConfigurationManager.AppSettings.Set(key, value.ToString());

        /// <summary>
        /// Read a value from a given key in the configuration file.
        /// </summary>
        public string Get(string key) => ConfigurationManager.AppSettings.Get(key);
    }
}
