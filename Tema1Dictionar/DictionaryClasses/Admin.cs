using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tema1Dictionar
{
    public class Admin
    {
        public string Email {  get; set; }
        public string Password { get; set; }
        public Admin(string email, string password)
        {
            Email = email;
            Password = password;
        }
    }
}
