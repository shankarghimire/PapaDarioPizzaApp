using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PapaDarioPizzaApp
{
    public class User
    {
        public string UserId { get; set; }
        public string UserName { get; set; }
        private string UserPassword { get; set; }
       
       
        public User() { }
        public User(string userId, string userName, string userPassword)
        {
            this.UserId = userId;
            this.UserName = userName;
            this.UserPassword = userPassword;
        }
    }
}
