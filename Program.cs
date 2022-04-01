using System;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace framework
{
    public class Program
    {
        public static void Main(string[] args)
        {
            CreateHostBuilder(args).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args)
        {
            var hostBuilder = Host.CreateDefaultBuilder(args)
            .ConfigureServices((hostContext, services) => {
                services.AddScoped<IFakeService, FakeService>();
                services.AddHostedService<Worker>();
            });
            return hostBuilder;
        }
    }
}