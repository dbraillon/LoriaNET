using System;
using System.Collections.Generic;
using System.Globalization;

namespace LoriaNET
{
    /// <summary>
    /// The reminder module provides intents and entities for creating, editing, and finding reminders.
    /// </summary>
    internal sealed class ReminderModule : Listener, IModule, IAction
    {
        const string ChangeIntent = "change";
        const string CreateIntent = "create";
        const string DeleteIntent = "delete";
        const string FindIntent = "find";

        const string TextEntity = "text";
        const string DateTimeEntity = "datetime";
        const string DateTime2Entity = "datetime2";
        const string DateEntity = "date";
        const string TimeEntity = "time";
        const string LocationEntity = "location";

        public override string Name => "Reminder";
        public string Description => "The reminder module provides intents and entities for creating, editing, and finding reminders.";

        public string Command => "reminder";
        public string[] SupportedIntents => new string[]
        {
            ChangeIntent, CreateIntent, DeleteIntent, FindIntent
        };
        public string[] SupportedEntities => new string[]
        {
            TextEntity, DateEntity, LocationEntity
        };
        public string[] Samples => new string[]
        {
            "Change my interview to 9 am tomorrow",
            "Remind me to buy milk on my way back home",
            "Can you check if I have a reminder about Christine's birthday?"
        };

        /// <summary>
        /// A flag to know if reminder module is enabled.
        /// </summary>
        private bool Enabled { get; set; }

        /// <summary>
        /// A list containing every reminders.
        /// </summary>
        public List<Reminder> Reminders { get; set; }
        
        /// <summary>
        /// Create the reminder module.
        /// </summary>
        /// <param name="configuration">Loria's configuration.</param>
        public ReminderModule(Configuration configuration)
            : base(configuration, 1000)
        {
            Reminders = new List<Reminder>();
        }
        
        /// <summary>
        /// Configure the reminder module.
        /// </summary>
        public void Configure()
        {
            // Nothing to configure
            Activate();
        }

        /// <summary>
        /// Check if reminder module is enabled.
        /// </summary>
        /// <returns>State of reminder module.</returns>
        public bool IsEnabled() => Enabled;
        
        /// <summary>
        /// Activate the reminder module.
        /// </summary>
        public void Activate() => Enabled = true;

        /// <summary>
        /// Deactivate the reminder module.
        /// </summary>
        public void Deactivate() => Enabled = false;

        /// <summary>
        /// Perform the action and intents.
        /// </summary>
        /// <param name="args">Should contains one intent and zero, one or multiple entities.</param>
        public void Perform(Command command)
        {
            switch (command.Intent.ToLowerInvariant())
            {
                case CreateIntent:

                    // Get needed entities (location is not yet supported)
                    var textEntity = command.GetEntity(TextEntity);
                    //var dateTimeEntity = command.GetEntity(DateTimeEntity);
                    //var dateTime2Entity = command.GetEntity(DateTime2Entity);
                    var dateEntity = command.GetEntity(TimeEntity);
                    //var timeEntity = command.GetEntity(TimeEntity);

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
                    if (DateTime.TryParseExact(dateEntity.Value, new string[] { "yyyyMMddHHmm", "yyyy-MM-ddTHH:mm" }, Configuration.Culture, DateTimeStyles.None, out DateTime date))
                    {
                        // Create the reminder
                        Reminders.Add(new Reminder(textEntity.Value, date));

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

        /// <summary>
        /// Will check each reminders to fire the first that needed to be fired.
        /// </summary>
        /// <returns>A callback or empty.</returns>
        public override string Listen()
        {
            if (!Enabled) return string.Empty;

            // Copy the list of reminders and loop through it
            var reminders = new List<Reminder>(Reminders);
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

    /// <summary>
    /// The reminder class.
    /// </summary>
    public class Reminder
    {
        /// <summary>
        /// A unique ID among all reminders.
        /// </summary>
        public Guid Id { get; set; }

        /// <summary>
        /// The text to remind.
        /// </summary>
        public string Text { get; set; }

        /// <summary>
        /// The firing event.
        /// </summary>
        public Event Event { get; set; }

        /// <summary>
        /// Create a reminder with a date time event.
        /// </summary>
        /// <param name="text">A text to remind.</param>
        /// <param name="dateTime">A date time when the reminder must fire.</param>
        public Reminder(string text, DateTime dateTime)
        {
            Id = Guid.NewGuid();
            Text = text;
            Event = new DateEvent(dateTime);
        }

        /// <summary>
        /// Create a reminder with a location event.
        /// </summary>
        /// <param name="text">A text to remind.</param>
        /// <param name="location">A location where the reminder must fire.</param>
        public Reminder(string text, string location)
        {
            throw new NotImplementedException();
        }
    }

    /// <summary>
    /// Base class for events.
    /// </summary>
    public abstract class Event
    {
        /// <summary>
        /// Reminder module will check events to be fired.
        /// </summary>
        /// <returns>Ready to fire, or not.</returns>
        public abstract bool Check();
    }

    /// <summary>
    /// Event that will fire against a precise date and time.
    /// </summary>
    public class DateEvent : Event
    {
        /// <summary>
        /// The date time when the event must be fired.
        /// </summary>
        public DateTime DateTime { get; set; }

        /// <summary>
        /// Create a new date event.
        /// </summary>
        /// <param name="dateTime">The date time when the event must be fired.</param>
        public DateEvent(DateTime dateTime)
        {
            DateTime = dateTime;
        }

        /// <summary>
        /// Create a new date event with date and time.
        /// </summary>
        /// <param name="date">A date.</param>
        /// <param name="time">A time.</param>
        public DateEvent(string date, string time)
        {

        }
        
        public override bool Check() => DateTime.Now >= DateTime;
    }

    /// <summary>
    /// Event that will fire against a precise location.
    /// </summary>
    public class LocationEvent : Event
    {
        public override bool Check()
        {
            throw new NotImplementedException();
        }
    }
}
