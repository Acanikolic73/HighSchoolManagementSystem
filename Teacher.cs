using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FacultySystem
{
    public class Teacher : User
    {
        public int Age { get; set; }

        public string teacher_class;
        public Teacher(string first_name, string last_name, string Email, int Age, string teacher_class) : base(first_name, last_name, Email)
        {
            this.Age = Age;
            this.teacher_class = teacher_class;
        }

        public override string GetData()
        {
            string MainData = base.GetData();
            return MainData + " is teacher";
        }
    }
}
