using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml.Linq;

namespace HighSchoolManagement
{
    public partial class DeleteGrade : Form
    {

        string Subject;
        DataRow Student;
        ModifyGrades ParentForm;

        private void FillData()
        { 

            string ID = Student.ItemArray[0].ToString();
            string name = Student.ItemArray[1].ToString() + Student.ItemArray[2].ToString();
            label1.Text = name;

            DataBaseConnection Con = new DataBaseConnection();
            Con.query = "select g.value, g.type, g.date from Grade g join Enrollment e on g.enrollment_id = e.enrollment_id join Subject s on e.subject_id = s.subject_id where g.student_id = '" + ID + "' and s.subject_id = '" + Subject + "'";
            DataSet Set = Con.Data();
            dataGridView1.DataSource = Set.Tables[0];
            ParentForm.FillData();
        }

        public DeleteGrade(string subject, DataRow dr, ModifyGrades parentForm)
        {
            InitializeComponent();

            Subject = subject;
            Student = dr;
            ParentForm = parentForm;

            FillData();
        }

        private int GetEnrollmentID(int Row)
        {
            string ID = Student.ItemArray[0].ToString();
            string name = Student.ItemArray[1].ToString() + Student.ItemArray[2].ToString();
            label1.Text = name;

            DataBaseConnection Con = new DataBaseConnection();
            Con.query = "select * from Grade g join Enrollment e on g.enrollment_id = e.enrollment_id join Subject s on e.subject_id = s.subject_id where g.student_id = '" + ID + "' and s.subject_id = '" + Subject + "'";
            DataSet Set = Con.Data();
            return int.Parse(Set.Tables[0].Rows[Row].ItemArray[0].ToString());
        }

        private void DeleteInGrade(int id)
        {
            DataBaseConnection Con = new DataBaseConnection();
            Con.query = "delete from Grade where enrollment_id = '" + id + "'";
            Con.ExecuteNonQuery(Con.query);
        }

        private void DeleteInEnrollment(int id)
        {
            DataBaseConnection Con = new DataBaseConnection();
            Con.query = "delete from Enrollment where enrollment_id = '" + id + "'";
            Con.ExecuteNonQuery(Con.query);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if(dataGridView1.SelectedRows.Count == 0)
            {
                MessageBox.Show("You need to select row of grade which you want to delete");
                return;
            }
            if(dataGridView1.SelectedRows.Count > 1)
            {
                MessageBox.Show("You need to selected one row not multiple");
                return;
            }
            int selectedRow = dataGridView1.SelectedRows[0].Index;
            int EnrollmentID = GetEnrollmentID(selectedRow);
            DeleteInGrade(EnrollmentID);
            DeleteInEnrollment(EnrollmentID);
            FillData();
            MessageBox.Show("You successfully deleted a grade");
            this.Hide();
        }

        private void DeleteGrade_Load(object sender, EventArgs e)
        {
        }
    }
}
