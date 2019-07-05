using System;
using System.Collections.Generic;

namespace SiestaCloud.WindingTreeConnector.Models.SendBooking
{
    public class Stay
    {
        public DateTime Arrival { get; set; }

        public double Subtotal { get; set; }

        public List<Guest> Guests { get; set; }
    }
}