using FactoryPatternDI.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FactoryPatternDI.Models.Products
{
    public class ConcreteProductA : IProduct
    {
        public void DoWork()
        {
            Console.WriteLine("ConcreteProductA is doing work.");
        }
    }
}
