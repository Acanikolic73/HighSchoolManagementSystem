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
    public partial class ChangeGrade : Form
    {

        string Subject;
        DataRow Student;
        ModifyGrades ParentForm;

        private void FillData()
        {

            string ID = Student.ItemArray[0].ToString();
            string name = Student.ItemArray[1].ToString() + " " + Student.ItemArray[2].ToString();

            label1.Text = name;

            DataBaseConnection Con = new DataBaseConnection();
            Con.query = "select g.value, g.type, g.date from Grade g join Enrollment e on g.enrollment_id = e.enrollment_id join Subject s on e.subject_id = s.subject_id where g.student_id = '" + ID + "' and s.subject_id = '" + Subject + "'";
            DataSet Set = Con.Data();
            dataGridView1.DataSource = Set.Tables[0];
            ParentForm.FillData();
        }

        public ChangeGrade(string subject, DataRow student, ModifyGrades parentForm)
        {
            InitializeComponent();

            Subject = subject;
            Student = student; 
            ParentForm = parentForm;

            FillData();
        }

        private void ChangeGrade_Load(object sender, EventArgs e)
        {

        }

        int GetEnrollmentID(int Row)
        {
            string ID = Student.ItemArray[0].ToString();
            DataBaseConnection Con = new DataBaseConnection();
            Con.query = "select * from Grade g join Enrollment e on g.enrollment_id = e.enrollment_id join Subject s on e.subject_id = s.subject_id where g.student_id = '" + ID + "' and s.subject_id = '" + Subject + "'";
            DataSet Set = Con.Data();
            return int.Parse(Set.Tables[0].Rows[Row].ItemArray[0].ToString());
        }

        private void Update(int grade, string type, int EnrollmentID)
        {
            DataBaseConnection Con = new DataBaseConnection();
            Con.query = "update Grade set value = '" + grade + "', type = '" + type + "' where enrollment_id = '" + EnrollmentID + "'"; 
            Con.ExecuteNonQuery(Con.query);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if(dataGridView1.SelectedRows.Count == 0)
            {
                MessageBox.Show("You need to select row of grade that you want to change");
                return;
            }
            if(dataGridView1.SelectedRows.Count > 1)
            {
                MessageBox.Show("You need to select exactly one row");
                return;
            }
            int grade = (int)numericUpDown1.Value;
            string type = comboBox1.Text;
            if(grade < 1 || grade > 5)
            {
                MessageBox.Show("You choose a invalid grade");
                return;
            }
            if(comboBox1.SelectedIndex == -1)
            {
                MessageBox.Show("You need to choose a type of the grade");
                return;
            }
            int selectedRow = dataGridView1.SelectedRows[0].Index;
            Update(grade, type, GetEnrollmentID(selectedRow));
            FillData();
            MessageBox.Show("You updated grade successfully");
            this.Hide();
        }
    }
}
