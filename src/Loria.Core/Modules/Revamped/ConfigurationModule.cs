using System.Configuration;
using System.Linq;

namespace LoriaNET
{
    public sealed class ConfigurationModule : Module, IAction
    {
        const string SetIntent = "set";
        const string GetIntent = "get";

        public override string Name => "Configuration module";
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
            "conf set spotify::APIKey ABCDEF",
            "conf get spotify::APIKey"
        };
        
        public ConfigurationModule(Loria loria)
            : base (loria)
        {
        }

        public override void Configure()
        {
            Activate();
        }

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
                        Loria.Hub.PropagateCallback(value);
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
