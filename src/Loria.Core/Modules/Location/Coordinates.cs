using Loria.Google;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace LoriaNET.Location
{
    /// <summary>
    /// Defines latitude and longitude.
    /// </summary>
    public class Coordinates
    {
        public string Latitude { get; set; }
        public string Longitude { get; set; }
        public string Address { get; set; }

        public Coordinates(string str)
        {
            var splitted = str.Split(',');
            if (splitted.Length < 2) throw new ArgumentException("Coordinate should defines latitude and longitude", nameof(str));

            Latitude = splitted.ElementAt(0);
            Longitude = splitted.ElementAt(1);
        }

        public async Task ReverseGeocodingAsync(GoogleMap googleMap)
        {
            if (Address == null)
            {
                var googleAddress = await googleMap.ReverseGeocodingAsync(Latitude, Longitude);
                if (googleAddress != null)
                {
                    Address = googleAddress.FormattedAddress;
                }
            }
        }

        public override string ToString()
        {
            return Address != null ? Address : $"{Latitude},{Longitude}";
        }
    }
}
