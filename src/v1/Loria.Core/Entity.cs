namespace Loria.Core
{
    public class Entity
    {
        public string Name { get; set; }
        public string Value { get; set; }

        public Entity(string name, string value)
        {
            Name = name;
            Value = value;
        }

        public override string ToString() => $"-{Name} {Value}";
    }
}
