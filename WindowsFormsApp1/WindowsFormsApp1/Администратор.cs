using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Net.Mail;
using System.Net;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.ListView;

namespace WindowsFormsApp1
{
    public partial class Администратор : Form
    {
        public Администратор()
        {
            InitializeComponent();
        }

        private void Администратор_Load(object sender, EventArgs e)
        {
            
            // TODO: данная строка кода позволяет загрузить данные в таблицу "sysTestDataSet.Должность". При необходимости она может быть перемещена или удалена.
            this.должностьTableAdapter.Fill(this.sysTestDataSet.Должность);
            updDb();
        }
        // Заполнение и обновление таблицы пользователей
        private void updDb()
        {
            DataSet ds = new DataSet();
            Авторизация.sqlcon.Open();
            new SqlDataAdapter(
                @"Select Пользователь.Логин, Пользователь.ФИО, Должность.Наименование, Пользователь.Почта 
                from Пользователь 
                join Должность on  Пользователь.id_Должности = Должность.id", Авторизация.sqlcon).Fill(ds);
            dataGridView1.DataSource = ds.Tables[0];
            dataGridView1.Columns[2].HeaderText = "Должность";
            Авторизация.sqlcon.Close();
        }
        // Проверка на заполненность всех текстовых полей
        private bool checkTb()
        {
            foreach (TextBox tb in Controls.OfType<TextBox>())
                if (!(tb.Text.Length > 0))
                    return true;
            return false;
        }
        // Проверка на правильное заполнение поля с почтой
        private bool checkEmail(string eMail)
        {
            var regex = @"\A(?:[a-z0-9!#$%&'*+\/=?^_`{|}~-]+(?:\.[a-z0-9!#$%&'*+\/=?^_`{|}~-]+)*@(?:[a-z0-9](?:[a-z0-9-]*[a-z0-9])?\.)+[a-z0-9](?:[a-z0-9-]*[a-z0-9])?)\Z";
            return Regex.IsMatch(eMail, regex, RegexOptions.IgnoreCase);
        }
        // Регистрация
        private void button1_Click(object sender, EventArgs e)
        {
            if (checkTb() || comboBox1.SelectedValue.ToString().Length<1)
            {
                MessageBox.Show("Заполните все поля");
                return;
            }
            if (!checkEmail(textBox4.Text))
            {
                MessageBox.Show("Введите корректную почту");
                return;
            }
            Авторизация.sqlcon.Open();
            try
            {
                new SqlDataAdapter(
                $@"INSERT INTO Пользователь(Логин, Пароль, ФИО, id_должности, Почта)
                VALUES('{textBox1.Text}','{textBox2.Text}','{textBox3.Text}',{comboBox1.SelectedValue},'{textBox4.Text}')",
                Авторизация.sqlcon).
                Fill(new DataSet());
                MessageBox.Show("Регистрация прошла успешно!");
                Авторизация.sqlcon.Close();
            }
            catch (Exception)
            {
                MessageBox.Show("Данный логин уже существует!");
                Авторизация.sqlcon.Close();
                return;
            }
            updDb();
            sendToEmail();
            MessageBox.Show("Данные для авторизации высланы на почту!");
        }

        private void sendToEmail()
        {
            string eMail, login, pass,fio;
            login = textBox1.Text;
            pass = textBox2.Text;
            fio = textBox3.Text;
            eMail = textBox4.Text;

            // отправитель - устанавливаем адрес и отображаемое в письме имя
            MailAddress from = new MailAddress("SysTestUp11@yandex.ru", "Система тестирования");
            // кому отправляем
            MailAddress to = new MailAddress(eMail);
            // создаем объект сообщения
            MailMessage m = new MailMessage(from, to);
            // тема письма
            m.Subject = $"Данные для входа в систему тестирования";
            // текст письма
            m.Body =
                $@"<h2>Здравстуйте, {fio}</h2> 
                <p>Ваши данные для входа в систему тестирования:</p>
                <br>
                <p>Логин: {login}</p>
                <p>Пароль: {pass}</p>";
            // письмо представляет код html
            m.IsBodyHtml = true;
            // адрес smtp-сервера и порт, с которого будем отправлять письмо
            SmtpClient smtp = new SmtpClient("smtp.yandex.ru", 587);
            // логин и пароль
            smtp.Credentials = new NetworkCredential("SysTestUp11@yandex.ru", "SystemTestUp11");
            smtp.EnableSsl = true;
            smtp.Send(m);
        }

    }
}
