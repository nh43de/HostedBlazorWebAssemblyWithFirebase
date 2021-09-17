using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace HostedBlazorWithFirebase.Client.Models
{
    public class FirebaseIdTokenResult
    {
        [JsonPropertyName("authTime")]
        public string AuthTime { get; set; }
        
        [JsonPropertyName("claims")]
        public Dictionary<string, object> Claims { get; set; }
        
        [JsonPropertyName("expirationTime")]
        public string ExpirationTime { get; set; }

        [JsonPropertyName("issuedAtTime")]
        public string IssuedAtTime { get; set; }

        [JsonPropertyName("signInProvider")]
        public string SignInProvider { get; set; }

        [JsonPropertyName("signInSecondFactor")]
        public string SignInSecondFactor { get; set; }

        [JsonPropertyName("token")]
        public string Token { get; set; }
    }
}
