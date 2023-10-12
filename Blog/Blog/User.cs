using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Blog
{
    internal class User
    {
        public string Email { get; }
        public string Firstname { get; }
        public string Lastname { get; }

        public User(string email, string firstname, string lastname)
        {
            Email = email;
            Firstname = firstname;
            Lastname = lastname;
        }

    }
}
