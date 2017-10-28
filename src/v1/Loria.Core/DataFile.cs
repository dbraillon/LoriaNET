using System.Configuration;

namespace Loria.Core
{
    public class DataFile
    {
        public void Set(string key, object value) => ConfigurationManager.AppSettings.Set(key, value.ToString());
        public string Get(string key) => ConfigurationManager.AppSettings.Get(key);
    }
}
