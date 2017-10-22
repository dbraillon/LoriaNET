using System;
using System.Collections.Generic;
using System.Linq;

namespace Loria.Core
{
    public class CommandSet<THasCommand> : Set<THasCommand>
        where THasCommand : IHasCommand
    {
        public CommandSet(params THasCommand[] objects) : base(objects)
        {
        }
        public CommandSet(IEnumerable<THasCommand> objects) : base(objects)
        {
        }

        public THasCommand GetByCommand(string command)
        {
            return Objects.FirstOrDefault(l => string.Equals(l.Command, command, StringComparison.InvariantCultureIgnoreCase));
        }

        public void Perform(Command command)
        {
            var handleCommand = GetByCommand(command.Module);
            if (handleCommand != null)
            {
                handleCommand.Perform(command);
            }
            else
            {
                // Don't do anything
            }
        }
    }
}
