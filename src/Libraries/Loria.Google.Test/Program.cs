using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Loria.Google.Test
{
    class Program
    {
        static GoogleMap GoogleMap { get; set; }
        
        static void Main(string[] args)
        {
            GoogleMap = new GoogleMap(ConfigurationManager.AppSettings.Get("google::APIKey"));

            var input = string.Empty;
            do
            {
                Console.WriteLine("Welcome to google test application, what you want to do?");
                Console.WriteLine("0: Quit");
                Console.WriteLine("1: Geocoding");
                Console.WriteLine("2: Reverse geocoding");

                Console.Write("> ");
                input = Console.ReadLine();

                HandleUserInputAsync(input).Wait();

                Console.WriteLine();
            }
            while (input != "0");
        }

        static async Task<bool> HandleUserInputAsync(string input)
        {
            switch (input)
            {
                case "1": return await GeocodingAsync();
                case "2": return await ReverseGeocodingAsync();
                default: return false;
            }
        }

        static async Task<bool> GeocodingAsync()
        {
            Console.WriteLine("Give me the address you want to geocoding");
            Console.Write("> ");
            var input = Console.ReadLine();

            if (!string.IsNullOrEmpty(input))
            {
                var googleAddress = await GoogleMap.GeocodingAsync(input);
                if (googleAddress != null)
                {
                    Console.WriteLine($"Here the location: {googleAddress.Latitude},{googleAddress.Longitude}");
                    return true;
                }
            }

            return false;
        }

        static async Task<bool> ReverseGeocodingAsync()
        {
            Console.WriteLine("Give me the coordinates you want to reverse geocoding");
            Console.Write("> latitude: ");
            var latitude = Console.ReadLine();
            Console.Write("> longitude: ");
            var longitude = Console.ReadLine();

            if (!string.IsNullOrEmpty(latitude) && ! string.IsNullOrEmpty(longitude))
            {
                var googleAddress = await GoogleMap.ReverseGeocodingAsync(latitude, longitude);
                if (googleAddress != null)
                {
                    Console.WriteLine($"Here the address: {googleAddress.FormattedAddress}");
                    return true;
                }
            }

            return false;
        }
    }
}
