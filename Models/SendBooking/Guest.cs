namespace SiestaCloud.WindingTreeConnector.Models.SendBooking
{
    public class Guest
    {
        public string GuestId { get; set; }

        public string RatePlanId { get; set; }

        public double BasePrice { get; set; }

        public double ResultingPrice { get; set; }
    }
}