using System;
using System.Net.Http;
using System.Threading.Tasks;
using System.Timers;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Microsoft.Extensions.DependencyInjection;

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
            builder.Services.AddTransient(sp => new HttpClient { BaseAddress = new Uri(builder.HostEnvironment.BaseAddress) });
            await builder.Build().RunAsync();
        }

        // public static IWebAssemblyHostBuilder CreateHostBuilder(string[] args) =>
        //     BlazorWebAssemblyHost.CreateDefaultBuilder()
        //         .UseBlazorStartup<Startup>();
    }
}