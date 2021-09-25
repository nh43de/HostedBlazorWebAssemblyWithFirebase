using System.Threading.Tasks;
using Microsoft.AspNetCore.Components.WebAssembly.Authentication;

namespace HostedBlazorWithFirebase.Client.Services.Firebase
{
    public class FirebaseTokenSource : IAccessTokenProvider
    {
        private readonly FirebaseJsProvider _firebaseJs;

        public FirebaseTokenSource(FirebaseJsProvider firebaseJs)
        {
            _firebaseJs = firebaseJs;
        }

        public async ValueTask<AccessTokenResult> RequestAccessToken()
        {
            //Console.WriteLine("Waiting for firebase init");

            //await _firebaseJs.InitTask;
            
            //Console.WriteLine("Completed waiting for firebase init");

            var token = await _firebaseJs.GetTokenInfo();

            if (token == null)
            {
                return new AccessTokenResult(AccessTokenResultStatus.RequiresRedirect, new AccessToken(), "/login");
            }
            else
            {
                var r = new AccessTokenResult(AccessTokenResultStatus.Success, new AccessToken() { Value = token.Token }, "/login");

                return r;
            }
        }

        public ValueTask<AccessTokenResult> RequestAccessToken(AccessTokenRequestOptions options)
        {
            return RequestAccessToken();
        }
    }
}
