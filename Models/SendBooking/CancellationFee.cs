using Newtonsoft.Json;

namespace SiestaCloud.WindingTreeConnector.Models.SendBooking
{
    public class CancellationFee
    {
        [JsonProperty(PropertyName = "from")]
        public string From { get; set; }

        [JsonProperty(PropertyName = "to")]
        public string To { get; set; }

        [JsonProperty(PropertyName = "amount")]
        public int Amount { get; set; }
    }
}
