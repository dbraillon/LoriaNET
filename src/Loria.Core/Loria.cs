using Loria.Core.Actions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Loria.Core
{
    public class Loria
    {
        public Configuration Configuration { get; set; }

        public IEnumerable<IAction> Actions { get; set; }

        public Loria(Configuration configuration)
        {
            Configuration = configuration;

            Actions = ActionFactory.GetAllActions(Configuration);
        }

        public async Task HandleCommandAsync(string command, string[] args)
        {
            if (string.IsNullOrWhiteSpace(command)) return;

            var relatedAction = Actions.FirstOrDefault(a => a.Command == command);
            if (relatedAction == null) throw new ApplicationException($"'{command}' is not a Loria command, see command 'help'.");

            await relatedAction.PerformAsync(args);
        }
    }
}
