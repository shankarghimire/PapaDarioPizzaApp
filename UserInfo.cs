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
        //public User GetLoggedInUser()
        //{
        //    User temp = new User();
        //    temp.UserId = 12345;
        //    temp.UserName = User;

        //    return temp;
        //}


    }
}
