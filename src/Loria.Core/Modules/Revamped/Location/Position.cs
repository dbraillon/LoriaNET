using LoriaNET.Core.Database;
using LoriaNET.Resources;
using LoriaNET.Storage.Database;
using System;

namespace LoriaNET.Location
{
    /// <summary>
    /// Defines a person location.
    /// </summary>
    public class Position
    {
        public PersonEntity Person { get; set; }
        public Place Place { get; set; }
        public Coordinates Coordinates { get; set; }
        public PositionState State { get; set; }

        public Position(PersonEntity person)
        {
            Person = person;
        }

        public void SetPlace(Place place)
        {
            Coordinates = null;
            Place = place;
            State = PositionState.At;
        }

        public void SetCoordinates(Coordinates coordinates)
        {
            Coordinates = coordinates;
            Place = null;
            State = PositionState.At;
        }

        public void LeavePlace(Place place)
        {
            SetPlace(place);
            State = PositionState.Leave;
        }

        public void EnterPlace(Place place)
        {
            SetPlace(place);
            State = PositionState.Enter;
        }

        public override string ToString()
        {
            var state =
                State == PositionState.At ? Strings.LocationModulePositionAt :
                State == PositionState.Enter ? Strings.LocationModulePositionEnter :
                State == PositionState.Leave ? Strings.LocationModulePositionLeave :
                string.Empty;

            var position =
                Place != null ? Place.ToString() :
                Coordinates != null ? Coordinates.ToString() :
                Strings.LocationModuleIDontKnow;

            return $"{Person} {state} {position}";
        }
    }
}
