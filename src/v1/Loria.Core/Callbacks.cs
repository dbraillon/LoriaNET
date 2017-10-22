using System.Collections.Generic;

namespace Loria.Core
{
    public class Callbacks : CommandSet<ICallback>
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
