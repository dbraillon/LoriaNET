using System;
using System.Linq;
using System.Threading.Tasks;

namespace LoriaNET
{
    public class HelpModule : IModule, IAction
    {
        public string Name => "Help module";
        public string Description => "Give you tips on command";

        public string Command => "help";
        public string[] SupportedIntents => new string[]
        {
        };
        public string[] SupportedEntities => new string[]
        {
        };
        public string[] Samples => new string[]
        {
            "Help voice module",
            "Give me help for reminder module",
            "How http module works"
        };

        public Configuration Configuration { get; set; }

        public HelpModule(Configuration configuration)
        {
            Configuration = configuration;
        }

        public void Activate()
        {
            // Help module is always activated
        }

        public void Configure()
        {
            // Nothing to configure
        }

        public void Deactivate()
        {
            // Help module is always activated
        }

        public bool IsEnabled() => true;

        public void Perform(Command command)
        {
            var module = command.Intent;
            var relatedAction = Configuration.Actions.GetByCommand(module);

            if (relatedAction != null)
            {
                Configuration.Hub.PropagateCallback($"{relatedAction.Description}{Environment.NewLine}{string.Join(Environment.NewLine, relatedAction.Samples)}");
            }
        }
    }
}
