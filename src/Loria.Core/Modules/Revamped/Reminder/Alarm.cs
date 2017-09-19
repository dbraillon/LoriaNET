using System;

namespace LoriaNET.Reminder
{
    public class Alarm
    {
        public Guid Id { get; set; }
        public string Text { get; set; }
        public Event Event { get; set; }

        public Alarm(string text, DateTime dateTime)
        {
            Id = Guid.NewGuid();
            Text = text;
            Event = new DateEvent(dateTime);
        }
    }
}
