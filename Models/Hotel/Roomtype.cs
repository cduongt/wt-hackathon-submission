using System;
using System.Collections.Generic;

namespace SiestaCloud.WindingTreeConnector.Models.Hotel
{
    public class WTRoomType
    {
        public string Name { get; set; }

        public string Description { get; set; }

        public int TotalQuantity { get; set; }

        public List<string> Amenities { get; set; }

        public List<string> Images { get; set; }

        public string Id { get; set; }

        public DateTime UpdatedAt { get; set; }
    }
}
