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
    public partial class ModifyGrades : Form
    {

        DataRow StudentData;
        DataTable Student;
        DataTable dt;
        string subject, teacher_id;
        Class_Grade ParentForm;

        public void FillData()
        {

            string ID = StudentData.ItemArray[0].ToString();

            DataBaseConnection Connection = new DataBaseConnection();
            Connection.query = "select * from Grade g join Enrollment e on g.enrollment_id = e.enrollment_id join Subject s on e.subject_id = s.subject_id where g.student_id = '" + ID + "' and s.subject_id = '" + subject + "'";
            Student = Connection.Data().Tables[0];

            dt = new DataTable();
            dt.Columns.Add("Grade", typeof(string));
            dt.Columns.Add("Type", typeof(string));
            dt.Columns.Add("Date", typeof(string));

            int numGrades = Student.Rows.Count;
            for (int i = 0; i < numGrades; i++)
            {
                DataRow dr = Student.Rows[i];
                DataRow row = dt.NewRow();
                row["Grade"] = dr.ItemArray[1].ToString();
                row["Type"] = dr.ItemArray[3].ToString();
                row["Date"] = dr.ItemArray[2].ToString();
                dt.Rows.Add(row);
            }

            dataGridView1.DataSource = dt;

            ParentForm.FillData();
        }

        public ModifyGrades(string Subject, string Teacher_id, DataRow studentdata, DataTable student, Class_Grade Parent_form)
        {
            InitializeComponent();

            ParentForm = Parent_form; 
            subject = Subject;
            teacher_id = Teacher_id;
            StudentData = studentdata;
            Student = student;

            label1.Text = Name;

            FillData();

        }

        private void ModifyGrades_Load(object sender, EventArgs e)
        {

        }

        private void button2_Click(object sender, EventArgs e)
        {
            DeleteGrade f = new DeleteGrade(subject, StudentData, this);
            f.Show();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            ChangeGrade f = new ChangeGrade(subject, StudentData, this);
            f.Show();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            AddGrade f = new AddGrade(subject, teacher_id, StudentData, Student, this);
            f.Show();
        }
    }
}
