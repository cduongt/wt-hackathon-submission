using System.Collections.Generic;
using Newtonsoft.Json;

namespace SiestaCloud.WindingTreeConnector.Models.SendBooking
{
    public class Room
    {
        [JsonProperty(PropertyName = "id")]
        public string Id { get; set; }

        [JsonProperty(PropertyName = "guestInfoIds")]
        public List<string> GuestInfoIds { get; set; } = new List<string>();
    }
}
