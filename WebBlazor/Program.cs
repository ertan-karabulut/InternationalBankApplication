using Blazored.LocalStorage;
using Blazored.SessionStorage;
using Fluxor;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using WebBlazor.Services.Abstrack;
using WebBlazor.Services.Concreate;

namespace WebBlazor
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = WebAssemblyHostBuilder.CreateDefault(args);
            builder.RootComponents.Add<App>("#app");

            builder.Services.AddBlazoredLocalStorage();
            builder.Services.AddBlazoredSessionStorage();
            builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri(builder.HostEnvironment.BaseAddress) });
            builder.Services.AddScoped<IServiceOperations, ServiceOperations>();
            builder.Services.AddFluxor(x =>
            {
                x.ScanAssemblies(typeof(Program).Assembly)
                .UseReduxDevTools();
            });
            await builder.Build().RunAsync();
        }
    }
}
