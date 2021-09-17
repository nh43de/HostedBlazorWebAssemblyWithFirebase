
using System.Text.Json.Serialization;

namespace TrailBlazor.Models
{
    public class FirebaseUserTokens
    {
        [JsonPropertyName("Access_Token")]
        public string AccessToken { get; set; }
        [JsonPropertyName("Expires_In")]
        public string ExpiresIn { get; set; }
        [JsonPropertyName("Token_Type")]
        public string TokenType { get; set; }
        [JsonPropertyName("Refresh_Token")]
        public string RefreshToken { get; set; }
        [JsonPropertyName("Id_Token")]
        public string IdToken { get; set; }
    }
}
