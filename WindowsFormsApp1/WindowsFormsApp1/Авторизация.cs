using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WindowsFormsApp1
{
    public partial class Авторизация : Form
    {
        static string dbname = @"KOMPUTER\SQLEXPRESS";
        public static SqlConnection sqlcon = new SqlConnection(@"Data Source=" + dbname + ";Initial Catalog=SysTest;Integrated Security=True");
        public static string la = "";

        public Авторизация()
        {
            
            InitializeComponent();
        }
        // Авторизация
        private void button1_Click(object sender, EventArgs e)
        {
            Авторизация.la = getLogin();
            var password = getPassowrd();

            sqlcon.Open();
            try
            {
                SqlDataReader dataReader = new SqlCommand($"SELECT * FROM Пользователь WHERE Пользователь.Логин = '{la}'", sqlcon).ExecuteReader();
                dataReader.Read();
                if (!(dataReader.GetString(1).Equals(password)))
                {
                    dataReader.Close();
                    throw new Exception("wrong pass");
                }

                switch (dataReader.GetValue(3))
                {
                    case 1:
                        dataReader.Close();
                        sqlcon.Close();
                        ActiveForm.Hide();
                        new Администратор().ShowDialog();
                        break;
                    case 2:
                        dataReader.Close();
                        sqlcon.Close();
                        ActiveForm.Hide();
                        new Преподаватель().ShowDialog();
                        break;
                    case 3:
                        dataReader.Close();
                        sqlcon.Close();
                        ActiveForm.Hide();
                        new Выбор_теста().ShowDialog();
                        break;
                }

            }
            catch (Exception)
            {
                MessageBox.Show("Данные для входа не верны");
                sqlcon.Close();
                return;
            }
            Close();
        }   
        
        private string getLogin()
        {
            return textBox1.Text;
        }
        
        private string getPassowrd()
        {
            // TODO: err if pass is nil
            return maskedTextBox1.Text;
        }
    }
}
