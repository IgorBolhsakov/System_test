using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Net.Mail;
using System.Net;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WindowsFormsApp1
{
    public partial class Ученик : Form
    {
        private int[] userTestData;
        private int countQuestions = 0;
        public Ученик()
        {
            InitializeComponent();
            listBox1.ColumnWidth = 22;
            listBox1.Width = 24*(int)Math.Ceiling(Convert.ToDouble(listBox1.Items.Count)/5);
            //Выбор_теста.TestId = 6;
            //Авторизация.la = "q";
        }

        private void listBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            radioButton1.Checked = false;
            radioButton2.Checked = false;
            radioButton3.Checked = false;
            label2.Text = "Номер вопроса: " + listBox1.GetItemText(listBox1.SelectedItem);
            button1.Enabled = !(listBox1.SelectedIndex == (countQuestions - 1));
            button2.Enabled = !(listBox1.SelectedIndex == 0);
            button3.Enabled = (listBox1.SelectedIndex == (countQuestions - 1));
            button3.Visible = (listBox1.SelectedIndex == (countQuestions - 1));
            int questionNum = Convert.ToInt32(listBox1.GetItemText(listBox1.SelectedItem));

            switch (userTestData[questionNum-1])
            {
                case 0:
                    radioButton1.Checked = false;
                    radioButton2.Checked = false;
                    radioButton3.Checked = false;
                    break;
                case 1:
                    radioButton1.Checked = true;
                    break;
                case 2:
                    radioButton2.Checked = true;
                    break;
                case 3:
                    radioButton3.Checked = true;
                    break;
            }
            Авторизация.sqlcon.Open();
            SqlCommand s = new SqlCommand(
                $@"SELECT [Наименование] FROM [dbo].[Вопрос] WHERE [id] = {listBox1.SelectedValue}",
                Авторизация.sqlcon);
            SqlDataReader r = s.ExecuteReader();
            r.Read();
            QuestionName.Text = "Вопрос: " + r.GetString(0);
            r.Close();
            s = new SqlCommand(
                $@"SELECT [Наименование],[Вариант_ответа] FROM [dbo].[Ответ] WHERE [id_вопроса] = {listBox1.SelectedValue}",
                Авторизация.sqlcon);
            r = s.ExecuteReader();
            while (r.Read())
            {
                switch (r.GetInt32(1))
                {
                    case 1:
                        radioButton1.Text = r.GetString(0);
                        break;
                    case 2:
                        radioButton2.Text = r.GetString(0);
                        break;
                    case 3:
                        radioButton3.Text = r.GetString(0);
                        break;
                    default:
                        break;
                }
            }
            r.Close();
            Авторизация.sqlcon.Close();

        }

        private void button1_Click(object sender, EventArgs e)
        {
            listBox1.SelectedIndex++;
        }

        private void Ученик_Load(object sender, EventArgs e)
        {
            
            DataSet ds = new DataSet();
            Авторизация.sqlcon.Open();
            SqlDataReader r = new SqlCommand(
                $@"SELECT [Наименование], [Количество_вопросов] FROM [dbo].[Тест] where [dbo].[Тест].[id] = {Выбор_теста.TestId}",
                Авторизация.sqlcon).ExecuteReader();
            r.Read();
            TestName.Text = r.GetString(0);
            countQuestions = r.GetInt32(1);
            r.Close();
            Авторизация.sqlcon.Close();

            userTestData = new int[countQuestions];
            // userTestData[,0] = questionId
            // userTestData[,1] = answerNum
            // userTestData[,2] = statusAnswer (0:1) //
            Авторизация.sqlcon.Open();        
            for (int i = 0; i < countQuestions; i++)
            {
                userTestData[i] = 0;
            }
            Авторизация.sqlcon.Close();
            new SqlDataAdapter($@"Select [id], [Номер_вопроса] FROM [dbo].[Вопрос] WHERE [id_теста] = {Выбор_теста.TestId}",
                Авторизация.sqlcon).Fill(ds);
            listBox1.ValueMember = "id";
            listBox1.DisplayMember = "Номер_вопроса";
            listBox1.DataSource = ds.Tables[0];
            Авторизация.sqlcon.Close();
            listBox1.SelectedIndex = 0;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            listBox1.SelectedIndex--;
        }

        private void radioButton1_CheckedChanged(object sender, EventArgs e)
        {
            int questionNum = Convert.ToInt32(listBox1.GetItemText(listBox1.SelectedItem));
            if (radioButton1.Checked)
                userTestData[questionNum-1] = 1;
        }

        private void radioButton2_CheckedChanged(object sender, EventArgs e)
        {
            int questionNum = Convert.ToInt32(listBox1.GetItemText(listBox1.SelectedItem));
            if (radioButton2.Checked)
                userTestData[questionNum-1] = 2;
        }

        private void radioButton3_CheckedChanged(object sender, EventArgs e)
        {
            int questionNum = Convert.ToInt32(listBox1.GetItemText(listBox1.SelectedItem));
            if (radioButton3.Checked)
                userTestData[questionNum-1] = 3;
        }

        private void button3_Click(object sender, EventArgs e)
        {
            int points = CalculatePoints();
            DateTime datetime = DateTime.Now; // .ToString("yyyy-MM-dd HH:mm:ss")
            WriteToHistory(points, datetime);
            SendToEmail(datetime);
            MessageBox.Show("Тест завершён!\nРезультаты теста отправлены на почту!");
            ActiveForm.Hide();
            new Выбор_теста().ShowDialog();
            Close();
        }
        private int CalculatePoints()
        {
            // 100/countQuestions*countTrueAnswer
            int countTrueAnswer = 0;
            Авторизация.sqlcon.Open();
            for (int i = 0; i < countQuestions; i++)
            {
                SqlCommand s = new SqlCommand(
                $@"SELECT [id] FROM [dbo].[Вопрос]
                WHERE [id_теста] = {Выбор_теста.TestId} and [Номер_вопроса] = {i+1}",
                Авторизация.sqlcon);
                SqlDataReader r = s.ExecuteReader();
                r.Read();
                int questionId = r.GetInt32(0);
                r.Close();
                s = new SqlCommand(
                $@"SELECT [id] FROM [dbo].[Ответ]
                WHERE [id_вопроса] = {questionId} and [Вариант_ответа] = {userTestData[i]} and [Статус_ответа] = 1",
                Авторизация.sqlcon);
                r = s.ExecuteReader();
                if(r.Read())
                    countTrueAnswer++;
                r.Close();
            }
            int sum = (int)Math.Round((100.0 / countQuestions * countTrueAnswer));
            Авторизация.sqlcon.Close();
            return sum;
        }
        private void WriteToHistory(int points, DateTime datetime)
        {
            Авторизация.sqlcon.Open();
            new SqlDataAdapter(
                $@"INSERT INTO [dbo].[История]([Дата],[Логин],[Номер_теста],[Количество_баллов])
                VALUES('{datetime}','{Авторизация.la}',{Выбор_теста.TestId},{points})",
                Авторизация.sqlcon).Fill(new DataSet());
            Авторизация.sqlcon.Close();
        }
        private void SendToEmail(DateTime datetime)
        {
            string fio, eMail, testName, date;
            int points;
            Авторизация.sqlcon.Open();
            SqlDataReader r = new SqlCommand(
                $@"SELECT p.[ФИО], p.[Почта], t.[Наименование], h.[Дата], h.[Количество_баллов]
                FROM [dbo].[История] as h
                JOIN [dbo].[Тест] as t ON t.[id] = h.[Номер_теста]
                JOIN [dbo].[Пользователь] as p ON p.[Логин] = h.[Логин]
                WHERE p.[Логин] ='{Авторизация.la}' and h.[Дата] = '{datetime}'",
                Авторизация.sqlcon).ExecuteReader();
            r.Read();
            fio = r.GetString(0);
            eMail = r.GetString(1);
            testName = r.GetString(2);
            date = r.GetValue(3).ToString();
            points = r.GetInt32(4);
            r.Close();
            Авторизация.sqlcon.Close();
            // Данные от почты
            // email: SysTestUp11@yandex.ru
            // pass: SystemTestUp11

            // отправитель - устанавливаем адрес и отображаемое в письме имя
            MailAddress from = new MailAddress("SysTestUp11@yandex.ru", "Система тестирования");
            // кому отправляем
            MailAddress to = new MailAddress(eMail);
            // создаем объект сообщения
            MailMessage m = new MailMessage(from, to);
            // тема письма
            m.Subject = $"Тест: {testName}";
            // текст письма
            m.Body =
                $@"<h2>Здравстуйте, {fio}</h2> 
                <br>
                <p>Ваше количество баллов за пройденный тест составляет: {points} б. </p>
                <p>Дата прохождения теста: {date}</p>";
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
