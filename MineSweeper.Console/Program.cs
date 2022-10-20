using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using MineSweeper.Business;
using MineSweeper.Business.Contracts;
using MineSweeper.Business.Services;

namespace MineSweeper.Console
{
    internal class Program
    {
        static void Main(string[] args)
        {
            var services = ConfigureServices();
            var serviceProvider = services.BuildServiceProvider();

            // Start the app.
            var handler = serviceProvider.GetService<MineSweeperHandler>();
            
            if(handler != null)
            { 
                handler.Run();
            }
        }

        private static IServiceCollection ConfigureServices()
        {
            // Setup dependency injection.
            IServiceCollection services = new ServiceCollection();

            services.AddLogging(configure => configure.AddConsole())
                .AddTransient<MineSweeperService>();

            // Register dependencies.
            services.AddTransient<IMineSweeperService, Business.Services.MineSweeperService>();
            services.AddTransient<MineSweeperHandler>();

            return services;
        }
    }
}