using Microsoft.CognitiveServices;
using System;
using System.Linq;
using System.Speech.Recognition;

namespace LoriaNET
{
    internal sealed class VoiceModule : Listener, IModule, IAction, ICallback
    {
        const string MicrosoftCognitiveApiKey = "voice::MicrosoftCognitiveApiKey";
        public const string LuisAppId = "voice::LuisAppId";
        public const string LuisSubscriptionId = "voice::LuisSubscriptionId";

        const string SayIntent = "say";
        const string TextEntity = "text";

        public override string Name => "Voice";
        public string Description => "The voice module provides a way to Loria to speak and to listen.";

        public string Command => "voice";
        public string[] SupportedIntents => new string[]
        {
            SayIntent
        };
        public string[] SupportedEntities => new string[]
        {
        };
        public string[] Samples => new string[]
        {
            "Say I love you",
            "Tell him to stop",
            "Ask if he build a house"
        };

        /// <summary>
        /// A flag to know if reminder module is enabled.
        /// </summary>
        public bool Enabled { get; set; }

        /// <summary>
        /// A flag to know if Loria is listening.
        /// </summary>
        public bool IsListening { get; set; }

        /// <summary>
        /// Cognitive service text to speech.
        /// </summary>
        public TextToSpeech TextToSpeech { get; set; }

        /// <summary>
        /// Local recognition engine.
        /// </summary>
        public SpeechRecognitionEngine SpeechRecognitionEngine { get; set; }

        /// <summary>
        /// Cognitive service speech to text.
        /// </summary>
        public SpeechToText SpeechToText { get; set; }

        /// <summary>
        /// Create the voice module.
        /// </summary>
        /// <param name="configuration">Loria's configuration.</param>
        public VoiceModule(Configuration configuration)
            : base(configuration, 1)
        {
        }

        /// <summary>
        /// Configure the voice module.
        /// </summary>
        public void Configure()
        {
            var microsoftCognitiveApiKey = Configuration.Get(MicrosoftCognitiveApiKey);
            if (string.IsNullOrEmpty(microsoftCognitiveApiKey))
            {
                Configuration.Hub.PropagateCallback(Resources.Strings.VoiceMicrosoftCognitiveServiceApiKeyNotFound);
                return;
            }

            var luisAppId = Configuration.Get(LuisAppId);
            if (string.IsNullOrEmpty(luisAppId))
            {
                Configuration.Hub.PropagateCallback(Resources.Strings.VoiceMicrosoftLuisAppIdNotFound);
                return;
            }

            var luisSubscriptionId = Configuration.Get(LuisSubscriptionId);
            if (string.IsNullOrEmpty(luisSubscriptionId))
            {
                Configuration.Hub.PropagateCallback(Resources.Strings.VoiceMicrosoftLuisApiKeyNotFound);
                return;
            }

            Enabled = true;
            SpeechToText = new SpeechToText(microsoftCognitiveApiKey, Configuration.Culture, luisAppId, luisSubscriptionId);
            SpeechToText.OnSpeechRecognized += (r) =>
            {
                var intent = r.Intents.Where(i => i.Intent != "None").First();
                var entities = string.Join(" ", r.Entities.Select(e =>
                {
                    if (e.Resolution != null)
                    {
                        return string.Join(" ", e.Resolution.Select(res => $"-{res.Key} {res.Value}"));
                    }
                    else
                    {
                        return $"-{e.Type} {e.Entity}";
                    }
                }));
                var intentSplitted = intent.Intent.Split('.');
                var command = $"{intentSplitted[0]} {intentSplitted[1]} {entities}".Trim();

                Configuration.Hub.Propagate(command);
                IsListening = true;
            };
            TextToSpeech = new TextToSpeech(microsoftCognitiveApiKey);

            SpeechRecognitionEngine = new SpeechRecognitionEngine(Configuration.Culture);
            SpeechRecognitionEngine.SetInputToDefaultAudioDevice();
            SpeechRecognitionEngine.SpeechRecognized += (s, e) =>
            {
                if (IsListening)
                {
                    IsListening = false;

                    Console.Beep();
                    SpeechToText.Start();
                }
            };
            SpeechRecognitionEngine.LoadGrammar(
                new Grammar(
                    new GrammarBuilder("loria")
                    {
                        Culture = Configuration.Culture
                    }
                )
            );
        }

        /// <summary>
        /// Check if voice module is enabled.
        /// </summary>
        /// <returns>State of reminder module.</returns>
        public bool IsEnabled() => Enabled;

        /// <summary>
        /// Activate the voice module.
        /// </summary>
        public void Activate() => Enabled = true;

        /// <summary>
        /// Deactivate the voice module.
        /// </summary>
        public void Deactivate() => Enabled = false;
        
        /// <summary>
        /// Will tell the message with Loria voice.
        /// </summary>
        /// <param name="message">A message to tell.</param>
        public void Callback(string message)
        {
            TextToSpeech.RequestAsync(message, Configuration.Culture).GetAwaiter().GetResult();
        }

        /// <summary>
        /// Perform the action and intents.
        /// </summary>
        /// <param name="args">Should contains one intent and zero, one or multiple entities.</param>
        public void Perform(Command command)
        {
            var text = command.GetEntity(TextEntity);
            if (text == null)
            {
                Callback(command.ToString());
            }
            else
            {
                Callback(text.Value);
            }
        }
        
        /// <summary>
        /// Will loop wait for the loria keyword.
        /// </summary>
        protected override void Loop()
        {
            SpeechRecognitionEngine.RecognizeAsync(RecognizeMode.Multiple);
            IsListening = true;

            base.Loop();
        }
        public override string Listen() => string.Empty;
    }
}
