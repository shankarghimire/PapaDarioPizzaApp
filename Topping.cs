using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PapaDarioPizzaApp
{
    class Topping
    {
        public int ID { get; set; }
        public string ToppingName { get; set; }
        public string ToppingDescription { get; set; }
        public double ToppingPrice { get; set; }
        public Topping()
        {

        }
        public Topping(int id, string toppingName, string toppingDescription, double toppingPrice)
        {
            try
            {
                this.ID = id;
                this.ToppingName = toppingName;
                this.ToppingDescription = toppingDescription;
                this.ToppingPrice = toppingPrice;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
           
        }
    }
}
