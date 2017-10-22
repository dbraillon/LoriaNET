using System.Collections.Generic;

namespace Loria.Core
{
    public class Actions : CommandSet<IAction>
    {
        public const string Keyword = "perform";

        public Actions(params IAction[] objects) : base(objects)
        {
        }
        public Actions(IEnumerable<IAction> objects) : base(objects)
        {
        }
    }
}
