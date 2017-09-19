using System;
using System.Collections.Generic;
using System.Linq;

namespace LoriaNET.Location
{
    /// <summary>
    /// Defines a set of known places.
    /// </summary>
    public class Places
    {
        public List<Place> Items { get; set; }

        public Places()
        {
            Items = new List<Place>();
        }

        public void Add(Place place)
        {
            Remove(place.Label);
            Items.Add(place);
        }

        public void Remove(string label)
        {
            var place = Get(label);
            if (place != null)
            {
                Items.Remove(place);
            }
        }

        public Place Get(string label)
        {
            return Items.FirstOrDefault(i => string.Equals(i.Label, label, StringComparison.InvariantCultureIgnoreCase));
        }
    }
}
