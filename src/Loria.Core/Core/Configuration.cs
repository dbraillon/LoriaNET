using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Configuration;
using System.Globalization;
using System.Linq;
using System.Resources;
using System.Threading;

namespace LoriaNET
{
    /// <summary>
    /// Loria's configuration object.
    /// </summary>
    public sealed class Configuration
    {
        /// <summary>
        /// Current Loria's culture.
        /// </summary>
        public CultureInfo Culture
        {
            get => CultureInfo.GetCultureInfo(Get("core::Culture"));
            set
            {
                // Persist culture to configuration file
                Set("core::Culture", value.Name);

                // Change current culture for resource manager to handle resource file
                CultureInfo.CurrentCulture = value;
                CultureInfo.CurrentUICulture = value;
                CultureInfo.DefaultThreadCurrentCulture = value;
                CultureInfo.DefaultThreadCurrentUICulture = value;

                // Propagate the culture change
                CultureChanged?.Invoke(value);
            }
        }

        /// <summary>
        /// An event fired when Loria's culture has changed.
        /// </summary>
        internal event Action<CultureInfo> CultureChanged;
        
        /// <summary>
        /// Every modules loaded by Loria.
        /// </summary>
        internal Modules Modules { get; set; }

        /// <summary>
        /// Every actions loaded by Loria.
        /// </summary>
        internal Actions Actions { get; }

        /// <summary>
        /// Every callbacks loaded by Loria.
        /// </summary>
        internal Callbacks Callbacks { get; }

        /// <summary>
        /// Every listeners loaded by Loria.
        /// </summary>
        internal Listeners Listeners { get; }

        /// <summary>
        /// Loria's hub, used to propagate commands or callbacks.
        /// </summary>
        internal Hub Hub { get; }

        /// <summary>
        /// Loria's manager.
        /// </summary>
        internal Person Manager { get; set; }

        /// <summary>
        /// Loria's contacts.
        /// </summary>
        internal ObservableCollection<Person> Contacts { get; set; } = new ObservableCollection<Person>();

        /// <summary>
        /// Get a Loria's contact by its first name.
        /// </summary>
        internal Person GetPerson(string firstName)
        {
            if (Manager.Is(firstName)) return Manager;
            return Contacts.FirstOrDefault(c => c.Is(firstName));
        }

        /// <summary>
        /// Create a basic configuration.
        /// </summary>
        public Configuration() : this(CultureInfo.CurrentUICulture) { }

        /// <summary>
        /// Create an advanced configuration.
        /// </summary>
        public Configuration(CultureInfo culture)
        {
            // Set culture
            Culture = culture;

            // Set manager
            Manager = new Person() { FirstName = "Davy" };

            // Configure all modules
            Modules = new Modules(this);
            Modules.ConfigureAll();

            // Initialize actions, callbacks and listeners
            Actions = new Actions(Modules.GetAll<IAction>());
            Callbacks = new Callbacks(Modules.GetAll<ICallback>());
            Listeners = new Listeners(Modules.GetAll<IListener>());

            // Prepare the hub
            Hub = new Hub(this);
        }
        
        /// <summary>
        /// Write a key/value pair in the configuration file.
        /// </summary>
        /// <param name="key">A key.</param>
        /// <param name="value">A value.</param>
        internal void Set(string key, object value) => ConfigurationManager.AppSettings.Set(key, value.ToString());

        /// <summary>
        /// Read a value from a given key in the configuration file.
        /// </summary>
        /// <param name="key">A key.</param>
        /// <returns>A value.</returns>
        internal string Get(string key) => ConfigurationManager.AppSettings.Get(key);
    }
}
