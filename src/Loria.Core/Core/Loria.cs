using LoriaNET.Storage;
using System.Threading;

namespace LoriaNET
{
    public class Loria
    {
        public bool IsLiving { get; private set; }
        
        public Data Data { get; }
        public Modules Modules { get; set; }
        public Actions Actions { get; }
        public Callbacks Callbacks { get; }
        public Listeners Listeners { get; }

        public ILog Log { get; set; }
        
        public Loria()
        {
            // Retrieve data
            Data = new Data();

            // Configure all modules
            Modules = new Modules();
            Modules.ConfigureAll();

            // Initialize actions, callbacks and listeners
            Actions = new Actions(Modules.GetAll<IAction>());
            Callbacks = new Callbacks(Modules.GetAll<ICallback>());
            Listeners = new Listeners(Modules.GetAll<IListener>());
            Listeners.OnEvent += (sender, e) => PropagateCommand(e.Command);
        }
        
        public void Live()
        {
            // Fallback to LiveAsync
            LiveAsync();

            // Block current thread
            while (IsLiving) Thread.Sleep(1000);
        }
        public void LiveAsync()
        {
            // Turn the flag on
            IsLiving = true;

            // Start enabled listeners
            Listeners.StartAll();
        }
        
        public void Stop()
        {
            // Stop listeners
            Listeners.StopAll();

            // Everything have been stopped
            IsLiving = false;
        }

        public void PropagateCommand(Command command)
        {
            if (command.IsActionRelated)
                Actions.Perform(command);

            if (command.IsCallbackRelated)
                Callbacks.Perform(command);
        }
    }
}
