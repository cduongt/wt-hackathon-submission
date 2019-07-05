using Newtonsoft.Json;

namespace SiestaCloud.WindingTreeConnector.Models.SendBooking
{
    public class Address
    {
        [JsonProperty(PropertyName = "line1")]
        public string Line1 { get; set; }

        [JsonProperty(PropertyName = "line2")]
        public string Line2 { get; set; }

        [JsonProperty(PropertyName = "city")]
        public string City { get; set; }

        [JsonProperty(PropertyName = "country")]
        public string Country { get; set; }

        [JsonProperty(PropertyName = "postalCode")]
        public string PostalCode { get; set; }

        [JsonProperty(PropertyName = "state")]
        public string State { get; set; }
    }
}
