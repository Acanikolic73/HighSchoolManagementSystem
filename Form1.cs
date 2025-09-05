using FacultySystem;
using Microsoft.Win32.SafeHandles;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace HighSchoolManagement
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
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
                    string teacher_id = dr.ItemArray[0].ToString();
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
                    ProfessorForm f = new ProfessorForm(teacher_id, teacher);
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

        private void Form1_Load(object sender, EventArgs e)
        {
            //set color for components
            panel4.BackColor = Color.FromArgb(44, 62, 80);
            label2.ForeColor = Color.FromArgb(47, 54, 64); // email
            label3.ForeColor = Color.FromArgb(47, 54, 64); // password
            label4.ForeColor = Color.FromArgb(74, 144, 226); // forgot password
            button1.BackColor = Color.FromArgb(74, 144, 226); // login button
            panel2.BackColor = Color.FromArgb(164, 176, 190); // footer panel
            panel3.BackColor = Color.FromArgb(116, 125, 140); // line panel
            //label5.ForeColor = Color.White; // copyright label

            //set text for email and password labels
            textBox1.Text = "Email";
            textBox2.UseSystemPasswordChar = false;
            textBox2.PasswordChar = '\0';
            textBox2.Text = "Password";
        }

        private void button1_Click_1(object sender, EventArgs e)
        {
            CheckLogin();
        }

        private void label2_Click(object sender, EventArgs e)
        {

        }

        bool shownEmail = false, shownPassword = false;

        private void textBox1_Enter(object sender, EventArgs e)
        {
            if(!shownEmail)
            {
                shownEmail = true;
                textBox1.Text = "";
            }
        }

        private void textBox2_Enter(object sender, EventArgs e)
        {
            if(!shownPassword)
            {
                shownPassword = true;
                textBox2.UseSystemPasswordChar = true;
                textBox2.PasswordChar = '.';
                textBox2.Text = "";
            }
        }

        private void textBox2_Leave(object sender, EventArgs e)
        {
            if (textBox2.Text != "") return;
            shownPassword = false;
            textBox2.UseSystemPasswordChar = false;
            textBox2.PasswordChar = '\0';
            textBox2.Text = "Password";
        }

        bool showPassword = false;

        private void pictureBox3_Click(object sender, EventArgs e)
        {
            if (textBox1.Text == "Password") return;
            if(showPassword)
            {
                textBox2.UseSystemPasswordChar = true;
                textBox2.PasswordChar = '.';
                pictureBox3.Image = Image.FromFile(@"C:\Users\Aleksandar\source\repos\HighSchoolManagement\Pictures\show_password.png");
                showPassword = false;
            }
            else
            {
                textBox2.UseSystemPasswordChar = false;
                textBox2.PasswordChar = '\0';
                pictureBox3.Image = Image.FromFile(@"C:\Users\Aleksandar\source\repos\HighSchoolManagement\Pictures\hide_password.png");
                showPassword = true;
            }
        }

        private void textBox1_Leave(object sender, EventArgs e)
        {
            if (textBox1.Text != "") return;
            shownEmail = false;
            textBox1.Text = "Email";
        }
    }
}
