using Microsoft.CognitiveServices;
using System.Threading.Tasks;

namespace Loria.Core.Actions
{
    public class SoundAction : IAction, ITalkBack
    {
        public string Command => "say";
        public string Description => "Make Loria says whatever you want";
        public string Usage => "say MESSAGE";

        public Configuration Configuration { get; set; }
        
        public SoundAction(Configuration configuration)
        {
            Configuration = configuration;
        }

        public async Task PerformAsync(string[] args)
        {
            var message = string.Join(" ", args);

            var textToSpeech = new TextToSpeech();
            await textToSpeech.RequestAsync(message, Configuration.Culture);
        }

        public async Task TalkBackAsync(string message)
        {
            var textToSpeech = new TextToSpeech();
            await textToSpeech.RequestAsync(message, Configuration.Culture);
        }
    }
}
