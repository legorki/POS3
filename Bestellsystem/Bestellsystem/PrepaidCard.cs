using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bestellsystem
{
    internal class PrepaidCard : PaymentProvider
    {
        public decimal credit;

        public decimal Credit { get; }

        public PrepaidCard(decimal limit, decimal credit) : base(limit)
        {
            Credit = credit;
        }

        public override bool Pay(decimal amount)
        {
            if (amount > limit || amount > credit)
            {
                return false; 
            }
            credit -= amount; 
            return true; 
        }

        public void Charge(decimal amount)
        {
            credit += amount;
        }



    }
}