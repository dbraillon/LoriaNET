using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Loria.Core
{
    public class Callbacks
    {
        public List<ICallback> All { get; }

        public Callbacks(List<ICallback> callbacks)
        {
            All = callbacks;
        }

        public ICallback Get(string name) => All.FirstOrDefault(l => string.Equals(l.Name, name, StringComparison.InvariantCultureIgnoreCase));

        public void Callback(string name, string message) => Get(name)?.Callback(message);
        public void CallbackAll(string message) => All.ForEach(c => c.Callback(message));
    }
}
