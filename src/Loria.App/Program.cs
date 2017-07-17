using Loria.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Loria.App
{
    class Program
    {
        static void Main(string[] args)
        {
            var input = string.Empty;
            var loria = new Core.Loria(new Configuration());

            while ((input = ReadLine()) != "quit")
            {
                try
                {
                    var splited = input.Split(' ');
                    var command = splited.FirstOrDefault();
                    var commandArgs = splited.Skip(1).ToArray();

                    loria.HandleCommandAsync(command, commandArgs).GetAwaiter().GetResult();
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.Message);
                }
            }
        }

        static string ReadLine()
        {
            Console.Write("loria> ");
            return Console.ReadLine();
        }
    }
}
