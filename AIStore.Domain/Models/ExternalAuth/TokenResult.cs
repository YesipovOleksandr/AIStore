using Newtonsoft.Json;

namespace AIStore.Domain.Models.ExternalAuth
{
    public class TokenResult
    {
        [JsonProperty("access_token")]
        public string AccessToken { get; set; }

        [JsonProperty("refresh_token")]
        public string RefreshToken { get; set; }

        [JsonProperty("expires")]
        public DateTime Expires { get; set; }
    }
}
