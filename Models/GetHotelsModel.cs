using SiestaCloud.WindingTreeConnector.Models.Hotel;

namespace SiestaCloud.WindingTreeConnector.Models
{
    public class GetHotelsModel: WindingTreeResponseBase
    {
        public Item[] Items { get; set; }
    }
}