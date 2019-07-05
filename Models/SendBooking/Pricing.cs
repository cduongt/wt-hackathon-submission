using System.Collections.Generic;
using Newtonsoft.Json;

namespace SiestaCloud.WindingTreeConnector.Models.SendBooking
{
    public class Pricing
    {
        [JsonProperty(PropertyName = "currency")]
        public string Currency { get; set; }

        [JsonProperty(PropertyName = "total")]
        public decimal Total { get; set; }

        [JsonProperty(PropertyName = "cancellationFees")]
        public List<CancellationFee> CancellationFees { get; set; } = new List<CancellationFee>();

        public PricingComponent Components { get; set; }
    }
}
