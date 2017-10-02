using System;

namespace LoriaNET.Storage.Database
{
    public class PersonEntity : Entity
    {
        public string Name { get; set; }

        public bool Is(string name) => string.Equals(name, Name, StringComparison.InvariantCultureIgnoreCase);

        public override string ToString() => $"{Name}";
    }
}
