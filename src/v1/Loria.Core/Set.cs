using System;
using System.Collections.Generic;
using System.Linq;

namespace Loria.Core
{
    public class Set<TObject>
        where TObject : IHasName
    {
        public TObject[] Objects { get; set; }

        public Set(params TObject[] objects)
        {
            Objects = objects ?? new TObject[0];
        }
        public Set(IEnumerable<TObject> objects)
            : this(objects?.ToArray())
        {
        }

        public TObject Get(string name) => Objects.FirstOrDefault(obj => string.Equals(obj.Name, name, StringComparison.InvariantCultureIgnoreCase));
    }
}
