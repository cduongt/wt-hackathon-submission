using Newtonsoft.Json;

namespace SiestaCloud.Models
{
    [JsonObject]
    public class UserStorage
    {

        [JsonProperty(PropertyName = "userId")]
        public string UserId { get; set; }
    }
}