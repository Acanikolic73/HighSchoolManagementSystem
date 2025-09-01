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
        ModifyGrades ParentForm;

        public AddGrade(string subject, string teacher_id, DataRow student, DataTable grade, ModifyGrades parentForm)
        {
            InitializeComponent();

            ParentForm = parentForm;
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

        private void InsertEnrollment(string student_id)
        {
            DataBaseConnection Connection = new DataBaseConnection();
            Connection.query = $"insert into Enrollment (subject_id, student_id) values ('{Subject}', '{student_id}');";
            Connection.ExecuteNonQuery(Connection.query);
        }

        private string GetLastID()
        {
            //from Enrollment table
            DataBaseConnection Connection = new DataBaseConnection();
            Connection.query = "select max(enrollment_id) from Enrollment";
            DataSet ds = Connection.Data();
            if (ds.Tables[0].Rows.Count == 0)
                return "-1"; // when enrollment table is empty
            return ds.Tables[0].Rows[0].ItemArray[0].ToString(); 
        }

        private void InsertGrade(string enrollment_id, string student_id, string teacher_id, string grade, string type, string date)
        {
            DataBaseConnection Connection = new DataBaseConnection();
            Connection.query = $"insert into Grade (enrollment_id, value, [date], type, student_id, teacher_id) values ('{enrollment_id}', '{grade}', '{date}', '{type}', '{student_id}', '{teacher_id}');";
            Connection.ExecuteNonQuery(Connection.query);
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
            InsertEnrollment(studentid);
            DateTime d = DateTime.Now.Date;
            InsertGrade(GetLastID(), studentid, Teacher_id, grade.ToString(), type, d.ToString("dd.MM.yyyy"));
            ParentForm.FillData();
            MessageBox.Show("You successfully added a grade");
            this.Hide();
        }
    }
}
