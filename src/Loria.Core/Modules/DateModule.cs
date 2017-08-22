//using System;

//namespace LoriaNET
//{
//    public class DateModule : IModule, IAction
//    {
//        public string Name => "Date module";
//        public string Command => "date";
//        public string Description => "Give you the current date";
//        public string Usage => "date";

//        public Configuration Configuration { get; set; }
        
//        public DateModule(Configuration configuration)
//        {
//            Configuration = configuration;
//        }

//        public void Activate()
//        {
//        }

//        public void Configure()
//        {
//        }

//        public void Deactivate()
//        {
//        }

//        public bool IsEnabled() => true;

//        public void Perform(string[] args)
//        {
//            var date = DateTime.Now;
//            var dateStr = date.ToString("D", Configuration.Culture);

//            Configuration.Callbacks.CallbackAll(dateStr);
//        }
//    }
//}
