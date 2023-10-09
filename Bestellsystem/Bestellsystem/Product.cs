using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bestellsystem
{
    internal class Product 
    {

        public string Ean {  get; }
        public string Name { get;  }
        public decimal Price { get; }

        public Product(string ean, string name, decimal price) {
            Ean = ean;
            Name = name;    
            Price = price;
        }
    }
}
