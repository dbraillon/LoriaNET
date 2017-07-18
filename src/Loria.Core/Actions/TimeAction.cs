using System;
using System.Threading.Tasks;

namespace Loria.Core.Actions
{
    public class TimeAction : IAction
    {
        public string Command => "time";
        public string Description => "Give you the current time";
        public string Usage => "time";

        public Configuration Configuration { get; set; }

        public TimeAction(Configuration configuration)
        {
            Configuration = configuration;
        }

        public async Task PerformAsync(string[] args)
        {
            await Task.Delay(0);

            var time = DateTime.Now;
            var timeStr = time.ToLongTimeString();

            Configuration.TalkBack(timeStr);
        }
    }
}
