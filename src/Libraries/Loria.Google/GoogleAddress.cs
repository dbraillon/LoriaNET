using Newtonsoft.Json;

namespace Loria.Google
{
    public class GoogleAddress
    {
        [JsonProperty(PropertyName = "formatted_address")]
        public string FormattedAddress { get; set; }

        [JsonProperty(PropertyName = "geometry")]
        public GoogleGeometry Geometry { get; set; }
    }
}
