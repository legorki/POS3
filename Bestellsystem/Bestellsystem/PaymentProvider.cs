using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bestellsystem
{
    internal abstract class PaymentProvider
    {
        protected decimal limit;
        public decimal Limit { get;  }
        protected PaymentProvider(decimal limit) {
            Limit = limit;
        }

        public virtual bool Pay(decimal amount) {
            if (amount > limit)
            {
                return false;
            }
            else
            {
                return true;
            }
        }
    }
}
