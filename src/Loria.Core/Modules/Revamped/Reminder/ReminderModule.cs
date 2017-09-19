using System;
using System.Collections.Generic;
using System.Globalization;

namespace LoriaNET.Reminder
{
    /// <summary>
    /// The reminder module provides intents and entities for creating, editing, and finding reminders.
    /// </summary>
    public class ReminderModule : Listener, IModule, IAction
    {
        const string ChangeIntent = "change";
        const string CreateIntent = "create";
        const string DeleteIntent = "delete";
        const string FindIntent = "find";

        const string TextEntity = "text";
        const string DateTimeEntity = "datetime";

        public override string Name => "Reminder module";

        public string Description => "The reminder module provides intents and entities for creating, editing, and finding reminders";

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
        
        public ReminderModule(Configuration configuration)
            : base(configuration, 1000)
        {
            Reminders = new List<Alarm>();
        }
        
        public override void Configure()
        {
            Activate();
        }
        
        public void Perform(Command command)
        {
            switch (command.Intent)
            {
                case CreateIntent:

                    // Get needed entities
                    var textEntity = command.GetEntity(TextEntity);
                    var dateEntity = command.GetEntity(DateTimeEntity);

                    // Give error messages if something is wrong with the command
                    if (textEntity == null || string.IsNullOrEmpty(textEntity.Value))
                    {
                        Configuration.Hub.PropagateCallback(Resources.Strings.ReminderTextNotFound);
                        return;
                    }

                    // Give error messages if something is wrong with the command
                    if (dateEntity == null || string.IsNullOrEmpty(dateEntity.Value))
                    {
                        Configuration.Hub.PropagateCallback(Resources.Strings.ReminderDateNotFound);
                        return;
                    }

                    // Parse given date
                    if (DateTime.TryParseExact(dateEntity.Value, new string[] { "yyyyMMddHHmm", "yyyy-MM-ddTHH:mm", "yyyy-MM-ddTHH" }, Configuration.Culture, DateTimeStyles.None, out DateTime date))
                    {
                        // Create the reminder
                        Reminders.Add(new Alarm(textEntity.Value, date));

                        // Callback what Loria understood
                        Configuration.Hub.PropagateCallback(
                            string.Join(" ",
                                Resources.Strings.ReminderCreateConfirmation,
                                textEntity.Value,
                                Resources.Strings.GeneralFor,
                                date.ToLongDateString(),
                                Resources.Strings.GeneralAt,
                                date.ToLongTimeString()
                            )
                        );
                    }
                    else
                    {
                        Configuration.Hub.PropagateCallback(Resources.Strings.ReminderDateNotUnderstood);
                    }

                    break;

                case DeleteIntent:
                    break;

                case ChangeIntent:
                    break;

                case FindIntent:
                    break;

                default:
                    Configuration.Hub.PropagateCallback(Resources.Strings.ReminderIntentNotUnderstood);
                    break;
            }
        }

        public override string Listen()
        {
            if (!Enabled) return string.Empty;

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
                    return $"{Callbacks.Keyword} {reminder.Text}";
                }
            }

            // Nothing to do
            return string.Empty;
        }
    }
}
