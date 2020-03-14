using System.Threading.Tasks;
using System.Timers;
using Microsoft.AspNetCore.Blazor.Hosting;

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

        internal static Timer BeatTimer;
        internal static Timer VdiTick;
        internal static Timer VdiTock;
    }

    public class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = WebAssemblyHostBuilder.CreateDefault(args);
            builder.RootComponents.Add<App>("app");
            await builder.Build().RunAsync();
        }

        // public static IWebAssemblyHostBuilder CreateHostBuilder(string[] args) =>
        //     BlazorWebAssemblyHost.CreateDefaultBuilder()
        //         .UseBlazorStartup<Startup>();
    }
}