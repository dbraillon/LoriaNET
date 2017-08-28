using System;

namespace LoriaNET
{
    /// <summary>
    /// The date time module provides intents and entities for giving the date and time.
    /// </summary>
    internal sealed class DateTimeModule : IModule, IAction
    {
        const string DateIntent = "date";
        const string TimeIntent = "time";

        public string Name => "Date time module";
        public string Description => "Give you the current date";

        public string Command => "datetime";
        public string[] SupportedIntents => new string[]
        {
            DateIntent, TimeIntent
        };
        public string[] SupportedEntities => new string[]
        {
        };
        public string[] Samples => new string[]
        {
            "What time is it",
            "What is the date",
            "Can you give me the time please"
        };

        /// <summary>
        /// Loria's configuration.
        /// </summary>
        public Configuration Configuration { get; set; }

        /// <summary>
        /// Create the date time module.
        /// </summary>
        /// <param name="configuration">Loria's configuration.</param>
        public DateTimeModule(Configuration configuration)
        {
            Configuration = configuration;
        }

        /// <summary>
        /// Configure the date time module.
        /// </summary>
        public void Configure()
        {
            // Nothing to configure
        }

        /// <summary>
        /// Check if date time module is enabled.
        /// </summary>
        /// <returns>State of date time module.</returns>
        public bool IsEnabled() => true;

        /// <summary>
        /// Activate the date time module.
        /// </summary>
        public void Activate()
        {
        }

        /// <summary>
        /// Deactivate the date time module.
        /// </summary>
        public void Deactivate()
        {
        }

        /// <summary>
        /// Perform the action and intents.
        /// </summary>
        /// <param name="args">Should contains one intent and zero, one or multiple entities.</param>
        public void Perform(Command command)
        {
            switch (command.Intent.ToLowerInvariant())
            {
                case DateIntent:

                    var todayDate = DateTime.Now.ToString("D", Configuration.Culture);
                    Configuration.Hub.PropagateCallback(todayDate);
                    break;

                case TimeIntent:

                    var todayTime = DateTime.Now.ToString("t", Configuration.Culture);
                    Configuration.Hub.PropagateCallback(todayTime);
                    break;

                default:
                    Configuration.Hub.PropagateCallback(Resources.Strings.DateTimeIntentNotUnderstood);
                    break;
            }
        }
    }
}
