//using System.Linq;
//using System.Threading.Tasks;

//namespace LoriaNET
//{
//    public class HelpModule : IModule, IAction
//    {
//        public string Name => "Help module";

//        public string Command => "help";
//        public string Description => "Give you tips on command";
//        public string Usage => "help COMMAND";

//        public Configuration Configuration { get; set; }

//        public HelpModule(Configuration configuration)
//        {
//            Configuration = configuration;
//        }

//        public void Activate()
//        {
//            // Help module is always activated
//        }

//        public void Configure()
//        {
//            // Nothing to configure
//        }

//        public void Deactivate()
//        {
//            // Help module is always activated
//        }

//        public bool IsEnabled() => true;

//        public void Perform(string[] args)
//        {
//            var command = args.FirstOrDefault();
//            var relatedAction = Configuration.Actions.GetByCommand(command);

//            if (relatedAction == null)
//            {
//                Configuration.Callbacks.CallbackAll($"Command '{command}' does not exist.");
//                Configuration.Callbacks.CallbackAll($"Commands:");
//                Configuration.Actions.All.ForEach(a => Configuration.Callbacks.CallbackAll($" {a.Command}\t{a.Description}"));
//            }
//            else
//            {
//                Configuration.Callbacks.CallbackAll($"{relatedAction.Description} - {relatedAction.Usage}");
//            }
//        }
//    }
//}
