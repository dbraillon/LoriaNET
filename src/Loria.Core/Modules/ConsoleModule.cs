using System;

namespace Loria.Core
{
    public class ConsoleModule : Listener, IModule, ICallback
    {
        public override string Name => "Console module";

        public ConsoleModule(Configuration configuration) 
            : base(configuration, 1)
        {
        }

        public void Callback(string message)
        {
            Console.WriteLine(message);
        }

        public override string Listen()
        {
            return Console.ReadLine();
        }

        public void Activate()
        {
            // Console module is always activated
        }

        public void Configure()
        {
            // Nothing to configure
        }

        public void Deactivate()
        {
            // Console module is always activated
        }

        public bool IsEnabled() => true;
    }
}
