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
            customerPage.btnLogIn.Content = "Log Out";
        }
    }
}
