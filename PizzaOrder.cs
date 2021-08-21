using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PapaDarioPizzaApp
{
    class PizzaOrder
    {
        public int OrderNumber { get; set; }
        public int SerialNumber { get; set; }
        public DateTime OrderDate { get; set; }
        public string CustomerId { get; set; }
        public string CustomerName { get; set; }
        public string IsDelivery { get; set; }
        public string PhoneNumber { get; set; }
        public string DeliveryAddress { get; set; }
        public string Size { get; set; }
        public string Toppings { get; set; }
        //public string[] ToppingsTest { get; set; }
        //public Dictionary<string, double> selectedToppings { get; set; }
        public double Price { get; set; }

        public PizzaOrder()
        {
            //selectedToppings = new Dictionary<string, double>();
            //ToppingsTest = new string[12];

        }

    }
}
