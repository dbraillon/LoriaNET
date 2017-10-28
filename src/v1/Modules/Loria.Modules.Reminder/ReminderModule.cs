using Loria.Core;
using System;
using System.Collections.Generic;
using System.Globalization;

namespace Loria.Modules.Reminder
{
    public class ReminderModule : Listener, IAction
    {
        public override string Name => "Reminder module";
        public override string Description => "The reminder module provides intents and entities for creating, editing, and finding reminders";

        public const string ChangeIntent = "change";
        public const string CreateIntent = "create";
        public const string DeleteIntent = "delete";
        public const string FindIntent = "find";

        public const string TextEntity = "text";
        public const string DateTimeEntity = "datetime";

        public string Command => "reminder";
        public string[] SupportedIntents => new string[]
        {
            ChangeIntent, CreateIntent, DeleteIntent, FindIntent
        };
        public string[] SupportedEntities => new string[]
        {
            TextEntity, DateTimeEntity
        };
        public string[] Samples => new string[]
        {
            "reminder create -datetime 2017-09-15T07:48 -text wake up!",
            "reminder delete -text wake up!",
            "reminder change -text wake up! -datetime 2017-09-15T10:00",
            "reminder find -text wake"
        };

        public List<Alarm> Reminders { get; set; }

        public ReminderModule(Engine engine)
            : base(engine, 1000)
        {
            Reminders = new List<Alarm>();
        }

        public override void Configure()
        {
            Activate();
        }

        public void Perform(Command command)
        {
            var actionCommand = command.AsActionCommand();

            switch (actionCommand.Intent)
            {
                case CreateIntent:

                    // Get needed entities
                    var textEntity = actionCommand.GetEntity(TextEntity);
                    var dateEntity = actionCommand.GetEntity(DateTimeEntity);

                    // Give error messages if something is wrong with the command
                    if (textEntity == null || string.IsNullOrEmpty(textEntity.Value))
                    {
                        Engine.Propagator.PropagateCallback(new Command(""));
                        return;
                    }

                    // Give error messages if something is wrong with the command
                    if (dateEntity == null || string.IsNullOrEmpty(dateEntity.Value))
                    {
                        Engine.Propagator.PropagateCallback(new Command(""));
                        return;
                    }

                    // Parse given date
                    if (DateTime.TryParseExact(dateEntity.Value, new string[] { "yyyyMMddHHmm", "yyyy-MM-ddTHH:mm", "yyyy-MM-ddTHH" }, Engine.Data.Culture, DateTimeStyles.None, out DateTime date))
                    {
                        // Create the reminder
                        Reminders.Add(new Alarm(textEntity.Value, date));

                        // Callback what Loria understood
                        Engine.Propagator.PropagateCallback(new Command("console",
                            string.Join(" ",
                                "I will remind you to",
                                textEntity.Value,
                                "for",
                                date.ToLongDateString(),
                                "at",
                                date.ToLongTimeString()
                            )
                        ));
                    }
                    else
                    {
                        Engine.Propagator.PropagateCallback(new Command(""));
                    }

                    break;

                case DeleteIntent:
                    break;

                case ChangeIntent:
                    break;

                case FindIntent:
                    break;

                default:
                    Engine.Propagator.PropagateCallback(new Command(""));
                    break;
            }
        }

        protected override Command Listen()
        {
            if (!Enabled) return null;

            // Copy the list of reminders and loop through it
            var reminders = new List<Alarm>(Reminders);
            foreach (var reminder in reminders)
            {
                // Check event in reminder
                if (reminder.Event.Check())
                {
                    // Remove to avoid multiple firing
                    Reminders.Remove(reminder);

                    // Callback the reminder
                    return new Command("console", reminder.Text);
                }
            }

            // Nothing to do
            return null;
        }
    }
}
