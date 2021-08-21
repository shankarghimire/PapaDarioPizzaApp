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
            try
            {
                UserId = userId;
                UserName = userName;
                LoginStatus = true;
            }
            catch(Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            //User user = new User();
            //LoggedInUser.UserId = userId;
            //LoggedInUser.UserName = userName;
           
            return LoggedInUser;
        }
        public static User DefaultUserInfo()
        {
            try
            {
                UserId = "Guest";
                UserName = "Guest";
                LoginStatus = false;
            }
            catch(Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
           
            return LoggedInUser;
        }

        public static void ChangeToLogOut(CustomerPage page)
        {
            try
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
            catch(Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            
        }
    }
}
