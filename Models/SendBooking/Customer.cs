using Newtonsoft.Json;
using SiestaCloud.WindingTreeConnector.Models.Hotel;

namespace SiestaCloud.WindingTreeConnector.Models.SendBooking
{
    public class Customer
    {
        [JsonProperty(PropertyName = "name")]
        public string Name { get; set; }

        [JsonProperty(PropertyName = "surname")]
        public string Surname { get; set; }

        [JsonProperty(PropertyName = "address")]
        public WTAddress Address { get; set; }

        [JsonProperty(PropertyName = "email")]
        public string Email { get; set; }

        [JsonProperty(PropertyName = "phone")]
        public string Phone { get; set; }
    }
}