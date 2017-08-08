using System;
using System.Collections.Generic;
using System.Linq;

namespace Loria.Core
{
    public class AlarmModule : Listener, IModule, IAction
    {
        public override string Name => "Alarm module";
        public string Command => "alarm";
        public string Description => "Set alarms";
        public string Usage => "alarm MODIFIER [NAME] [DATETIME]";

        public Dictionary<string, DateTime> Alarms { get; set; }
        
        public AlarmModule(Configuration configuration)
            : base(configuration, 1000)
        {
            Alarms = new Dictionary<string, DateTime>();
        }
        
        public bool IsEnabled() => true;

        public void Configure()
        {
        }

        public void Activate()
        {
        }

        public void Deactivate()
        {
        }

        public void Perform(string[] args)
        {
            var modifier = args.FirstOrDefault();
            if (modifier != null)
            {
                if (string.Equals(modifier, "get", StringComparison.CurrentCultureIgnoreCase))
                {
                    if (Alarms.Any())
                    {
                        foreach (var alarm in Alarms)
                        {
                            Configuration.Callbacks.CallbackAll($"{alarm.Key}: {alarm.Value.ToLongDateString()} {alarm.Value.ToLongTimeString()}");
                        }
                    }
                    else
                    {
                        Configuration.Callbacks.CallbackAll($"No alarm set");
                    }
                }
                else if (string.Equals(modifier, "set", StringComparison.CurrentCultureIgnoreCase))
                {
                    var name = args.Skip(1).FirstOrDefault();
                    var value = string.Join(" ", args.Skip(2));
                    var date = DateTime.Parse(value, Configuration.Culture);

                    Alarms.Add(name, date);
                }
                else if (string.Equals(modifier, "rm", StringComparison.CurrentCultureIgnoreCase))
                {
                    var name = args.Skip(1).FirstOrDefault();
                    Alarms.Remove(name);
                }
            }
            else
            {
                Configuration.Callbacks.CallbackAll("Modifiers:");
                Configuration.Callbacks.CallbackAll($" get\tRetrieve all alarms");
                Configuration.Callbacks.CallbackAll($" set\tSet an alarm");
                Configuration.Callbacks.CallbackAll($" rm\tRemove an alarm");
            }
        }

        public override string Listen()
        {
            var alarms = new Dictionary<string, DateTime>(Alarms);
            var response = new List<string>();

            foreach (var alarm in alarms)
            {
                if (alarm.Value < DateTime.Now)
                {
                    response.Add(alarm.Key);
                    Alarms.Remove(alarm.Key);
                }
            }

            var callback = string.Join(" ", response);
            return string.IsNullOrEmpty(callback) ? string.Empty : $"callback {callback}";
        }
    }
}
