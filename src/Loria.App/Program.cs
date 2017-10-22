using LoriaNET.Core.Database;
using System.Globalization;
using System.Linq;

namespace LoriaNET
{
    class Program
    {
        static void Main(string[] args)
        {
            var loria = new Loria();

            if (args.Contains("-d"))
            {
                loria.SetLog(new ConsoleLog());
            }

            loria.Live();
        }
    }
}
