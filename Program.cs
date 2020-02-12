using System.Threading.Tasks;
using System.Timers;
using Microsoft.AspNetCore.Blazor.Hosting;

namespace HSTempoWasm
{
    internal class Timers
    {
        internal static void StopAll()
        {
            if (beatTimer != null && beatTimer.Enabled)
            {
                beatTimer.Enabled = false;
                beatTimer.Stop();
                beatTimer.Close();
            }

            if (vdiTick != null)
            {
                vdiTick.Enabled = false;
                vdiTick.Close();
            }

            if (vdiTock != null)
            {
                vdiTock.Enabled = false;
                vdiTock.Close();
            }
        }

        internal static Timer beatTimer;
        internal static Timer vdiTick;
        internal static Timer vdiTock;
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