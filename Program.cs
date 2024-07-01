using FactoryPatternDI.Factories;
using FactoryPatternDI.Models.Products;
using Microsoft.Extensions.DependencyInjection;

namespace FactoryPatternDI
{
    internal class Program
    {
        static async Task Main(string[] args)
        {
            // Setup DI
            var serviceProvider = new ServiceCollection()
                .AddTransient<ConcreteProductA>()
                .AddTransient<ConcreteProductB>()
                .AddSingleton<IProductFactory, ProductFactory>()
                .BuildServiceProvider();

            var factory = serviceProvider.GetService<IProductFactory>();
            if (factory == null)
            {
                Console.WriteLine("Error in setting up DI. Exiting program. Press any key to exit.");
                Console.ReadLine();
                return;
            }

            while (true)
            {
                Console.WriteLine("Enter type of product to do work:");
                var type = Console.ReadLine();
                if (string.IsNullOrEmpty(type))
                {
                    Console.WriteLine("invalid entry. press any key to continue.");
                    Console.ReadLine();
                    continue;
                }
                var worker = await factory.CreateProduct(type);
                worker.DoWork();
                Console.WriteLine("Press any key to continue.");
                Console.ReadLine();
            }
        }
    }
}
