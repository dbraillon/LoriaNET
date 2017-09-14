using System;

namespace LoriaNET.Location
{
    /// <summary>
    /// Defines a person location.
    /// </summary>
    public class Position
    {
        public Person Person { get; set; }
        public Place Place { get; set; }
        public Coordinates Coordinates { get; set; }
        public PositionState State { get; set; }

        public Position(Person person)
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
                State == PositionState.At ? "is at" :
                State == PositionState.Enter ? "enter" :
                State == PositionState.Leave ? "leave" :
                throw new NotImplementedException($"State {State} has not been implemented");

            var position =
                Place != null ? Place.ToString() :
                Coordinates != null ? Coordinates.ToString() :
                "... I don't know";

            return $"{Person} {state} {position}";
        }
    }
}
