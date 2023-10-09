using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bestellsystem
{
    internal class Order
    {

        private readonly List<Product> products = new List<Product>();
        private readonly PaymentProvider paymentProvider;

        public IReadOnlyList<Product> Products => new ReadOnlyCollection<Product>(products);
        public decimal InvoiceAmount => CalculateInvoiceAmount();

        public Order(PaymentProvider paymentProvider)
        {
            this.paymentProvider = paymentProvider;
        }

        public void AddProduct(Product product)
        {
            products.Add(product);
        }

        public bool Checkout()
        {
            if (paymentProvider.Pay(InvoiceAmount))
            {
                products.Clear();
                return true;
            }
            return false;
        }

        private decimal CalculateInvoiceAmount()
        {
            decimal totalAmount = 0;
            foreach (var product in products)
            {
                totalAmount += product.Price;
            }
            return totalAmount;
        }


    }
}
