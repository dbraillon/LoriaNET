//using System;

//namespace LoriaNET
//{
//    public class TimeModule : IModule, IAction
//    {
//        public string Name => "Time module";
//        public string Command => "time";
//        public string Description => "Give you the current time";
//        public string Usage => "time";

//        public Configuration Configuration { get; set; }
        
//        public TimeModule(Configuration configuration)
//        {
//            Configuration = configuration;
//        }

//        public bool IsEnabled() => true;

//        public void Configure()
//        {
//        }

//        public void Activate()
//        {
//        }

//        public void Deactivate()
//        {
//        }

//        public void Perform(string[] args)
//        {
//            var time = DateTime.Now;
//            var timeStr = time.ToString("t", Configuration.Culture);

//            Configuration.Callbacks.CallbackAll(timeStr);
//        }
//    }
//}
