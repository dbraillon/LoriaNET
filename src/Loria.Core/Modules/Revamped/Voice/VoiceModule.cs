using Loria.Voice;

namespace LoriaNET.Voice
{
    public class VoiceModule : Module, ICallback
    {
        public override string Name => "Voice module";

        public Speaker Speaker { get; }

        public VoiceModule(Loria loria) 
            : base(loria)
        {
            Speaker = new Speaker();
        }
        
        public override void Configure()
        {
            if (Speaker.CanSpeak(Loria.Data.Culture))
            {
                Speaker.SetVoice(Loria.Data.Culture);
                Activate();
            }
        }

        public void Callback(string message)
        {
            Speaker.Speak(message);
        }
    }
}
