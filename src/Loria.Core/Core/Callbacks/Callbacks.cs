using System.Collections.Generic;

namespace LoriaNET
{
    public class Callbacks : HandleCommandSet<ICallback>
    {
        public const string Keyword = "callback";

        public Callbacks(params ICallback[] objects) : base(objects)
        {
        }
        public Callbacks(IEnumerable<ICallback> objects) : base(objects)
        {
        }
    }
}
