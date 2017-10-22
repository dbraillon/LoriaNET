using System.Collections.Generic;

namespace LoriaNET
{
    public class Actions : HandleCommandSet<IAction>
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
