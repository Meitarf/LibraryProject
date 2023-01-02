using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BL
{
    // class user w/ username & password
    // if is customer - IsCust = true
    public class User
    {
        public string UserName { get; set; }
        public string Password { get; set; }
        public bool IsCust;
        public User(string name, string password)
        {
            UserName = name;
            Password = password;
        }
    }
}
