using Newtonsoft.Json;
using System.Collections.Generic;

namespace Loria.Google
{
    public class GoogleResult
    {
        [JsonProperty(PropertyName = "status")]
        public string Status { get; set; }

        [JsonProperty(PropertyName = "results")]
        public IEnumerable<GoogleAddress> Addresses { get; set; }
    }
}
