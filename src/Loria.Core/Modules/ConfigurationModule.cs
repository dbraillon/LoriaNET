using System.Configuration;
using System.Linq;

namespace LoriaNET
{
    internal sealed class ConfigurationModule : IModule, IAction
    {
        const string SetIntent = "set";
        const string GetIntent = "get";

        public string Name => "Configuration module";
        public string Description => "Get or set a property value of configuration object";

        public string Command => "conf";
        public string[] SupportedIntents => new string[]
        {
            SetIntent, GetIntent
        };
        public string[] SupportedEntities => new string[]
        {
        };
        public string[] Samples => new string[]
        {
            "Set culture to french",
            "Get culture",
            "Get voice api key"
        };

        public Configuration Configuration { get; set; }

        public ConfigurationModule(Configuration configuration)
        {
            Configuration = configuration;
        }

        public void Activate()
        {
        }

        public void Deactivate()
        {
        }

        public void Configure()
        {
        }

        public bool IsEnabled() => true;

        public void Perform(Command command)
        {
            var splitted = command.Raw.Split(' ');
            var key = splitted.Skip(2).FirstOrDefault();

            switch (command.Intent.ToLowerInvariant())
            {
                case GetIntent:

                    if (ConfigurationManager.AppSettings.AllKeys.Contains(key))
                    {
                        var value = ConfigurationManager.AppSettings.Get(key);
                        Configuration.Hub.PropagateCallback(value);
                    }
                    break;

                case SetIntent:
                    
                    if (ConfigurationManager.AppSettings.AllKeys.Contains(key))
                    {
                        var newValue = string.Join(" ", splitted.Skip(3));
                        ConfigurationManager.AppSettings.Set(key, newValue);
                    }
                    break;

                default:
                    break;
            }
        }
    }
}
