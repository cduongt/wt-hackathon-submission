using System;

namespace SiestaCloud.WindingTreeConnector.Models.Hotel
{
    public class Item
    {
        public string Name { get; set; }

        public string Description { get; set; }

        public Location Location { get; set; }

        public WTAddress Address { get; set; }

        public WTRoomType[] RoomTypes { get; set; }

        public string Timezone { get; set; }

        public string Currency { get; set; }

        public string[] Images { get; set; }

        public string[] Amenities { get; set; }

        public DateTime UpdatedAt { get; set; }

        public string NotificationsUri { get; set; }

        public string ManagerAddress { get; set; }

        public string Id { get; set; }

        public string BookingUri { get; set; }
    }
}
