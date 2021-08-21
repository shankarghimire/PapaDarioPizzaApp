using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PapaDarioPizzaApp
{
    class Pizza
    {
        public int ID { get; set; }
        public string PizzaSize { get; set; }
        public string Description { get; set; }
        public double Price { get; set; }
        public Pizza()
        {

        }
        public Pizza(int id, string pizzaSize, string description, double price)
        {
            try
            {
                this.ID = id;
                this.PizzaSize = pizzaSize;
                this.Description = description;
                this.Price = price;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            
        }
    }
}
