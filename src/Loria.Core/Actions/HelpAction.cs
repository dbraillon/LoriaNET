using System;
using System.Linq;
using System.Threading.Tasks;

namespace Loria.Core.Actions
{
    public class HelpAction : IAction
    {
        public string Command => "help";
        public string Description => "Give you tips on commands";
        public string Usage => "help COMMAND";
        
        public Configuration Configuration { get; set; }

        public HelpAction(Configuration configuration)
        {
            Configuration = configuration;
        }

        public async Task PerformAsync(string[] args)
        {
            await Task.Delay(0);

            var command = args.FirstOrDefault();
            var actions = ActionFactory.GetAllActions(Configuration);
            var relatedAction = actions.FirstOrDefault(a => a.Command == command);

            if (relatedAction != null)
            {
                Console.WriteLine();
                Console.WriteLine(relatedAction.Description);
                Console.WriteLine();
                Console.WriteLine($"Usage: {relatedAction.Usage}");
                Console.WriteLine();
            }
            else
            {
                Console.WriteLine();
                Console.WriteLine("Commands:");

                foreach (var action in actions)
                {
                    Console.WriteLine($" {action.Command}\t{action.Description}");
                }

                Console.WriteLine();
            }
        }
    }
}
