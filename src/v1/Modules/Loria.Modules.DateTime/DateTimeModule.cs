using Loria.Core;

namespace Loria.Modules.DateTime
{
    public class DateTimeModule : Module, IAction
    {
        public override string Name => "Date time module";
        public override string Description => "It allows me to give you current date and time";

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

        public const string DateIntent = "date";
        public const string TimeIntent = "time";

        public DateTimeModule(Engine engine)
            : base(engine)
        {
        }

        public override void Configure()
        {
            // No configuration needed, always activated
            Activate();
        }

        public void Perform(Command command)
        {
            var actionCommand = command.AsActionCommand();

            switch (actionCommand.Intent)
            {
                case DateIntent:

                    var todayDate = System.DateTime.Now.ToString("D", Engine.Data.Culture);
                    Engine.Propagator.PropagateCallback(new Command("console", todayDate));
                    break;

                case TimeIntent:

                    var todayTime = System.DateTime.Now.ToString("t", Engine.Data.Culture);
                    Engine.Propagator.PropagateCallback(new Command("console", todayTime));
                    break;

                default:
                    break;
            }
        }
    }
}
