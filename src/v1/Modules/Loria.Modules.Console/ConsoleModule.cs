using Loria.Core;
using System.Threading;

namespace Loria.Modules.Console
{
    public class ConsoleModule : Listener, ICallback
    {
        public override string Name => "Console module";
        public override string Description => "It allows me to see what you type in console and to answer you there";

        public string Command => "console";

        public ConsoleModule(Engine engine) 
            : base(engine, 1)
        {
        }

        public override void Configure()
        {
            // Nothing to configure yet
            Activate();
        }

        public void Perform(Command command)
        {
            System.Console.WriteLine(command.AsCallbackCommand().Message);
        }

        protected override Command Listen()
        {
            try
            {
                var input = System.Console.ReadLine();
                if (string.IsNullOrWhiteSpace(input)) return null;

                return new Command(input);
            }
            catch (ThreadInterruptedException)
            {
                return null;
            }
        }
    }
}
