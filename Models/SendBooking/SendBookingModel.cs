using Newtonsoft.Json;

namespace SiestaCloud.WindingTreeConnector.Models.SendBooking
{
    public class SendBookingModel
    {
        [JsonProperty(PropertyName = "hotelId")]
        public string HotelId { get; set; }

        [JsonProperty(PropertyName = "customer")]
        public Customer Customer { get; set; }

        [JsonProperty(PropertyName = "pricing")]
        public Pricing Pricing { get; set; }

        [JsonProperty(PropertyName = "booking")]
        public Booking Booking { get; set; }

        public string Origin { get; set; }

        public string Note { get; set; }
    }
}