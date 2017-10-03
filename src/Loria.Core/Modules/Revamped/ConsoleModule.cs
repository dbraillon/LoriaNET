using System;

namespace LoriaNET
{
    /// <summary>
    /// The console module provides a listener and a callback to interact with a console.
    /// </summary>
    /// <remarks>
    /// Updated for version 1.1.0.
    /// </remarks>
    public class ConsoleModule : Listener, ICallback, IModule
    {
        public override string Name => "Console module";
        
        public ConsoleModule(Loria loria)
            : base(loria, 1)
        {
        }
        
        public override void Configure()
        {
            // No configuration needed, always activated
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
