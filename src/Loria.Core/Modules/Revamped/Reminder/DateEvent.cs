using System;

namespace LoriaNET.Reminder
{
    /// <summary>
    /// Event that will fire against a precise date and time.
    /// </summary>
    public class DateEvent : Event
    {
        public DateTime DateTime { get; set; }

        public DateEvent(DateTime dateTime)
        {
            DateTime = dateTime;
        }

        public override bool Check() => DateTime.Now >= DateTime;
    }
}
