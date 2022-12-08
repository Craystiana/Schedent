using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;

namespace Schedent.API
{
    public static class Program
    {
        public static void Main(string[] args)
        {
            // Run the actions to be used by the host
            // Then run the application and block the calling thread until host shutdown
            CreateHostBuilder(args).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            // Initialize a new instance of the host builder with pre-configured defaults
            // Then configure the host builder with the defaults from startup
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    // Specify the startup type to be used by the web host
                    webBuilder.UseStartup<Startup>();
                });
    }
}
