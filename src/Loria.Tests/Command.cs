using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Loria.Tests
{
    public class Command
    {
        public string Module { get; set; }
        public string Intent { get; set; }
        public Dictionary<string, string> Entities { get; set; }

        public Command()
        {
            Entities = new Dictionary<string, string>();
        }
    }
}
