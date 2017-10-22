using System;
using System.Collections.Generic;
using System.Linq;

namespace LoriaNET
{
    public class HandleCommandSet<THandleCommand> : Set<THandleCommand>
        where THandleCommand : IHandleCommand
    {
        public HandleCommandSet(params THandleCommand[] objects) : base(objects)
        {
        }
        public HandleCommandSet(IEnumerable<THandleCommand> objects) : base(objects)
        {
        }

        public THandleCommand GetByCommand(string command)
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
