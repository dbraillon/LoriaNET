using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;

namespace Loria.Core
{
    public class Configuration
    {
        public CultureInfo DefaultCulture => CultureInfo.GetCultureInfo("en-US");
        public CultureInfo Culture { get; set; }

        private List<Action<string>> _TalkBackActions { get; } = new List<Action<string>>();

        public Configuration()
        {
            Culture = CultureInfo.CurrentCulture;
        }

        public void RegisterTalkBack(Action<string> talkBackAction)
        {
            _TalkBackActions.Add(talkBackAction);
        }

        public void RegisterTalkBack<TTalkBack>() where TTalkBack : ITalkBack
        {
            var type = typeof(TTalkBack);
            var constructors = type.GetConstructors().Where(c => c.IsPublic);

            var hasInjectableConstructor = constructors.Any(c => 
                c.GetParameters().Any(p => 
                    p.ParameterType == typeof(Configuration) && 
                    c.GetParameters().Length == 1
                )
            );

            if (hasInjectableConstructor)
            {
                var talkBackItem = (TTalkBack)Activator.CreateInstance(typeof(TTalkBack), this);
                _TalkBackActions.Add(async (mess) => await talkBackItem.TalkBackAsync(mess));

                return;
            }
            else
            {
                var hasParameterlessConstructor = constructors.Any(c =>
                    c.GetParameters().Length == 0
                );

                if (hasParameterlessConstructor)
                {
                    var talkBackItem = Activator.CreateInstance<TTalkBack>();
                    _TalkBackActions.Add(async (mess) => await talkBackItem.TalkBackAsync(mess));

                    return;
                }
            }

            throw new NotImplementedException($"Can't create instance of {type.Name}");
        }

        public void TalkBack(string message)
        {
            foreach (var talkBackAction in _TalkBackActions)
            {
                talkBackAction(message);
            }
        }
    }
}
