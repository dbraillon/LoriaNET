using System;

namespace LoriaNET
{
    /// <summary>
    /// The console module provides a listener and a callback to interact with a console.
    /// </summary>
    internal sealed class ConsoleModule : Listener, ICallback, IModule
    {
        public override string Name => "Console module";

        /// <summary>
        /// Create the console module.
        /// </summary>
        /// <param name="configuration">Loria's configuration.</param>
        public ConsoleModule(Configuration configuration)
            : base(configuration, 1)
        {
        }

        /// <summary>
        /// Configure the console module.
        /// </summary>
        public void Configure()
        {
            // Nothing to configure
        }

        /// <summary>
        /// Check if console module is enabled.
        /// </summary>
        /// <returns>State of console module.</returns>
        public bool IsEnabled() => true;

        /// <summary>
        /// Activate the console module.
        /// </summary>
        public void Activate()
        {
            // Console module is always activated
        }

        /// <summary>
        /// Deactivate the console module.
        /// </summary>
        public void Deactivate()
        {
            // Console module is always activated
        }

        /// <summary>
        /// Will propagate what the user writes inside the console.
        /// </summary>
        /// <returns>A command or a message.</returns>
        public override string Listen()
        {
            return Console.ReadLine();
        }

        /// <summary>
        /// Will display the message inside the console to the user.
        /// </summary>
        /// <param name="message">A message to display.</param>
        public void Callback(string message)
        {
            Console.WriteLine(message);
        }
    }
}
