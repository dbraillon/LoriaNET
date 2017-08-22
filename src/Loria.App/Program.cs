using System.Globalization;

namespace LoriaNET
{
    class Program
    {
        static void Main(string[] args)
        {
            var loria = new Loria(new Configuration(CultureInfo.GetCultureInfo("fr-fr")));
            loria.Live();
        }
    }
}
