using System;

namespace LoriaNET
{
    public class HelpModule : Module, IAction
    {
        public override string Name => "Help module";
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
            "help spotify"
        };

        public HelpModule(Configuration configuration)
            : base(configuration)
        {
        }

        public override void Configure()
        {
            Activate();
        }

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
