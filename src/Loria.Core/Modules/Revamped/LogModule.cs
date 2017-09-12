using Roggle.Core;

namespace LoriaNET
{
    /// <summary>
    /// A module to log every callbacks in a file.
    /// </summary>
    internal sealed class LogModule : Module, ICallback
    {
        public override string Name => "Log module";
        
        public LogModule(Configuration configuration) 
            : base(configuration)
        {
        }

        public override void Configure()
        {
            GRoggle.Use(new FileRoggle(@"logs\callbacks.log", acceptedLogLevels: RoggleLogLevel.Debug | RoggleLogLevel.Error));
            Activate();
        }

        public void Callback(string message) => GRoggle.Write(message, level: RoggleLogLevel.Debug);
    }
}
