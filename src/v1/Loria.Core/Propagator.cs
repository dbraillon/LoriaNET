using System.Diagnostics;

namespace Loria.Core
{
    public class Propagator : IPropagator
    {
        public Engine Engine { get; set; }

        public Propagator(Engine engine)
        {
            Engine = engine;
        }

        public void Propagate(Command command)
        {
            if (command == null) return;

            Debug.WriteLine($"Propagate command {command}");

            if (command.IsActionRelated)
                PropagateAction(command);

            if (command.IsCallbackRelated)
                PropagateCallback(command);
        }

        public void PropagateAction(Command command) => Engine.Actions.Perform(command);
        public void PropagateCallback(Command command) => Engine.Callbacks.Perform(command);
    }
}
