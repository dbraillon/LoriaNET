using System;
using System.Collections.Generic;
using System.Configuration;
using System.Globalization;
using System.Linq;

namespace Loria.Core
{
    public class Configuration
    {
        public CultureInfo DefaultCulture => CultureInfo.GetCultureInfo("en-US");
        public CultureInfo Culture
        {
            get => CultureInfo.GetCultureInfo(ConfigurationManager.AppSettings.Get("core::Culture"));
            set => ConfigurationManager.AppSettings.Set("core::Culture", value.Name);
        }

        public Dictionary<string, string> Configs { get; set; }

        public Modules Modules { get; set; }
        public Actions Actions { get; }
        public Callbacks Callbacks { get; }
        public Listeners Listeners { get; }

        public Configuration()
        {
            Culture = CultureInfo.CurrentCulture;
            Configs = ConfigurationManager.AppSettings.AllKeys.ToDictionary(k => k, k => ConfigurationManager.AppSettings[k]);

            // Configure all modules
            Modules = new Modules(this);
            Modules.ConfigureAll();

            // Initialize actions, callbacks and listeners
            Actions = new Actions(Modules.All.Where(c => typeof(IAction).IsAssignableFrom(c.GetType()) && c.IsEnabled()).OfType<IAction>().ToList());
            Callbacks = new Callbacks(Modules.All.Where(c => typeof(ICallback).IsAssignableFrom(c.GetType()) && c.IsEnabled()).OfType<ICallback>().ToList());
            Listeners = new Listeners(Modules.All.Where(c => typeof(IListener).IsAssignableFrom(c.GetType()) && c.IsEnabled()).OfType<IListener>().ToList());
        }

        public void Hub(string command)
        {
            if (string.IsNullOrEmpty(command)) return;

            var commandSplitted = command.Split(' ');
            var direction = commandSplitted.ElementAtOrDefault(0);

            if (string.Equals(direction, "callback", StringComparison.InvariantCultureIgnoreCase))
            {
                var optionnalName = commandSplitted.ElementAtOrDefault(1);
                var callback = Callbacks.Get(optionnalName);

                if (callback == null)
                {
                    Callbacks.CallbackAll(string.Join(" ", commandSplitted.Skip(1).ToArray()));
                }
                else
                {
                    callback.Callback(string.Join(" ", commandSplitted));
                }
            }
            else
            {
                var action = Actions.GetByCommand(direction);
                if (action != null)
                {
                    action.Perform(commandSplitted.Skip(1).ToArray());
                }
                else
                {
                    Callbacks.CallbackAll($"Command '{direction}' does not exist.");
                }
            }
        }
    }
}
