using System;
using System.Globalization;
using System.Linq;

namespace Loria.Core
{
    public class Data
    {
        public Engine Engine { get; set; }
        public DataFile File { get; set; }

        public CultureInfo Culture
        {
            get => _Culture;
            set
            {
                _Culture = value;

                // Persist culture to configuration file
                File.Set("core::Culture", value.Name);

                // Change current culture for resource manager to handle resource file
                CultureInfo.CurrentCulture = value;
                CultureInfo.CurrentUICulture = value;
                CultureInfo.DefaultThreadCurrentCulture = value;
                CultureInfo.DefaultThreadCurrentUICulture = value;

                // Propagate the culture change
                CultureChanged?.Invoke(value);
            }
        }
        private CultureInfo _Culture { get; set; }
        public event Action<CultureInfo> CultureChanged;

        public Data(Engine engine)
        {
            Engine = engine;
            File = new DataFile();

            // Try to load culture from configuration file
            var supportedCultures = CultureInfo.GetCultures(CultureTypes.AllCultures);
            var storedCultureStr = File.Get("core::Culture");
            var storedCulture = supportedCultures.FirstOrDefault(c => string.Equals(c.Name, storedCultureStr, StringComparison.InvariantCultureIgnoreCase));

            // Set culture to stored or default
            Culture = storedCulture ?? CultureInfo.GetCultureInfo("en-US");
        }
    }
}
