using Microsoft.CognitiveServices;

namespace Loria.Core
{
    public class VoiceModule : IModule, IAction, ICallback
    {
        public const string MicrosoftCognitiveApiKey = "voice::MicrosoftCognitiveApiKey";

        public string Name => "Voice module";
        public string Command => "say";
        public string Description => "Make Loria says whatever you want";
        public string Usage => "say MESSAGE";

        public Configuration Configuration { get; set; }
        public bool Enabled { get; set; }

        public TextToSpeech TextToSpeech { get; set; }
        
        public VoiceModule(Configuration configuration)
        {
            Configuration = configuration;
            Enabled = false;
        }

        public void Activate() => Enabled = true;
        public void Deactivate() => Enabled = false;
        public bool IsEnabled() => Enabled;

        public void Configure()
        {
            var microsoftCognitiveApiKey = Configuration.Configs[MicrosoftCognitiveApiKey];

            if (!string.IsNullOrEmpty(microsoftCognitiveApiKey))
            {
                TextToSpeech = new TextToSpeech(microsoftCognitiveApiKey);
                Enabled = true;
            }
        }

        public void Callback(string message)
        {
            TextToSpeech.RequestAsync(message, Configuration.Culture).GetAwaiter().GetResult();
        }

        public void Perform(string[] args)
        {
            Callback(string.Join(" ", args));
        }
    }
}
