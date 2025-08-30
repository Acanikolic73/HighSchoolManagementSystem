using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Globalization;


namespace FacultySystem
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void toolStripButton1_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void panel1_Paint(object sender, PaintEventArgs e)
        {

        }

        private void Form1_Load(object sender, EventArgs e)
        {
            label2.ForeColor = Color.FromArgb(40, 44, 52);
            label5.ForeColor = Color.FromArgb(0, 120, 215);
            button1.BackColor = Color.FromArgb(0, 120, 215);
            button1.ForeColor = Color.White;
            button1.FlatAppearance.BorderSize = 0;

        }

        private void CheckLogin()
        {
            if (textBox1.Text == "" || textBox2.Text == "")
            {
                MessageBox.Show("You need to fill the empty field");
                return;
            }
            string Email = textBox1.Text;
            string Password = textBox2.Text;
            DataBaseConnection conStudent = new DataBaseConnection();
            DataBaseConnection conTeacher = new DataBaseConnection();
            conStudent.query = "select * from Student where email = '" + Email + "'";
            conTeacher.query = "select * from Teacher where email = '" + Email + "'";
            DataSet dsStudent = conStudent.Data();
            DataSet dsTeacher = conTeacher.Data();
            //check whether user a student 
            if (dsStudent.Tables[0].Rows.Count > 0)
            {
                DataRow dr = dsStudent.Tables[0].Rows[0];
                if (dr.ItemArray[4].ToString() == Password)
                {
                    // getting student data for student atributes
                    string first_name = dr.ItemArray[1].ToString();
                    string last_name = dr.ItemArray[2].ToString();
                    string email = dr.ItemArray[3].ToString();
                    string birth = dr.ItemArray[5].ToString();
                    DateTime birth_date = DateTime.ParseExact(birth, "yyyy-M-d", CultureInfo.InvariantCulture);
                    string gender = dr.ItemArray[6].ToString();

                    //getting class name by id
                    string id = dr.ItemArray[7].ToString();
                    DataBaseConnection ClassName = new DataBaseConnection();
                    ClassName.query = "select class_id from Class where class_id = '" + id + "'";
                    DataSet Set = ClassName.Data();
                    if (Set.Tables[0].Rows.Count == 1)
                    {
                        // we have class name and we can show student form 
                        DataRow red = Set.Tables[0].Rows[0];
                        string name = red.ItemArray[0].ToString();
                        Student student = new Student(first_name, last_name, email, birth_date, gender, name);

                        StudentForm f = new StudentForm(student);
                        f.Show();
                        this.Hide();
                    }
                    else
                    {
                        // it should never been happen
                        MessageBox.Show("Something wrong?");
                    }
                }
                else
                {
                    MessageBox.Show("Password is incorrect");
                }
                return;
            }
            //check whether user a teacher
            if (dsTeacher.Tables[0].Rows.Count > 0)
            {
                DataRow dr = dsTeacher.Tables[0].Rows[0];
                if (dr.ItemArray[4].ToString() == Password)
                {
                    //getting teachers data
                    string first_name = dr.ItemArray[1].ToString();
                    string last_name = dr.ItemArray[2].ToString();
                    string email = dr.ItemArray[3].ToString();
                    int Age = int.Parse(dr.ItemArray[5].ToString());
                    string teacher_class = "n/a"; // base case if teacher does not have a class

                    //checking does teacher have a class where he is a head teacher
                    int id = int.Parse(dr.ItemArray[0].ToString());
                    DataBaseConnection dc = new DataBaseConnection();
                    dc.query = "select class_id from Class where head_teacher_id = '" + id + "'";
                    DataSet dataSet = dc.Data();
                    if (dataSet.Tables[0].Rows.Count > 0)
                    {
                        // teacher have a class where he is a head teacher
                        DataRow dataRow = dataSet.Tables[0].Rows[0];
                        teacher_class = dataRow.ItemArray[0].ToString();
                    }

                    Teacher teacher = new Teacher(first_name, last_name, email, Age, teacher_class);
                    ProfessorForm f = new ProfessorForm(teacher);
                    f.Show();
                    this.Hide();
                }
                else
                {
                    MessageBox.Show("Password is incorrect");
                }
                return;
            }
            MessageBox.Show("Email does not exist in database");
        }

        private void button1_Click(object sender, EventArgs e)
        {
            CheckLogin();
        }
    }
}
