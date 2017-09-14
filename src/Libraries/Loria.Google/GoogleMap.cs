using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace Loria.Google
{
    public class GoogleMap
    {
        public string ApiKey { get; set; }
        public bool IsConfigured { get; set; }

        public GoogleMap(string apiKey)
        {
            ApiKey = apiKey;

            // TODO: Better test
            IsConfigured = ApiKey != null;
        }

        public async Task<GoogleAddress> ReverseGeocodingAsync(string latitude, string longitude)
        {
            var url = $"https://maps.googleapis.com/maps/api/geocode/json?latlng={latitude},{longitude}&key={ApiKey}";

            using (var httpClient = new HttpClient())
            {
                var response = await httpClient.GetAsync(url);
                if (response.IsSuccessStatusCode)
                {
                    var result = await response.Content.ReadAsAsync<GoogleResult>();
                    if (result.Status == "OK")
                    {
                        return result.Addresses?.FirstOrDefault();
                    }
                }
            }

            return null;
        }

        public async Task<GoogleLocation> GeocodingAsync(string address)
        {
            var url = $"https://maps.googleapis.com/maps/api/geocode/json?address={address}&key={ApiKey}";

            using (var httpClient = new HttpClient())
            {
                var response = await httpClient.GetAsync(url);
                if (response.IsSuccessStatusCode)
                {
                    var result = await response.Content.ReadAsAsync<GoogleResult>();
                    if (result.Status == "OK")
                    {
                        return result.Addresses?.FirstOrDefault()?.Geometry?.Location;
                    }
                }
            }

            return null;
        }
    }
}
