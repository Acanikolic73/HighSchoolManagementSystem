using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FacultySystem
{
    public class User
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }

        public string Email { get; set; }
        public User(string first_name, string last_name, string Email)
        {
            this.FirstName = first_name;
            this.LastName = last_name;
            this.Email = Email;
        }

        public virtual string GetData()
        {
            return FirstName + " " + LastName + " ";
        }
    }
}
