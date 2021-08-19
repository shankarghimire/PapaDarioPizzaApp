using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PapaDarioPizzaApp
{
    public class UserInfo
    {
        private User LoggedInUser { get; set; } 
        public UserInfo()
        {
            LoggedInUser = new User();
        }


    }
}
