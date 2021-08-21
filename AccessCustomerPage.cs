using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PapaDarioPizzaApp
{
    public static class AccessCustomerPage
    {
        public static void ChangeToLogOutButton(CustomerPage customerPage)
        {
            try
            {
                customerPage.btnLogIn.Content = "Log Out";
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
           
        }
    }
}
