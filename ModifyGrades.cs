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

        public ModifyGrades(string Subject, string Teacher_id, DataRow studentdata, DataTable student)
        {
            InitializeComponent();

            subject = Subject;
            teacher_id = Teacher_id;
            StudentData = studentdata;
            Student = student;

            label1.Text = Name;
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

        }

        private void ModifyGrades_Load(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            AddGrade f = new AddGrade(subject, teacher_id, StudentData, Student);
            f.Show();
        }
    }
}
