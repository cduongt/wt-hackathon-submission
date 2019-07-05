using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace SiestaCloud.WindingTreeConnector.Models.Hotel
{
    public class Description
    {
        public Location Location { get; set; }

        public string Name { get; set; }

        [JsonProperty(PropertyName = "description")]
        public string HotelDescription { get; set; }

        public List<WTRoomType> RoomTypes { get; set; }

        public WTAddress Address { get; set; }

        public string Timezone { get; set; }

        public string Currency { get; set; }

        public List<string> SpokenLanguages { get; set; }

        public string Category { get; set; }

        public List<string> Images { get; set; }

        public List<string> Amenities { get; set; }

        public DateTime UpdatedAt { get; set; }

        public decimal DefaultCancellationAmount { get; set; }
    }
}
