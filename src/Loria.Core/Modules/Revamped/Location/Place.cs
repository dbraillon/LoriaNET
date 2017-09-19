namespace LoriaNET.Location
{
    /// <summary>
    /// Defines a custom named location with specific coordinates.
    /// </summary>
    public class Place
    {
        public Coordinates Coordinates { get; set; }
        public string Label { get; set; }

        public Place(Coordinates coordinates, string label)
        {
            Coordinates = coordinates;
            Label = label;
        }

        public override string ToString()
        {
            return Label;
        }
    }
}
