using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PapaDarioPizzaApp
{
    public class Order
    {
        public int OrderId { get; set; }
        public DateTime OrderDate { get; set; }
        public string CustomerId { get; set; }
        public string PhoneNumber { get; set; }
        public string IsDelivery { get; set; }
        public string DeliveryAddress { get; set; }
        public double TotalPrice { get; set; }
        public double Discount { get; set; }
        public double GrandTotal { get; set; }
        public Order() { }
        public Order(int orderId,DateTime orderDate, string customerId,string phoneNumber, string isDelivery, string deliveryAddress, double totalPrice, double discount, double grandTotal)
        {
            try
            {
                this.OrderId = orderId;
                this.OrderDate = orderDate;
                this.CustomerId = customerId;
                this.PhoneNumber = phoneNumber;
                this.IsDelivery = isDelivery;
                this.DeliveryAddress = deliveryAddress;
                this.TotalPrice = totalPrice;
                this.Discount = discount;
                this.GrandTotal = grandTotal;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            
        }
    }
}
