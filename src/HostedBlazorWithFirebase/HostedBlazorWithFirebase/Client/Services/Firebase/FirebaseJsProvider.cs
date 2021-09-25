using System;
using System.Collections.Generic;
using System.Text.Json;
using System.Threading.Tasks;
using HostedBlazorWithFirebase.Client.Models;
using Microsoft.JSInterop;
using TrailBlazor.Models;

namespace HostedBlazorWithFirebase.Client.Services.Firebase
{
    /// <summary>
    /// C# wrapper around the firebase.js library for Blazor
    /// </summary>
    public class FirebaseJsProvider
    {
        public bool Init { get; set; }
        private readonly FirebaseCache _firebaseCache;

        public IJSRuntime JS { get; set; }

        public FirebaseJsProvider(IJSRuntime js, FirebaseCache firebaseCache)
        {
            _firebaseCache = firebaseCache;
            JS = js;
        }

        //TODO: implement oauth-only popup flow
        //await JSRuntime.InvokeAsync<object>("FirebaseLoginOauth", DotNetObjectReference.Create(this));
        #region jsInterop

        public void SetInitialized()
        {
            Init = true;
        }

        #endregion
        #region methods

        public async Task SignOut()
        {
            var r = await JS.InvokeAsync<bool>("firebaseSignOut");



        }

        public async Task<string> UserSignIn(string email, string password)
        {
            var r = await JS.InvokeAsync<string>("firebaseEmailSignIn", email, password);

            return r;
        }

        public async Task CreateUser(string registerRequestUserName, string registerRequestPassword)
        {
            var r = await JS.InvokeAsync<string>("firebaseCreateUser", registerRequestUserName, registerRequestPassword);
        }

        #endregion


        #region helpers

        public async Task<Dictionary<string, object>> GetUserClaims()
        {
            var tokenInfo = await GetTokenInfo();

            return tokenInfo.Claims;
        }

        public async Task<bool> IsUserLoggedIn()
        {
            var u = await GetUser();

            if (u == null)
                return false;

            return true;
        }

        #endregion


        #region cachables

        public async Task<FirebaseUserTokens> GetRefreshTokens(string refreshToken)
        {
            var response = await JS.InvokeAsync<string>("getRefreshToken", refreshToken);
            var firebaseUserTokens = JsonSerializer.Deserialize<FirebaseUserTokens>(response);

            return firebaseUserTokens;
        }

        public async Task<string> GetIdToken()
        {
            var r = await GetTokenInfo();
            return r.Token;
        }

        public async Task<FirebaseIdTokenResult> GetTokenInfo()
        {
            var response = await JS.InvokeAsync<string>("firebaseGetIdTokenResult");

            //Console.WriteLine("Got token from firebase: " + response);

            if (response == null)
            {
                //try to get from cache -- this is because there is a lag for loading and may return null on page refresh
                var cachedTokens = await _firebaseCache.GetTokens();

                //Console.WriteLine("Got token from cache: " + cachedTokens);

                return cachedTokens;
            }

            var firebaseUserTokens = JsonSerializer.Deserialize<FirebaseIdTokenResult>(response);

            return firebaseUserTokens;
        }

        /// <summary>
        /// Gets user - can be from cache.
        /// </summary>
        /// <param name="force"></param>
        /// <returns></returns>
        public async Task<FirebaseUser> GetUser(bool force = true)
        {
            var userJson = await JS.InvokeAsync<string>("firebaseGetCurrentUser");

            //Console.WriteLine(userJson);

            if (userJson == null)
            {
                //try get from cache
                var cachedUser = await _firebaseCache.GetUser();

                return cachedUser;
            }

            var user = JsonSerializer.Deserialize<FirebaseUser>(userJson, new JsonSerializerOptions() { PropertyNameCaseInsensitive = true });

            //Console.WriteLine(user);

            return user;
        }

        #endregion


    }
}