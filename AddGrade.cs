using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace HighSchoolManagement
{
    public partial class AddGrade : Form
    {

        DataRow Student;
        DataTable Grade;
        string Subject, Teacher_id;

        public AddGrade(string subject, string teacher_id, DataRow student, DataTable grade)
        {
            InitializeComponent();

            Subject = subject;
            Teacher_id = teacher_id;
            Student = student;
            Grade = grade;
            string name = Student.ItemArray[1].ToString() + " " + Student.ItemArray[2].ToString();
            label1.Text = name;
        }

        private void AddGrade_Load(object sender, EventArgs e)
        {
        }

        private void button1_Click(object sender, EventArgs e)
        {
            int grade = int.Parse(numericUpDown1.Value.ToString());
            string type = comboBox1.Text;
            if(grade < 1 || grade > 5)
            {
                MessageBox.Show("Grade is invalid, it has to be [1, 5]");
                return;
            }
            if(comboBox1.SelectedIndex == -1)
            {
                MessageBox.Show("You need to choose type of grade");
                return;
            }
            string studentid = Student.ItemArray[0].ToString();
            string gradeid = Grade.Rows[0].ItemArray[0].ToString();

        }
    }
}
