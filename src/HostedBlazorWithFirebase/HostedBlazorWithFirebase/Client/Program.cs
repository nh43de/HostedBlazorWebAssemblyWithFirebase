using Microsoft.AspNetCore.Components.WebAssembly.Authentication;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Blazored.LocalStorage;
using HostedBlazorWithFirebase.Client.Services;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;

namespace HostedBlazorWithFirebase.Client
{

    public class Program
    {
        public static IServiceProvider Services { get; private set; }

        public static async Task Main(string[] args)
        {
            var builder = WebAssemblyHostBuilder.CreateDefault(args);
            builder.RootComponents.Add<App>("#app");

            builder.Services.AddHttpClient("HostedBlazorWithFirebase.ServerAPI", client => client.BaseAddress = new Uri(builder.HostEnvironment.BaseAddress))
                .AddHttpMessageHandler<FirebaseTokenMessageHandler>();

            builder.Services.AddAuthorizationCore();

            builder.Services.AddScoped<FirebaseTokenMessageHandler>();
            builder.Services.AddScoped<FirebaseCache>();
            builder.Services.AddScoped<FirebaseJsProvider>();
            builder.Services.AddScoped<ClientSideStateProvider>();
            builder.Services.AddBlazoredLocalStorage();
            builder.Services.AddScoped<AuthenticationStateProvider>(s => s.GetRequiredService<ClientSideStateProvider>());
            
            // Supply HttpClient instances that include access tokens when making requests to the server project
            builder.Services.AddScoped(sp => sp.GetRequiredService<IHttpClientFactory>().CreateClient("HostedBlazorWithFirebase.ServerAPI"));

            var host = builder.Build();

            Services = host.Services;

            await host.RunAsync();
        }
    }
}
