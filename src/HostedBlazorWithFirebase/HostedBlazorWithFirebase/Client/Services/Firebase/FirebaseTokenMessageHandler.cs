using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading;
using System.Threading.Tasks;
using HostedBlazorWithFirebase.Client.Services;
using Microsoft.AspNetCore.Components.WebAssembly.Authentication;

namespace HostedBlazorWithFirebase.Client
{
    /// <summary>
    /// Attaches the firebase token to outgoing requests.
    /// </summary>
    public class FirebaseTokenMessageHandler : DelegatingHandler
    {
        private readonly FirebaseJsProvider _firebaseJs;
        
        public FirebaseTokenMessageHandler(FirebaseJsProvider firebaseJs)
        {
            _firebaseJs = firebaseJs;
        }
        
        protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            await _firebaseJs.InitTask;

            var token = await _firebaseJs.GetIdToken();

            var header = new AuthenticationHeaderValue("Bearer", token);
            request.Headers.Authorization = header;
            
            return await base.SendAsync(request, cancellationToken);
        }
    }
}
