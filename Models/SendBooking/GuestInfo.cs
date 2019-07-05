using Newtonsoft.Json;

namespace SiestaCloud.WindingTreeConnector.Models.SendBooking
{
    public class GuestInfo
    {
        [JsonProperty(PropertyName = "id")]
        public string Id { get; set; }

        [JsonProperty(PropertyName = "name")]
        public string Name { get; set; }

        [JsonProperty(PropertyName = "surname")]
        public string Surname { get; set; }
    }
}
