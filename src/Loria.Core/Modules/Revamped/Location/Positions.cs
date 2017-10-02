using LoriaNET.Core.Database;
using LoriaNET.Storage.Database;
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

        public bool IsKnown(PersonEntity person)
        {
            return Items.Any(i => i.Person == person);
        }

        public Position Add(PersonEntity person)
        {
            if (IsKnown(person)) return Get(person);

            var position = new Position(person);
            Items.Add(position);

            return position;
        }
        public void AddRange(IEnumerable<PersonEntity> persons)
        {
            foreach (var person in persons)
            {
                Add(person);
            }
        }
        public void Remove(PersonEntity person)
        {
            var position = Get(person);
            if (position != null)
            {
                Items.Remove(position);
            }
        }
        public Position Get(PersonEntity person)
        {
            if (!IsKnown(person)) return Add(person);

            return Items.First(p => p.Person == person);
        }

        public void SetPlace(PersonEntity person, Place place)
        {
            Get(person)?.SetPlace(place);
        }
        public void SetCoordinate(PersonEntity person, Coordinates coordinates)
        {
            Get(person)?.SetCoordinates(coordinates);
        }
        public void LeavePlace(PersonEntity person, Place place)
        {
            Get(person)?.LeavePlace(place);
        }
        public void EnterPlace(PersonEntity person, Place place)
        {
            Get(person)?.EnterPlace(place);
        }
    }
}
