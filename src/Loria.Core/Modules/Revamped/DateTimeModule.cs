using System;

namespace LoriaNET
{
    /// <summary>
    /// The date time module provides intents and entities for giving the date and time.
    /// </summary>
    public sealed class DateTimeModule : Module, IAction
    {
        const string DateIntent = "date";
        const string TimeIntent = "time";

        public override string Name => "Date time module";
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
            "datetime date",
            "datetime time"
        };
        
        public DateTimeModule(Loria loria)
            : base(loria)
        {
        }

        public override void Configure()
        {
            Activate();
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

                    var todayDate = DateTime.Now.ToString("D", Loria.Data.Culture);
                    Loria.Hub.PropagateCallback(todayDate);
                    break;

                case TimeIntent:

                    var todayTime = DateTime.Now.ToString("t", Loria.Data.Culture);
                    Loria.Hub.PropagateCallback(todayTime);
                    break;

                default:
                    Loria.Hub.PropagateCallback(Resources.Strings.DateTimeIntentNotUnderstood);
                    break;
            }
        }
    }
}
