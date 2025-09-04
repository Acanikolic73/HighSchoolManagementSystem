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
    public partial class ProfessorForm : Form
    {

        public Teacher teacher;

        DataTable dataTable; // we maintain dataTable as global structures because we need it for doubleclick event

        DataBaseConnection dataBaseConnection;

        string Teacher_id;
        List<string> Ids;

        public ProfessorForm(string teacher_id, Teacher t)
        {
            InitializeComponent();

            Ids = new List<string>();
            Teacher_id = teacher_id; 
            teacher = t;

            //filling the fields(labels) with teachers data

            label_head.Text = "Welcome teacher, " + teacher.FirstName + " " + teacher.LastName;
            labelFirst_name.Text = teacher.FirstName;
            label_last_name.Text = teacher.LastName;
            labelClass.Text = teacher.teacher_class;
            labelAge.Text = teacher.Age.ToString();
            //labelEmail.Text = teacher.Email;

            // part of code for showing subject that the teacher teaches

            dataBaseConnection = new DataBaseConnection();
            dataBaseConnection.query = "select s.subject_id, s.name from Subject s join Teacher t on s.teacher_id = t.teacher_id";
            DataSet dataSet = dataBaseConnection.Data();
            int numOfSubjects = dataSet.Tables[0].Rows.Count;

            /* Initializing datatable for store data  */

            dataTable = new DataTable();
            dataTable.Columns.Add("Subject", typeof(string));
            dataTable.Columns.Add("Class", typeof(string));

            for (int i = 0; i < numOfSubjects; i++)
            {
                /* getting each classes where teacher teaches */
                DataRow dr = dataSet.Tables[0].Rows[i];
                string id = dr.ItemArray[0].ToString();
                string name = dr.ItemArray[1].ToString();

                DataBaseConnection db = new DataBaseConnection();
                db.query = "select c.class_id from Class c join Section s on c.section = s.section join Subject sub on sub.subject_id = s.subject_id where sub.subject_id = '" + id + "'";

                DataSet ds = db.Data();
                int numOfClass = ds.Tables[0].Rows.Count;
                for (int j = 0; j < numOfClass; j++)
                {
                    DataRow row = ds.Tables[0].Rows[j];
                    dataTable.Rows.Add(name, row.ItemArray[0].ToString());
                    Ids.Add(id);
                }

            }

            /* Completing datagrid view */

            dataGridView1.DataSource = dataTable;

        }

        private void ProfessorForm_Load(object sender, EventArgs e)
        {
            ///panels
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

        private void dataGridView1_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0 || e.ColumnIndex != 0 || e.RowIndex >= dataTable.Rows.Count)
            {
                MessageBox.Show("You double-clicked invalid cell");
                return;
            }

            // cell (e.rowIndex, 0)
            DataSet ds = dataBaseConnection.Data();
            string id = ds.Tables[0].Rows[e.RowIndex].ItemArray[0].ToString();
            DataRow dr = dataTable.Rows[e.RowIndex];
            string subject = dr.ItemArray[0].ToString();
            string Class = dr.ItemArray[1].ToString();
            string t = teacher.FirstName + " " + teacher.LastName;

            Class_Grade f = new Class_Grade(Teacher_id, Ids[e.RowIndex], t, subject, Class, id);
            f.Show();
            this.Hide();
        }
    }
}
