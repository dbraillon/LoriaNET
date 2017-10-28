namespace Loria.Modules.Reminder
{
    public abstract class Event
    {
        /// <summary>
        /// Reminder module will check events to be fired.
        /// </summary>
        /// <returns>Ready to fire, or not.</returns>
        public abstract bool Check();
    }
}
