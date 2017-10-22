using Loria.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Loria.Modules.Help
{
    public class HelpModule : Module, IAction
    {
        public override string Name => "Help module";
        public override string Description => "It allows me to describe what I can do for you";

        public string Command => "help";
        public string[] SupportedIntents => new string[] { };
        public string[] SupportedEntities => new string[] { };
        public string[] Samples => new string[] 
        {
            "help",
            "help spotify",
            "help reminder"
        };

        public HelpModule(Engine engine)
            : base(engine)
        {
        }

        public override void Configure()
        {
            // Nothing to configure yet
            Activate();
        }

        public void Perform(Command command)
        {
            var actionCommand = command.AsActionCommand();
            var needHelpCommand = actionCommand.Intent;

            var action = Engine.Actions.GetByCommand(needHelpCommand);
            var callback = Engine.Callbacks.GetByCommand(needHelpCommand);

            var description = string.Empty;

            if (action != null)
                description = DescribeAction(action);

            else if (callback != null)
                description = DescribeCallback(callback);

            if (!string.IsNullOrEmpty(description))
                Engine.Propagator.PropagateCallback(new Command("console", description));
        }

        public string DescribeAction(IAction action)
        {
            var sb = new StringBuilder();

            // Name and description
            sb.Append($"{action.Name} - {action.Description}. ");

            // Intents
            foreach (var intent in action.SupportedIntents)
            {
                sb.Append($"{intent} - ...");
            }

            // Entities
            foreach (var entity in action.SupportedEntities)
            {
                sb.Append($"{entity} - ...");
            }

            return sb.ToString();
        }

        public string DescribeCallback(ICallback callback)
        {
            return $"{callback.Name} - {callback.Description}.";
        }
    }
}
