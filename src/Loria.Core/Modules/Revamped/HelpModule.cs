using System.Linq;
using System.Text;

namespace LoriaNET
{
    /// <summary>
    /// Help module let user call for assistance about Loria's capabilities.
    /// </summary>
    /// <remarks>
    /// Updated for version 1.1.0.
    /// </remarks>
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
            "help",
            "help spotify",
            "help reminder"
        };

        public HelpModule(Loria loria)
            : base(loria)
        {
        }

        public override void Configure()
        {
            // No configuration needed, always activated
            Activate();
        }

        public void Perform(Command command)
        {
            var sb = new StringBuilder();

            // Intent for help module is module that need help
            var module = command.Intent;

            if (string.IsNullOrEmpty(module))
            {
                // No module given, display all available commands with its description
                sb.AppendLine();
                sb.AppendLine("Usage: loria COMMAND");
                sb.AppendLine();
                sb.AppendLine("More than an assistant");
                sb.AppendLine();
                sb.AppendLine("Commands:");

                foreach (var action in Loria.Actions.Set.OrderBy(a => a.Command))
                {
                    sb.AppendLine($"  {action.Command.PadRight(10, ' ')}\t{action.Description}");
                }
            }
            else
            {
                var relatedAction = Loria.Actions.GetByCommand(module);
                if (relatedAction != null)
                {
                    // Valid module given, display its description and samples
                    sb.AppendLine();
                    sb.AppendLine($"Usage: loria {relatedAction.Command} INTENT [ENTITIES]");
                    sb.AppendLine();
                    sb.AppendLine(relatedAction.Description);

                    if (relatedAction.SupportedIntents.Any())
                    {
                        sb.AppendLine();
                        sb.AppendLine("Intents:");

                        foreach (var intent in relatedAction.SupportedIntents.OrderBy(i => i))
                        {
                            sb.AppendLine($"  {intent.PadRight(10, ' ')}\t[NO DESCRIPTION YET AVAILABLE]");
                        }
                    }
                    
                    if (relatedAction.SupportedEntities.Any())
                    {
                        sb.AppendLine();
                        sb.AppendLine("Entities:");

                        foreach (var entity in relatedAction.SupportedEntities.OrderBy(e => e))
                        {
                            sb.AppendLine($"  {entity.PadRight(10, ' ')}\t[NO DESCRIPTION YET AVAILABLE]");
                        }
                    }
                    
                    if (relatedAction.Samples.Any())
                    {
                        sb.AppendLine();
                        sb.AppendLine("Samples:");

                        foreach (var sample in relatedAction.Samples.OrderBy(s => s))
                        {
                            sb.AppendLine($"  {sample}");
                        }
                    }
                }
                else
                {
                    // Invalid module given, display error message
                    sb.AppendLine($"'{module}' is not a loria command");
                    sb.AppendLine("See 'loria help'");
                }
            }

            Loria.Hub.PropagateCallback(sb.ToString());
        }
    }
}
