using System.Linq;

namespace LoriaNET
{
    /// <summary>
    /// The console module provides a listener and a callback to interact with a console.
    /// </summary>
    /// <remarks>
    /// Updated for version 1.1.0.
    /// </remarks>
    public class ConfigurationModule : Module, IAction
    {
        const string SetIntent = "set";
        const string GetIntent = "get";

        public override string Name => "Configuration module";
        public string Description => "Get or set a property value from configuration file";

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
            // No configuration needed, always activated
            Activate();
        }

        public void Perform(Command command)
        {
            var splitted = command.Raw.Split(' ');
            var key = splitted.Skip(2).FirstOrDefault();

            switch (command.Intent.ToLowerInvariant())
            {
                case GetIntent:
                    var value = Loria.Data.ConfigurationFile.Get(key);
                    Loria.Hub.PropagateCallback(value);
                    break;

                case SetIntent:
                    var newValue = string.Join(" ", splitted.Skip(3));
                    Loria.Data.ConfigurationFile.Set(key, newValue);
                    break;

                default:
                    break;
            }
        }
    }
}
