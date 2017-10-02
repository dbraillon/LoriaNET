using System;
using System.Globalization;

namespace Loria.Voice.Test
{
    class Program
    {
        static void Main(string[] args)
        {
            var speaker = new Speaker();
            speaker.SetVoice(CultureInfo.GetCultureInfo("fr-FR"));

            var input = string.Empty;
            do
            {
                Console.Write("> ");
                input = Console.ReadLine();

                speaker.Speak(input);
            }
            while (input != "quit");
        }
    }
}