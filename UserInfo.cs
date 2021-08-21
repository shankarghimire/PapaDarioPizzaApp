using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PapaDarioPizzaApp
{
    public  class UserInfo
    {
        public static User LoggedInUser  { get; set; }
        public static bool LoginStatus{get;set;}
        public static string UserId { get; set; }
        public static string UserName { get; set; }

        public  UserInfo()
        {
            //LoggedInUser = new User();
            LoggedInUser = new User();
            LoggedInUser.UserId = "Guest";
            LoggedInUser.UserName = "Guest";
            LoginStatus = false;
        }
        public static User GetLoggedInUser(string userId, string userName)
        {
            //User user = new User();
            //LoggedInUser.UserId = userId;
            //LoggedInUser.UserName = userName;
            UserId = userId;
            UserName = userName;
            LoginStatus = true;
            return LoggedInUser;
        }
        public static User DefaultUserInfo()
        {
            UserId = "Guest";
            UserName = "Guest";
            LoginStatus = false;
            return LoggedInUser;
        }

        public static void ChangeToLogOut(CustomerPage page)
        {
            
            if (LoginStatus == true)
            {
                page.btnLogIn.Content = "Lot Out";
            }
            else
            {
                page.btnLogIn.Content = "Lot In";
            }
        }
    }
}
