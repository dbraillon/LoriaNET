using Loria.Core;

namespace Loria.Modules.Voice
{
    public class VoiceModule : Module, ICallback
    {
        public override string Name => "Voice module";
        public override string Description => "It allows me to speak with you with my voice";

        public string Command => "voice";

        public Speaker Speaker { get; }
        
        public VoiceModule(Engine engine) 
            : base(engine)
        {
            Speaker = new Speaker();
        }

        public override void Configure()
        {
            if (Speaker.CanSpeak(Engine.Data.Culture))
            {
                Speaker.SetVoice(Engine.Data.Culture);
                Activate();
            }
        }

        public void Perform(Command command)
        {
            Speaker.Speak(command.AsCallbackCommand().Message);
        }
    }
}
