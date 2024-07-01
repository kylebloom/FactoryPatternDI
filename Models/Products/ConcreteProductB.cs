using FactoryPatternDI.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FactoryPatternDI.Models.Products
{
    public class ConcreteProductB : IProduct
    {
        public void DoWork()
        {
            Console.WriteLine("ConcreteProductB is doing work.");
        }
    }
}
