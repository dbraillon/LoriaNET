using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LoriaNET
{
    public class Person
    {
        public string FirstName { get; set; }

        public bool Is(string firstName) => string.Equals(firstName, FirstName, StringComparison.InvariantCultureIgnoreCase);

        public override string ToString()
        {
            return $"{FirstName}";
        }
    }
}
