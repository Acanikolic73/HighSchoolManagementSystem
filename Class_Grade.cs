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
    public partial class Class_Grade : Form
    {

        DataTable dt; // datatable for datagrid view
        DataSet dataSet, SetOfStudents; // for each student grades
        string Teacher_id;
        string Subject, ClassGlobal, idGlobal;

        public void FillData()
        {

            /**/

            dt = new DataTable();

            dt.Columns.Add("Student", typeof(string));
            dt.Columns.Add("Grades", typeof(string));
            dt.Columns.Add("Average Grade", typeof(string));

            dataSet = new DataSet();

            DataBaseConnection db1 = new DataBaseConnection();
            db1.query = "select student_id, first_name, last_name from Student where class_id = '" + ClassGlobal + "'";
            SetOfStudents = db1.Data();

            int numOfStudents = SetOfStudents.Tables[0].Rows.Count;

            double AVG = 0; // average for whole class

            for (int i = 0; i < numOfStudents; i++)
            {
                DataRow row = SetOfStudents.Tables[0].Rows[i];
                string ID = row.ItemArray[0].ToString();
                string first_name = row.ItemArray[1].ToString();
                string last_name = row.ItemArray[2].ToString();
                string name = first_name + " " + last_name;

                DataBaseConnection db2 = new DataBaseConnection();
                db2.query = "select g.value from Grade g join Enrollment e on g.enrollment_id = e.enrollment_id join Subject s on e.subject_id = s.subject_id where g.student_id = '" + ID + "' and s.subject_id = '" + idGlobal + "'";

                DataSet Grades = db2.Data(); // dataset for grades for ith student

                int numGrades = Grades.Tables[0].Rows.Count;
                int sum = 0;

                string grades = "";

                for (int j = 0; j < numGrades; j++)
                {
                    DataRow dr = Grades.Tables[0].Rows[j];

                    string value = dr.ItemArray[0].ToString();

                    grades += value;
                    sum += int.Parse(value);

                    if (j + 1 < numGrades)
                    {
                        grades += ", ";
                    }
                }

                //asdijasodjas

                double avg = sum;
                if (numGrades > 0) avg /= numGrades;
                AVG += avg;
                if (numOfStudents > 0) AVG /= numOfStudents;

                // rounding avg on 2 decimals

                avg = Math.Round(avg, 2);
                AVG = Math.Round(AVG, 2);

                dt.Rows.Add(name, grades, avg.ToString());

                /*  for i-th student we need to store data in datatable and dataset  */

                DataBaseConnection ClassForm = new DataBaseConnection();
                ClassForm.query = "select * from Grade g join Enrollment e on g.enrollment_id = e.enrollment_id join Subject s on e.subject_id = s.subject_id where g.student_id = '" + ID + "' and s.subject_id = '" + idGlobal + "'";
                DataTable tmp = ClassForm.Data().Tables[0].Copy();
                dataSet.Tables.Add(tmp);
            }

            dataGridView1.DataSource = dt;
            label7.Text = AVG.ToString();

        }

        public Class_Grade(string teacher_id, string ss, string t, string subject, string Class, string id)
        {
            InitializeComponent();

            idGlobal = id;
            ClassGlobal = Class;
            Subject = ss;
            Teacher_id = teacher_id;
            dataSet = new DataSet();

            /* initializing datatable columns */
            dt = new DataTable();

            dt.Columns.Add("Student", typeof(string));
            dt.Columns.Add("Grades", typeof(string));
            dt.Columns.Add("Average Grade", typeof(string));

            // initializing datas

            label1.Text = Class;
            label3.Text = t;
            label5.Text = subject;

            /*  section label  */

            DataBaseConnection Section = new DataBaseConnection();
            Section.query = "select s.section from Section s join Class c on s.section = c.section and c.class_id = '" + Class + "'";
            DataSet SectionSet = Section.Data();
            if (SectionSet.Tables[0].Rows.Count > 0)
            {
                string sekcija = SectionSet.Tables[0].Rows[0].ItemArray[0].ToString();
                label8.Text = sekcija;
            }
            else
            {
                MessageBox.Show("It should never been happen");
            }

            /* first and last name of student that belong to that Class */

            FillData();

        }

        private void Class_Grade_Load(object sender, EventArgs e)
        {

        }

        private void dataGridView1_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0 || e.RowIndex >= dt.Rows.Count || e.ColumnIndex != 0)
            {
                MessageBox.Show("Click on students name");
                return;
            }
            DataTable Student = dataSet.Tables[e.RowIndex];
            DataRow row = SetOfStudents.Tables[0].Rows[e.RowIndex];
            string name = dt.Rows[e.RowIndex].ItemArray[0].ToString();
            ModifyGrades f = new ModifyGrades(Subject, Teacher_id, row, Student, this);
            f.Show();

        }
    }
}
