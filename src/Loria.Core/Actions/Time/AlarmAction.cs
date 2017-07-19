using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Loria.Core.Actions.Time
{
    public class AlarmAction : IAction
    {
        public string Command => "alarm";
        public string Description => "Set alarms";
        public string Usage => "alarm MODIFIER [NAME] [DATETIME]";

        public Dictionary<string, DateTime> Alarms { get; set; }
        public Configuration Configuration { get; set; }
        public bool AlarmThreadIsRunning { get; set; }
        public Thread AlarmThread { get; set; }

        public AlarmAction(Configuration configuration)
        {
            Alarms = new Dictionary<string, DateTime>();
            AlarmThreadIsRunning = false;
            AlarmThread = new Thread(Loop);
            AlarmThread.Start();

            Configuration = configuration;
        }

        ~AlarmAction()
        {
            AlarmThreadIsRunning = false;
            AlarmThread.Join();
            AlarmThread = null;
        }

        public void Loop()
        {
            AlarmThreadIsRunning = true;

            while (AlarmThreadIsRunning)
            {
                var alarms = new Dictionary<string, DateTime>(Alarms);

                foreach (var alarm in alarms)
                {
                    if (alarm.Value < DateTime.Now)
                    {
                        Configuration.TalkBack(alarm.Key);
                        Alarms.Remove(alarm.Key);
                    }
                }

                Thread.Sleep(1000);
            }
        }

        public async Task PerformAsync(string[] args)
        {
            await Task.Delay(0);

            var modifier = args.FirstOrDefault();
            if (modifier != null)
            {
                if (string.Equals(modifier, "get", StringComparison.CurrentCultureIgnoreCase))
                {
                    if (Alarms.Any())
                    {
                        foreach (var alarm in Alarms)
                        {
                            Configuration.TalkBack($"{alarm.Key}: {alarm.Value.ToLongDateString()} {alarm.Value.ToLongTimeString()}");
                        }
                    }
                    else
                    {
                        Configuration.TalkBack($"No alarm set");
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
                Console.WriteLine();
                Console.WriteLine("Modifiers:");

                Console.WriteLine($" get\tRetrieve all alarms");
                Console.WriteLine($" set\tSet an alarm");
                Console.WriteLine($" rm\tRemove an alarm");

                Console.WriteLine();
            }
        }
    }
}
