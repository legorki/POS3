using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bestellsystem
{
    internal class CreditCard : PaymentProvider
    {
        public string Number { get; }
        public CreditCard(decimal limit, string number) : base(limit){
            Number = number;
        
        }

        public override bool Pay(decimal amount)
        {
            if (amount > Limit)
            {
                return false; 
            }
            return true; 
        }
    }
}
