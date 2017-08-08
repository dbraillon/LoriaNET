using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Loria.Core
{
    public class Actions
    {
        public List<IAction> All { get; }

        public Actions(List<IAction> actions)
        {
            All = actions;
        }

        public IAction Get(string name) => All.FirstOrDefault(l => string.Equals(l.Name, name, StringComparison.InvariantCultureIgnoreCase));
        public IAction GetByCommand(string command) => All.FirstOrDefault(l => string.Equals(l.Command, command, StringComparison.InvariantCultureIgnoreCase));

        public void Perform(string name, string[] args) => Get(name)?.Perform(args);
        public void PerformAll(string[] args) => All.ForEach(l => l.Perform(args));
    }
}
