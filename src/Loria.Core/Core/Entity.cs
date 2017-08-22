namespace LoriaNET
{
    /// <summary>
    /// A class to keep entity part of a command.
    /// </summary>
    public class Entity
    {
        public string Name { get; set; }
        public string Value { get; set; }

        public Entity()
        {
        }

        public Entity(string name, string value)
        {
            Name = name;
            Value = value;
        }

        public override string ToString() => $"-{Name} {Value}";
    }
}
