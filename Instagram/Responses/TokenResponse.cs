using Newtonsoft.Json;

namespace Instagram.Responses
{
    public class TokenResponse
    {
        [JsonProperty("access_token")]
        public string AccessToken { get; set; }
    }
}
