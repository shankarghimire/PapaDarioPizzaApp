using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PapaDarioPizzaApp
{
    public class OrderDetails
    {
        public int SN { get; set; }
        public int OrderId { get; set; }
        public string PizzaSize { get; set; }
        public string Toppings { get; set; }

        public OrderDetails() { }
        public OrderDetails(int sn,int orderid, string pizzaSize, string toppings)
        {
            this.SN = sn;
            this.OrderId = orderid;
            this.PizzaSize = pizzaSize;
            this.Toppings = toppings;
        }

    }
}
