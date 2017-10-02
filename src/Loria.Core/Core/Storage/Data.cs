using LoriaNET.Storage.Database;
using LoriaNET.Storage.File;
using System;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Linq;

namespace LoriaNET.Storage
{
    public class Data
    {
        public ConfigurationFile ConfigurationFile { get; set; }

        /// <summary>
        /// Current Loria's culture.
        /// </summary>
        public CultureInfo Culture
        {
            get => _Culture;
            set
            {
                // Persist culture to configuration file
                ConfigurationFile.Set(ConfigurationFile.CultureKey, value.Name);

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

        /// <summary>
        /// An event fired when Loria's culture has changed.
        /// </summary>
        public event Action<CultureInfo> CultureChanged;

        /// <summary>
        /// Loria's manager.
        /// </summary>
        public PersonEntity Manager { get; set; }

        /// <summary>
        /// Loria's contacts.
        /// </summary>
        public ObservableCollection<PersonEntity> Contacts { get; set; } = new ObservableCollection<PersonEntity>();

        /// <summary>
        /// Get a Loria's contact by its first name.
        /// </summary>
        public PersonEntity GetPerson(string firstName)
        {
            if (Manager.Is(firstName)) return Manager;
            return Contacts.FirstOrDefault(c => c.Is(firstName));
        }



        public Data()
        {
            ConfigurationFile = new ConfigurationFile();

            // Try to load culture from configuration file
            var supportedCultures = CultureInfo.GetCultures(CultureTypes.AllCultures);
            var storedCultureStr = ConfigurationFile.Get(ConfigurationFile.CultureKey);
            var storedCulture = supportedCultures.FirstOrDefault(c => string.Equals(c.Name, storedCultureStr, StringComparison.InvariantCultureIgnoreCase));

            // Set culture to stored or default
            Culture = storedCulture ?? CultureInfo.GetCultureInfo("en-US");

            // Set manager
            Manager = new PersonEntity() { Name = "Davy" };
        }
    }
}
