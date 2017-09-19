using System;

namespace LoriaNET
{
    /// <summary>
    /// The console module provides a listener and a callback to interact with a console.
    /// </summary>
    internal sealed class ConsoleModule : Listener, ICallback, IModule
    {
        public override string Name => "Console module";
        
        public ConsoleModule(Configuration configuration)
            : base(configuration, 1)
        {
        }
        
        public override void Configure()
        {
            Activate();
        }

        public override string Listen()
        {
            return Console.ReadLine();
        }

        public void Callback(string message)
        {
            Console.WriteLine(message);
        }
    }
}
