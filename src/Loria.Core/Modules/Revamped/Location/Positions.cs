using System.Collections.Generic;
using System.Linq;

namespace LoriaNET.Location
{
    /// <summary>
    /// Defines a set of person location.
    /// </summary>
    public class Positions
    {
        public List<Position> Items { get; set; }

        public Positions()
        {
            Items = new List<Position>();
        }

        public bool IsKnown(Person person)
        {
            return Items.Any(i => i.Person == person);
        }

        public Position Add(Person person)
        {
            if (IsKnown(person)) return Get(person);

            var position = new Position(person);
            Items.Add(position);

            return position;
        }
        public void AddRange(IEnumerable<Person> persons)
        {
            foreach (var person in persons)
            {
                Add(person);
            }
        }
        public void Remove(Person person)
        {
            var position = Get(person);
            if (position != null)
            {
                Items.Remove(position);
            }
        }
        public Position Get(Person person)
        {
            if (!IsKnown(person)) return Add(person);

            return Items.First(p => p.Person == person);
        }

        public void SetPlace(Person person, Place place)
        {
            Get(person)?.SetPlace(place);
        }
        public void SetCoordinate(Person person, Coordinates coordinates)
        {
            Get(person)?.SetCoordinates(coordinates);
        }
        public void LeavePlace(Person person, Place place)
        {
            Get(person)?.LeavePlace(place);
        }
        public void EnterPlace(Person person, Place place)
        {
            Get(person)?.EnterPlace(place);
        }
    }
}
