using Microsoft.Extensions.Hosting;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using SiestaCloud.WindingTreeConnector.Models;
using SiestaCloud.WindingTreeConnector.Models.Hotel;
using SiestaCloud.WindingTreeConnector.Models.SendBooking;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SiestaCloud.WindingTreeConnector
{
    public class WindingTreeModule
    {
        private const string READ_URL = "https://playground-api.windingtree.com";

        private const string HOTELS = "/hotels";
        private const string CREATE_BOOKING = "/booking";

        public WindingTreeModule()
        {
            JsonConvert.DefaultSettings = () => new JsonSerializerSettings
            {
                Formatting = Formatting.Indented,
                ContractResolver = new CamelCasePropertyNamesContractResolver()
            };
        }

        public async Task<GetHotelsModel> GetAllHotels()
        {
            var parameters = new List<string>
            {
                "id",
                "name",
                "location",
                "address",
                "roomTypes",
                "currency",
                "bookingUri",
                "ratePlans",
                "availability"
            };

            var hotels = await WindingTreeSender.SendGetRequest<GetHotelsModel>(READ_URL, HOTELS, parameters);

            return hotels;
        }

        public async Task<SendBookingResponse> SendBookingAsync(SendBookingModel sendBookingModel, string bookingUri, string messageHash = "")
        {
            /* var headers = new Dictionary<string, string>()
            {
                { "X-Message-Hash", messageHash }
            };*/

            string input = JsonConvert.SerializeObject(sendBookingModel);

            return await WindingTreeSender.SendPostRequest<SendBookingResponse>(bookingUri, CREATE_BOOKING, input);
        }
    }
}