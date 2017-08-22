//using Microsoft.CognitiveServices;
//using System;
//using System.Linq;
//using System.Speech.Recognition;

//namespace LoriaNET
//{
//    public class SpeechRecognitionModule : Listener, IModule
//    {
//        public const string MicrosoftCognitiveApiKey = "voice::MicrosoftCognitiveApiKey";
//        public const string LuisAppId = "voice::LuisAppId";
//        public const string LuisSubscriptionId = "voice::LuisSubscriptionId";

//        public override string Name => "SpeechRecognition module";
        
//        public SpeechRecognitionEngine SpeechRecognitionEngine { get; set; }
//        public SpeechToText SpeechToText { get; set; }

//        public SpeechRecognitionModule(Configuration configuration)
//            : base(configuration, 1000)
//        {
//        }

//        public void Configure()
//        {
//            SpeechRecognitionEngine = new SpeechRecognitionEngine(Configuration.Culture);
//            SpeechRecognitionEngine.SetInputToDefaultAudioDevice();
//            SpeechRecognitionEngine.SpeechRecognized += (s, e) =>
//            {
//                Configuration.Hub($"callback Oui ?");
//                SpeechToText.Start();
//            };

//            Choices colors = new Choices();
//            colors.Add(new string[] { "loria" });

//            GrammarBuilder gb = new GrammarBuilder();
//            gb.Append(colors);
//            gb.Culture = Configuration.Culture;

//            // Create the Grammar instance.
//            Grammar g = new Grammar(gb);
            
//            SpeechRecognitionEngine.LoadGrammar(g);

//            var microsoftCognitiveApiKey = Configuration.Get(MicrosoftCognitiveApiKey);
//            var luisAppId = Configuration.Get(LuisAppId);
//            var luisSubscriptionId = Configuration.Get(LuisSubscriptionId);

//            if (!string.IsNullOrEmpty(microsoftCognitiveApiKey))
//            {
//                SpeechToText = new SpeechToText(microsoftCognitiveApiKey, Configuration.Culture, luisAppId, luisSubscriptionId);
//                SpeechToText.OnSpeechRecognized += (r) => Configuration.Hub(r);
//            }
//        }

//        public void Activate()
//        {
//        }

//        public void Deactivate()
//        {
//        }

//        public bool IsEnabled() => true;

//        protected override void Loop()
//        {
//            SpeechRecognitionEngine.RecognizeAsync(RecognizeMode.Multiple);
//            base.Loop();
//        }
//        public override string Listen() => string.Empty;
//    }
//}
