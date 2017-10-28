using System.Globalization;
using System.Linq;
using System.Speech.Synthesis;

namespace Loria.Modules.Voice
{
    public class Speaker
    {
        protected SpeechSynthesizer SpeechSynthesizer { get; }

        public Speaker()
        {
            SpeechSynthesizer = new SpeechSynthesizer();
            SpeechSynthesizer.SetOutputToDefaultAudioDevice();
            SpeechSynthesizer.Rate = 0;
        }

        public void SetVoice(CultureInfo culture)
        {
            SpeechSynthesizer.SelectVoiceByHints(VoiceGender.Female, VoiceAge.NotSet, 0, culture);
        }

        public bool CanSpeak(CultureInfo culture)
        {
            return SpeechSynthesizer.GetInstalledVoices(culture).Any();
        }

        public void Speak(string message)
        {
            SpeechSynthesizer.Speak(message);
        }
    }
}
