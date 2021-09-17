using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Security.Claims;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.WebAssembly.Authentication;

namespace HostedBlazorWithFirebase.Client.Services
{
    public class ClientSideStateProvider : AuthenticationStateProvider
    {
        private readonly FirebaseJsProvider _firebase;
        
        public ClientSideStateProvider(FirebaseJsProvider firebase)
        {
            _firebase = firebase;
        }

        public override async Task<AuthenticationState> GetAuthenticationStateAsync()
        {
            var identity = new ClaimsIdentity();
            try
            {
                var isAuthenticated = await _firebase.IsUserLoggedIn();

                if (isAuthenticated)
                {
                    var user = await _firebase.GetUser();
                    var tokenInfo = await _firebase.GetTokenInfo();

                    if (user == null || tokenInfo == null)
                        return new AuthenticationState(new ClaimsPrincipal(new ClaimsIdentity()));

                    Console.WriteLine("Auth state got user: " + JsonSerializer.Serialize(user));
                    Console.WriteLine("Auth state got token: " + JsonSerializer.Serialize(tokenInfo));

                    var claims = new List<Claim>
                    {
                        new Claim(ClaimTypes.Name, user.Email),
                        new Claim(ClaimTypes.Role, "Basic User")
                    };

                    foreach (var (key, value) in tokenInfo.Claims)
                    {
                        var r = value?.ToString();

                        if(r == null)
                            continue;
                        
                        claims.Add(new Claim(key, r));
                    }

                    identity = new ClaimsIdentity(claims, "authentication");

                    return await Task.FromResult(new AuthenticationState(new ClaimsPrincipal(identity)));
                }
            }
            catch (HttpRequestException ex)
            {
                Console.WriteLine("Request failed:" + ex.ToString());
            }

            var authState = new AuthenticationState(new ClaimsPrincipal(identity));
            return authState;
        }

        public void ManageUser()
        { 
            NotifyAuthenticationStateChanged(GetAuthenticationStateAsync());
        }
    }
}