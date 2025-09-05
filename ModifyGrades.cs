using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Drawing2D;
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
            ///buttons
            button1.BackColor = Color.FromArgb(41, 128, 185);
            button1.ForeColor = Color.White;
            button2.BackColor = Color.FromArgb(231, 76, 60);
            button2.ForeColor = Color.White;
            button3.BackColor = Color.FromArgb(230, 126, 34);
            button3.ForeColor = Color.White;

            //round buttons
            /*RoundButton(button1, 20);
            RoundButton(button2, 20);
            RoundButton(button3, 20);*/

            ///panels
            panel2.BackColor = Color.FromArgb(164, 176, 190); // footer panel
            panel3.BackColor = Color.FromArgb(116, 125, 140); // line panel
            panel1.BackColor = Color.FromArgb(44, 62, 80);


            //for header
            dataGridView1.ColumnHeadersDefaultCellStyle.BackColor = Color.FromArgb(30, 30, 30); // dark color for header of datagridview
            dataGridView1.ColumnHeadersDefaultCellStyle.ForeColor = Color.White;
            dataGridView1.ColumnHeadersDefaultCellStyle.Font = new Font("Segoe UI", 10, FontStyle.Bold);

            //for rows
            dataGridView1.RowsDefaultCellStyle.BackColor = Color.White;
            dataGridView1.RowsDefaultCellStyle.ForeColor = Color.Black;
            dataGridView1.RowsDefaultCellStyle.Font = new Font("Segoe UI", 9);
            dataGridView1.AlternatingRowsDefaultCellStyle.BackColor = Color.FromArgb(240, 240, 240);
            dataGridView1.CellBorderStyle = DataGridViewCellBorderStyle.SingleHorizontal;
            dataGridView1.GridColor = Color.LightGray;

            //hover effect
            dataGridView1.DefaultCellStyle.SelectionBackColor = Color.FromArgb(0, 120, 215);
            dataGridView1.DefaultCellStyle.SelectionForeColor = Color.White;

            dataGridView1.DefaultCellStyle.Padding = new Padding(5);
            dataGridView1.ColumnHeadersDefaultCellStyle.Padding = new Padding(10, 5, 10, 5);

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
