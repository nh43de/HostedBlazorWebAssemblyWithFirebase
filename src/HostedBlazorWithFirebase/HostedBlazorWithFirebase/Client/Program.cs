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
using HostedBlazorWithFirebase.Client.Services.Firebase;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;

namespace HostedBlazorWithFirebase.Client
{

    public class Program
    {
        public static IServiceProvider Services { get; private set; }

        public static void AddFirebaseAuthServices(IServiceCollection services)
        {
            services.AddScoped<IAccessTokenProvider, FirebaseTokenSource>();

            services.AddAuthorizationCore();

            services.AddScoped<FirebaseTokenMessageHandler>();
            services.AddScoped<FirebaseCache>();
            services.AddScoped<FirebaseJsProvider>();
            services.AddScoped<FirebaseClientSideStateProvider>();
            services.AddBlazoredLocalStorage();
            services.AddScoped<AuthenticationStateProvider>(s => s.GetRequiredService<FirebaseClientSideStateProvider>());
        }

        public static async Task Main(string[] args)
        {
            var builder = WebAssemblyHostBuilder.CreateDefault(args);
            builder.RootComponents.Add<App>("#app");

            AddFirebaseAuthServices(builder.Services);

            builder.Services.AddHttpClient("HostedBlazorWithFirebase.Authenticated", client => client.BaseAddress = new Uri(builder.HostEnvironment.BaseAddress))
                //.AddHttpMessageHandler<BaseAddressAuthorizationMessageHandler>();
                .AddHttpMessageHandler<FirebaseTokenMessageHandler>();
            
            // Supply HttpClient instances that include access tokens when making requests to the server project
            builder.Services.AddScoped(sp => sp.GetRequiredService<IHttpClientFactory>().CreateClient("HostedBlazorWithFirebase.Authenticated"));

            var host = builder.Build();

            Services = host.Services;

            await host.RunAsync();
        }
    }
}
