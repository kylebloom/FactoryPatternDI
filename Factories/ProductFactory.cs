using FactoryPatternDI.Interfaces;
using FactoryPatternDI.Models.Products;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FactoryPatternDI.Factories
{
    public class ProductFactory(IServiceProvider serviceProvider) : IProductFactory
    {
        private readonly IServiceProvider _serviceProvider = serviceProvider;

        public async Task<IProduct> CreateProduct(string type)
        {

            return type.ToLower() switch
            {
                "a" => await GetWorker<ConcreteProductA>(),
                "b" => await GetWorker<ConcreteProductB>(),
                _ => throw new ArgumentException("Invalid product type"),
            };
        }

        private async Task<TWorker> GetWorker<TWorker>() where TWorker : class, IProduct
        {
            await Task.Delay(3000);
            if (_serviceProvider.GetService(serviceType: typeof(TWorker)) is not TWorker worker)
            {
                throw new ArgumentException($"Error in DI Setup. {typeof(TWorker)} was null.");
            }
            return worker;
        }
    }

    public interface IProductFactory
    {
        Task<IProduct> CreateProduct(string type);
    }
}
