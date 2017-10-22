using Loria.Core;

namespace Loria.Modules.Broadcast
{
    public class BroadcastModule : Module, ICallback
    {
        public override string Name => "Broadcast module";
        public override string Description => "It allows me to broadcast a message to every callback I have";

        public string Command => "broadcast";

        public BroadcastModule(Engine engine) 
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
            foreach (var callback in Engine.Callbacks.Objects)
            {
                if (callback != this)
                {
                    callback.Perform(command);
                }
            }
        }
    }
}
