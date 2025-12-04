using System;
using System.Net.Http;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Text;
using System.Timers;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Fluxor;
using Fluxor.Blazor.Web.ReduxDevTools;

namespace HSTempoWasm
{
    internal class Timers
    {
        internal static void StopAll()
        {
            if (BeatTimer != null && BeatTimer.Enabled)
            {
                BeatTimer.Enabled = false;
                BeatTimer.Stop();
                BeatTimer.Close();
            }

            if (VdiTick != null)
            {
                VdiTick.Enabled = false;
                VdiTick.Close();
            }

            if (VdiTock != null)
            {
                VdiTock.Enabled = false;
                VdiTock.Close();
            }
        }
        
        public static Timer BeatTimer;
        public static Timer VdiTock;
        public static Timer VdiTick;
    }

    public class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = WebAssemblyHostBuilder.CreateDefault(args);
            builder.RootComponents.Add<App>("app");
            
            // Use a trusted base address from the host environment
            var trustedBaseAddress = builder.HostEnvironment.BaseAddress;
            if (!Uri.TryCreate(trustedBaseAddress, UriKind.Absolute, out var baseUri))
            {
                throw new InvalidOperationException("Invalid base address");
            }
            builder.Services.AddScoped(sp => new HttpClient { BaseAddress = baseUri });
            builder.Services.AddLogging(builder => builder
                .SetMinimumLevel(LogLevel.Information)
            );
            var currentAssembly = typeof(Program).Assembly;
            builder.Services.AddFluxor(options => options.ScanAssemblies(currentAssembly).UseReduxDevTools(rdt => rdt.Name = "HSTempoWasm"));
            
            await builder.Build().RunAsync();
        }

        // public static IWebAssemblyHostBuilder CreateHostBuilder(string[] args) =>
        //     BlazorWebAssemblyHost.CreateDefaultBuilder()
        //         .UseBlazorStartup<Startup>();
    }
}