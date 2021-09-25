using System;
using System.Text.Json;
using System.Threading.Tasks;
using Blazored.LocalStorage;
using HostedBlazorWithFirebase.Client.Models;
using TrailBlazor.Models;

namespace HostedBlazorWithFirebase.Client.Services.Firebase
{
    public class FirebaseCache
    {
        private readonly ILocalStorageService localStorage;

        public FirebaseCache(ILocalStorageService localStorage)
        {
            this.localStorage = localStorage;
        }

        public async Task SetUser(FirebaseUser user)
        {
            //Console.WriteLine("Setting user in cache: " + JsonSerializer.Serialize(user));

            await localStorage.SetItemAsync("__firebaseUser", user);
        }

        public async Task<FirebaseUser> GetUser()
        {
            var r = await localStorage.GetItemAsync<FirebaseUser>("__firebaseUser");
            return r;
        }

        public async Task SetTokens(FirebaseIdTokenResult firebaseTokens)
        {
            //Console.WriteLine("Setting tokens in cache: " + JsonSerializer.Serialize(firebaseTokens));

            await localStorage.SetItemAsync("__firebaseTokens", firebaseTokens);
        }

        public async Task<FirebaseIdTokenResult> GetTokens()
        {
            var r = await localStorage.GetItemAsync<FirebaseIdTokenResult>("__firebaseTokens");
            return r;
        }

    }
}
