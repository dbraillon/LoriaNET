using Loria.Core;

namespace Loria.App
{
    class Program
    {
        static void Main(string[] args)
        {
            var loria = new Core.Loria(new Configuration());
            loria.Live();
        }
    }
}
