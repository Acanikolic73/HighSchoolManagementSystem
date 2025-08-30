using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FacultySystem
{
    public class Student : User
    {
        public DateTime Date_of_birth { get; set; }

        public string gender { get; set; }

        public string class_name { get; set; }

        public Student(string first_name, string last_name, string Email, DateTime date, string gender, string class_name) : base(first_name, last_name, Email)
        {
            this.Date_of_birth = date;
            this.gender = gender;
            this.class_name = class_name;
        }

        public override string GetData()
        {
            string MainData = base.GetData();
            return MainData + " is student";
        }
    }
}
