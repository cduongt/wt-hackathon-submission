using Newtonsoft.Json;
using RestSharp;
using SiestaCloud.WindingTreeConnector.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SiestaCloud.WindingTreeConnector
{
    public static class WindingTreeSender
    {
        public static async Task<T> SendPostRequest<T>(string url, string action, string input, Dictionary<string, string> headers = null)
        {
            var client = new RestClient(url + action);
            var request = new RestRequest(Method.POST);

            request.AddJsonBody(input);
            throw new Exception(input + client.BaseUrl);

            if (headers != null)
            {
                foreach (var header in headers) request.AddHeader(header.Key, header.Value);
            }

            IRestResponse response = await client.ExecuteTaskAsync(request);

            if (response.IsSuccessful)
            {
                var retval = JsonConvert.DeserializeObject<T>(response.Content);
                return retval;
            }
            else
            {
                var error = JsonConvert.DeserializeObject<Error>(response.Content);

                if (error == null)
                {
                    throw new Exception("Call failed without further info.");
                }
                else
                {
                    throw new Exception(error.ToString());
                }
            }
        }

        public static async Task<T> SendGetRequest<T>(string url, string action, List<string> parameters, int limit = 50) where T : WindingTreeResponseBase
        {
            var client = new RestClient(url + action);
            var request = new RestRequest(Method.GET);

            request.AddQueryParameter("limit", limit.ToString());
            foreach (var parameter in parameters)
            {
                request.AddQueryParameter("fields", parameter);
            }

            IRestResponse response = await client.ExecuteTaskAsync(request);

            var retval = JsonConvert.DeserializeObject<T>(response.Content);

            // WT sends errors even if all the data is present. Commented out until further clarification.
            //if (retval.Errors != null && retval.Errors.Length > 0 && retval.Errors.Any(x => x.Status != 200))
            //{
            //    throw new WindingTreeException(retval.Errors.FirstOrDefault());
            //}

            return retval;
        }
    }
}