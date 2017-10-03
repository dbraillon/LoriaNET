using Roggle.Core;

namespace LoriaNET
{
    /// <summary>
    /// A module to log every callbacks in a file.
    /// </summary>
    /// <remarks>
    /// Updated for version 1.1.0.
    /// </remarks>
    public class LogModule : Module, ICallback
    {
        public override string Name => "Log module";
        
        public LogModule(Loria loria) 
            : base(loria)
        {
        }

        public override void Configure()
        {
            // Create the log file
            GRoggle.Use(new FileRoggle(@"logs\callbacks.log", acceptedLogLevels: RoggleLogLevel.Debug | RoggleLogLevel.Error));

            Activate();
        }

        public void Callback(string message) => GRoggle.Write(message, level: RoggleLogLevel.Debug);
    }
}
