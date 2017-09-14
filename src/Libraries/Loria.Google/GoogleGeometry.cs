using Newtonsoft.Json;

namespace Loria.Google
{
    public class GoogleGeometry
    {
        [JsonProperty(PropertyName = "location")]
        public GoogleLocation Location { get; set; }
    }
}
