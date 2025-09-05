using FacultySystem;
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
    public partial class StudentForm : Form
    {

        public Student Student;
        public StudentForm(Student student)
        {
            InitializeComponent();

            Student = student;

            if(Student.gender == "female")
            {
                pictureBox1.Image = Image.FromFile(@"C:\Users\Aleksandar\source\repos\HighSchoolManagement\Pictures\female_icon.png");
            }

            // filling data on form with student datas

            labelHead.Text = "Welcome " + Student.FirstName + " " + Student.LastName;
            labelFirstName.Text = Student.FirstName.ToString();
            labelLastName.Text = Student.LastName.ToString();
            labelClassName.Text = Student.class_name.ToString();
            labelBirthDate.Text = Student.Date_of_birth.ToString("dd.MM.yyyy");


            /// store subject in datagridview, just name of subject
            DataBaseConnection db = new DataBaseConnection();
            db.query = "select s.subject_id, s.name from Section sec join Subject s on sec.subject_id = s.subject_id";


            /// getting grades for each subject

            DataTable dataTable = new DataTable();
            dataTable.Columns.Add("Subject", typeof(string));
            dataTable.Columns.Add("Grades", typeof(string));
            dataTable.Columns.Add("Average", typeof(double));

            DataSet ds = db.Data();
            int numSubjects = ds.Tables[0].Rows.Count;
            double AVG = 0;
            for (int i = 0; i < numSubjects; i++)
            {
                DataRow row = ds.Tables[0].Rows[i];
                string id = row.ItemArray[0].ToString();
                string name = row.ItemArray[1].ToString();

                /* for Subject named (name) getting datas from SQL query */

                DataBaseConnection dataBaseConnection = new DataBaseConnection();
                dataBaseConnection.query = "select value from Grade g join Enrollment e on g.enrollment_id = e.enrollment_id join subject s on s.subject_id = e.subject_id where s.subject_id = '" + id + "'";
                string Grades = ""; // list of grades representing with string 
                DataSet GradesSet = dataBaseConnection.Data();
                int numGrades = GradesSet.Tables[0].Rows.Count;

                //variables for average grade
                double sum = 0, avg;

                for (int j = 0; j < numGrades; j++)
                {
                    DataRow red = GradesSet.Tables[0].Rows[j];
                    string grade = red.ItemArray[0].ToString();
                    Grades += grade;
                    sum += int.Parse(grade);
                    if (j + 1 < numGrades)
                    {
                        // adding , if it is not the last grade
                        Grades += ", ";
                    }
                }
                avg = sum / numGrades;

                // rounding avg on 2 decimals

                avg = Math.Round(avg, 2);
                AVG += avg;

                /* filling datagrid view with datas and storing in datatable */

                dataTable.Rows.Add(name, Grades, avg);
            }

            AVG /= numSubjects;
            labelEmail.Text = AVG.ToString();
            dataGridView1.DataSource = dataTable;

        }
        private void StudentForm_Load(object sender, EventArgs e)
        {
            //labels
            labelHead.ForeColor = Color.White;

            // panels
            panel4.BackColor = Color.FromArgb(44, 62, 80);
            panel2.BackColor = Color.FromArgb(164, 176, 190); // footer panel
            panel3.BackColor = Color.FromArgb(116, 125, 140); // line panel




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

        private void label2_Click(object sender, EventArgs e)
        {

        }

        private void labelClassName_Click(object sender, EventArgs e)
        {

        }
    }
}
