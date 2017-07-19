using System;
using System.Threading.Tasks;

namespace Loria.Core.Actions.Time
{
    public class DateAction : IAction
    {
        public string Command => "date";
        public string Description => "Give you the current date";
        public string Usage => "date";

        public Configuration Configuration { get; set; }

        public DateAction(Configuration configuration)
        {
            Configuration = configuration;
        }

        public async Task PerformAsync(string[] args)
        {
            await Task.Delay(0);

            var time = DateTime.Now;
            var timeStr = time.ToLongDateString();

            Configuration.TalkBack(timeStr);
        }
    }
}
