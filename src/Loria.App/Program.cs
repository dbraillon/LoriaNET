using System.Globalization;

namespace LoriaNET
{
    class Program
    {
        static void Main(string[] args)
        {
            var loria = new Loria(
                configuration: new Configuration(
                    culture: CultureInfo.GetCultureInfo("fr-fr")
                )
            );
            loria.Live();
        }
    }
}
