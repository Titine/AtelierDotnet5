using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Atelier_5.Model
{
    partial class Order
    {
        public decimal Amount
        {

            get
            {
                decimal total = Order_Details.Sum(cmddet => cmddet.Unit_Price * cmddet.Quantity * (decimal)cmddet.Discount);
                if (Freight.HasValue)
                {
                    return total + Freight.Value;
                }
                else
                    return total;

            }

        }
    }
}
