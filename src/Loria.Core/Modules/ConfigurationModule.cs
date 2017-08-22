//using System;
//using System.Configuration;
//using System.Linq;

//namespace LoriaNET
//{
//    public class ConfigurationModule : IModule, IAction
//    {
//        public string Name => "Configuration module";
//        public string Command => "conf";
//        public string Description => "Get or set a property value of configuration object";
//        public string Usage => "conf MODIFIER PROPERTY [VALUE]";

//        public Configuration Configuration { get; set; }

//        public ConfigurationModule(Configuration configuration)
//        {
//            Configuration = configuration;
//        }
        
//        public void Activate()
//        {
//        }

//        public void Deactivate()
//        {
//        }

//        public void Configure()
//        {
//        }

//        public bool IsEnabled() => true;

//        public void Perform(string[] args)
//        {
//            var modifier = args.FirstOrDefault();
//            if (modifier != null)
//            {
//                var key = args.Skip(1).FirstOrDefault();

//                if (ConfigurationManager.AppSettings.AllKeys.Contains(key))
//                {
//                    var value = ConfigurationManager.AppSettings.Get(key);

//                    if (string.Equals(modifier, "get", StringComparison.CurrentCultureIgnoreCase))
//                    {
//                        Configuration.Callbacks.CallbackAll($"{value}");
//                    }
//                    else if (string.Equals(modifier, "set", StringComparison.CurrentCultureIgnoreCase))
//                    {
//                        var newValue = string.Join(" ", args.Skip(2));
//                        ConfigurationManager.AppSettings.Set(key, newValue);
//                    }
//                }
//                else
//                {
//                    Configuration.Callbacks.CallbackAll("Configs:");

//                    foreach (var allKey in ConfigurationManager.AppSettings.AllKeys)
//                    {
//                        Configuration.Callbacks.CallbackAll($" {allKey}\t{ConfigurationManager.AppSettings.Get(allKey)}");
//                    }
//                }
//            }
//            else
//            {
//                Configuration.Callbacks.CallbackAll("Modifiers:");
//                Configuration.Callbacks.CallbackAll($" get\tRetrieve a property value");
//                Configuration.Callbacks.CallbackAll($" set\tOverride a property value");
//            }
//        }
//    }
//}
