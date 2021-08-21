using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PapaDarioPizzaApp
{
    public class Customer
    {
        public string UserId { get; set; }
        public string Password { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Phone { get; set; }
        public string Email { get; set; }
        public Customer() { }
        public Customer(string userId, string password, string firstName, string lastName, string phone, string email) {
            try
            {
                this.UserId = userId;
                this.Password = password;
                this.FirstName = firstName;
                this.LastName = lastName;
                this.Phone = phone;
                this.Email = email;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
           
        }
    }
}
