using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BL
{
    public class UserLogin
    {
        public List<User> Users { get; set; }

        public UserLogin()
        {
            // one librarian and one customer
            Users = new List<User>
            {
                new User("Librarian" , "1111"),
                new User("Customer" , "2222"){IsCust = true}
            };
        }
        public User GetUserByNameAndPassword(string name, string password) =>
            Users.FirstOrDefault(u => u.UserName.Equals(name) && u.Password.Equals(password));
    }
}
