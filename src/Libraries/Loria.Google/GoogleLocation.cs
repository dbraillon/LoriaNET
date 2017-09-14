using Newtonsoft.Json;

namespace Loria.Google
{
    public class GoogleLocation
    {
        [JsonProperty(PropertyName = "lat")]
        public string Latitude { get; set; }

        [JsonProperty(PropertyName = "lng")]
        public string Longitude { get; set; }
    }
}
