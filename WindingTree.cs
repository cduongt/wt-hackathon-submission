using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System.Threading;
using System.Collections.Generic;
using Google.Cloud.Dialogflow.V2;
using Google.Protobuf;
using Google.Protobuf.WellKnownTypes;
using SiestaCloud.Models;
using SiestaCloud.WindingTreeConnector;
using SiestaCloud.WindingTreeConnector.Models.Hotel;
using SiestaCloud.WindingTreeConnector.Models.SendBooking;
using System.Linq;

namespace SiestaCloud
{
    public static class WindingTree
    {
        private static readonly JsonParser jsonParser = new JsonParser(JsonParser.Settings.Default.WithIgnoreUnknownFields(true));

        internal enum RequestType
        {
            GetHotels,
            SendBooking,
            None
        }

        [FunctionName("WindingTree")]
        public static async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Function, "get", "post", Route = null)] HttpRequest req,
            ILogger log)
        {
            log.LogInformation("C# HTTP trigger function processed a request.");

            WebhookRequest request = null;

            string requestBody = await new StreamReader(req.Body).ReadToEndAsync();
            dynamic data = JsonConvert.DeserializeObject(requestBody);

            string responseJson = null;
            string userId = null;

            try
            {
                request = jsonParser.Parse<WebhookRequest>(requestBody);
            }
            catch (InvalidProtocolBufferException ex)
            {

                log.LogError(ex, "Web hook request could not be parsed.");
                return new BadRequestObjectResult("Error deserializing Dialog flow request from message body");
            }

            userId = GetUserId(request);
            string intent = request.QueryResult.Intent.DisplayName;
            RequestType requestType = GetRequestType(intent);
            WebhookResponse webHookResponse = null;
            var windingTreeConnector = new WindingTreeModule();

            switch (requestType)
            {
                case RequestType.GetHotels:
                    var city = request.QueryResult.Parameters.Fields["geo-city"].ToString().Trim('"');
                    var hotels = await windingTreeConnector.GetAllHotels();

                    var select = hotels.Items.Where(x => x.Address.City.Equals(city)).Select(x => x.Name).ToList(); 
                    string selectedHotels = "";
                    foreach (var hotel in select)
                    {
                        selectedHotels += $"{hotel}, ";
                    }
                    if(select.Count() == 0) {
                        webHookResponse = GetDialogFlowResponse(userId, $"Sorry, there are no available hotels in {city}.");
                    } else if(select.Count() == 1 ) {
                        webHookResponse = GetDialogFlowResponse(userId, $"Available hotel in {city} is {selectedHotels}. Do you wanna make reservation in {selectedHotels}?");
                    } else {
                        webHookResponse = GetDialogFlowResponse(userId, $"Available hotels in {city} are {selectedHotels}.");
                    }
                    break;
                case RequestType.SendBooking:
                    var from = Convert.ToDateTime(request.QueryResult.Parameters.Fields["checkin"].ToString().Trim('"'));
                    var to = Convert.ToDateTime(request.QueryResult.Parameters.Fields["checkout"].ToString().Trim('"'));
                    var response = await SendBooking("0x6036848A2C4c43e6EFAAF885615f8667569caA00", from, to);

                    webHookResponse = GetDialogFlowResponse(userId, $"I am making a reservation from {from.ToShortDateString()} to {to.ToShortDateString()} in Foreign Friend Guest House. Your reservation ID is {response.Id.ToString()}. I sent you details to your email.");
                    
                    break;
            }

            responseJson = webHookResponse?.ToString();
            log.LogCritical(responseJson);
            ContentResult contRes = new ContentResult()
            {
                Content = responseJson,
                ContentType = "application/json",
                StatusCode = 200
            };

            return contRes;
        }

        private static string GetUserId(WebhookRequest request)
        {

            string userId = null;
            Struct intentRequestPayload = request.OriginalDetectIntentRequest?.Payload;

/* 
            var userStruct = intentRequestPayload.Fields?["user"];

            string userStorageText = null;
            if ((userStruct?.StructValue?.Fields?.Keys.Contains("userStorage")).GetValueOrDefault(false))
            {
                userStorageText = userStruct?.StructValue?.Fields?["userStorage"]?.StringValue;
            }

            if (!string.IsNullOrWhiteSpace(userStorageText))
            {
                UserStorage userStore = JsonConvert.DeserializeObject<UserStorage>(userStorageText);

                userId  = userStore.UserId;
            }
            else
            {
                if ((userStruct?.StructValue?.Fields?.Keys.Contains("userId")).GetValueOrDefault(false))
                {
                    userId = userStruct?.StructValue?.Fields?["userId"]?.StringValue;
                }

                if (string.IsNullOrWhiteSpace(userId))
                {
                    // The user Id is not provided. Generate a new one and return it.
                    userId = Guid.NewGuid().ToString("N");

                }
            }
            */

            return userId;
        }

        private static RequestType GetRequestType(string intent)
        {
            if (intent.Equals("GetWindingTree", StringComparison.OrdinalIgnoreCase))
            {
                return RequestType.GetHotels;    
            }
            if (intent.Equals("Reservation", StringComparison.OrdinalIgnoreCase))
            {
                return RequestType.SendBooking;    
            }

            return RequestType.None;
        }

        private static WebhookResponse GetDialogFlowResponse(string userId, string message)
        {
            WebhookResponse webHookResp = message.StartsWith("Sorry") ? InitializeResponse(false, userId) : InitializeResponse(true, userId);

            var fulfillmentMessage = webHookResp.FulfillmentMessages[0];

            fulfillmentMessage.SimpleResponses = new Intent.Types.Message.Types.SimpleResponses();
            var simpleResp = new Intent.Types.Message.Types.SimpleResponse();
            simpleResp.Ssml = $"<speak>{message}</speak>";           
            fulfillmentMessage.SimpleResponses.SimpleResponses_.Add(simpleResp);

            return webHookResp;
        }

        private static WebhookResponse InitializeResponse(bool expectUserInput, string userId)
        {
            WebhookResponse webResp = new WebhookResponse();

            var message = new Intent.Types.Message();
            webResp.FulfillmentMessages.Add(message);
            message.Platform = Intent.Types.Message.Types.Platform.ActionsOnGoogle;

            // var payload = Struct.Parser.ParseJson("{\"google\": { \"expectUserResponse\": true}} ");


            //message.Payload = new
            Value payloadVal = new Value();
            payloadVal.StructValue = new Struct();

            Value expectedUserResp = new Value();
            expectedUserResp.BoolValue = expectUserInput;
            payloadVal.StructValue.Fields.Add("expectUserResponse", expectedUserResp);

            Value userStorageValue = new Value();

            UserStorage userStorage = new UserStorage();
            userStorage.UserId = userId;
            userStorageValue.StringValue = JsonConvert.SerializeObject(userStorage);

            payloadVal.StructValue.Fields.Add("userStorage", userStorageValue);

            Struct payloadStruct = new Struct();

            payloadStruct.Fields.Add("google", payloadVal);


            webResp.Payload = payloadStruct;




            return webResp;
        }

        private static async Task<SendBookingResponse> SendBooking(string hotelId, DateTime from, DateTime to)
        {

            var windingTreeModule = new WindingTreeModule();
            var hotels = await windingTreeModule.GetAllHotels();
            var hotel = hotels.Items.Where(x => x.Id == hotelId).FirstOrDefault();

            var booking = new SendBookingModel() 
            {
                HotelId = hotelId,
                Customer = CreateCustomer(),
                Pricing = CreatePricing(),
                Booking = CreateBooking(hotel, from, to)
            };

            return await windingTreeModule.SendBookingAsync(booking, hotel.BookingUri);
        }

        private static Customer CreateCustomer()
        {
            return new Customer()
            {
                Email = "david.duong@siesta.cloud",
                Phone = "+420123456789",
                Name = "David",
                Surname = "Duong",
                Address = CreateAddress()
            };
        }

        private static WTAddress CreateAddress()
        {
            return new WTAddress()
            {
                City = "Prague",
                CountryCode = "CZ",
                Road = "Za Humny",
                HouseNumber = "42",
                PostCode = "12345",
                State = ""
            };
        }

        private static Pricing CreatePricing()
        {
            return new Pricing()
            {
                Currency = "USD",
                Total = 500
            };
        }
        private static Booking CreateBooking(Item hotel, DateTime from, DateTime to)
        {
            return new Booking()
            {
                Arrival = from.ToShortDateString(),
                Departure = to.ToShortDateString(),
                GuestInfo = AddGuestInfo(),
                Rooms = AddRoomAsync(hotel)
            };
        }

        private static List<GuestInfo> AddGuestInfo()
        {
            var retval = new List<GuestInfo>
            {
                new GuestInfo()
                {
                    Id = "1",
                    Name = "David",
                    Surname = "Duong"
                }
            };

            return retval;
        }

        private static List<Room> AddRoomAsync(Item hotel)
        {
            var retval = new List<Room>
            {
                new Room()
                {
                    Id = hotel.RoomTypes[0].Id,
                    GuestInfoIds = new List<string>(){ "1" }
                }
            };

            return retval;
        }
    }
}
