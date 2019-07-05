using System.Collections.Generic;
using Newtonsoft.Json;

namespace SiestaCloud.WindingTreeConnector.Models.SendBooking
{
    public class Booking
    {
        [JsonProperty(PropertyName = "arrival")]
        public string Arrival { get; set; }

        [JsonProperty(PropertyName = "departure")]
        public string Departure { get; set; }

        [JsonProperty(PropertyName = "rooms")]
        public List<Room> Rooms { get; set; } = new List<Room>();

        [JsonProperty(PropertyName = "guestInfo")]
        public List<GuestInfo> GuestInfo { get; set; } = new List<GuestInfo>();
    }
}
