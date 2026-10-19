using System.Text.Json.Serialization;

namespace DPBack.Application.Contracts.Payment.PayU
{
    public class PayUTokenReponseDto
    {   [JsonPropertyName("access_token")]
        public required string AccessToken { get; set; }
        [JsonPropertyName("expires_in")]
        public int ExpiresIn { get; set; }
    }
}