using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WindowsFormsApp1
{
    public partial class Преподаватель : Form
    {
        public Преподаватель()
        {
            InitializeComponent();
        }

        private void Преподаватель_Load(object sender, EventArgs e)
        {
            // TODO: данная строка кода позволяет загрузить данные в таблицу "sysTestDataSet.Тест". При необходимости она может быть перемещена или удалена.
            //this.тестTableAdapter.Fill(this.sysTestDataSet.Тест);
            fillFiltersLogDataTable();
            comboBox2.Text = "";
            comboBox3.Text = "";
            updTestLogDataTable();
            updDT();
            try
            {
                SelectedTest.SelectedIndex = 0;
                updTestDataTable();
            }
            catch (Exception) { }
            if (Авторизация.sqlcon.State == ConnectionState.Open)
                Авторизация.sqlcon.Close();
        }
        public void cleanElements()
        {
            InputQuestion.Text = "";
            InputAnswerOption1.Text = "";
            InputAnswerOption2.Text = "";
            InputAnswerOption3.Text = "";
            radioButton1.Checked = false;
            radioButton2.Checked = false;
            radioButton3.Checked = false;
        }
        private void fillFiltersLogDataTable()
        {
            Авторизация.sqlcon.Open();
            DataSet ds = new DataSet();
            new SqlDataAdapter(
                $@"SELECT [Логин], [ФИО]
                FROM [dbo].[Пользователь]
                WHERE [id_должности] = 3",
                Авторизация.sqlcon).Fill(ds);
            comboBox2.DisplayMember = "ФИО";
            comboBox2.ValueMember = "Логин";
            comboBox2.DataSource = ds.Tables[0];
            ds = new DataSet();
            new SqlDataAdapter(
                $@"SELECT [id], [Наименование]
                FROM [dbo].[Тест]
                WHERE [Количество_вопросов] > 0",
                Авторизация.sqlcon).Fill(ds);
            comboBox3.DisplayMember = "Наименование";
            comboBox3.ValueMember = "id";
            comboBox3.DataSource = ds.Tables[0];
            Авторизация.sqlcon.Close();
        }
        // Обновление таблицы с историей
        private void updTestLogDataTable()
        {
            Авторизация.sqlcon.Open();
            DataSet ds = new DataSet();
            try
            {
                new SqlDataAdapter(
                @"SELECT h.[Дата], h.[Логин], p.[ФИО], t.[Наименование], h.[Количество_баллов] as [Количество баллов]
                FROM [dbo].[История] as h
                JOIN [dbo].[Тест] as t on h.[Номер_теста] = t.[id]
                JOIN [dbo].[Пользователь] as p on p.[Логин] = h.[Логин]",
                Авторизация.sqlcon).
                Fill(ds);
                dataGridView1.DataSource = ds.Tables[0];
            }
            catch (Exception) { }
            finally
            {
                if (Авторизация.sqlcon.State == ConnectionState.Open)
                    Авторизация.sqlcon.Close();
            }
        }
        // Обновление выпадающего списка с тестами
        private void updDT()
        {
            // TODO: данная строка кода позволяет загрузить данные в таблицу "sysTestDataSet.Тест". При необходимости она может быть перемещена или удалена.
            this.тестTableAdapter.Fill(this.sysTestDataSet.Тест);
            DataSet ds = new DataSet();
            new SqlDataAdapter("SELECT id, Наименование FROM [dbo].[Тест]", Авторизация.sqlcon).Fill(ds);
            SelectedTest.DisplayMember = "Наименование";
            SelectedTest.ValueMember = "id";
            SelectedTest.DataSource = ds.Tables[0];
            try
            {
                SelectedTest.SelectedIndex = 0;
            }
            catch (Exception) { }
        }
        // Обновление таблцы для просмотра вопросов из выбранного теста
        private void updTestDataTable()
        {
            if (Авторизация.sqlcon.State == ConnectionState.Open)
                Авторизация.sqlcon.Close();
            Авторизация.sqlcon.Open();
            try
            {
                DataSet ds = new DataSet();
                new SqlDataAdapter(
                    $@"SELECT  v.[Номер_вопроса],v.[Наименование] as Вопрос, o.Наименование as [Ответ], o.[Вариант_ответа] as [Вариант], o.Статус_ответа as [Статус ответа]
                FROM [dbo].[Тест] as t
                JOIN [dbo].[Вопрос] as v on v.[id_теста] = t.id
                JOIN [dbo].[Ответ] as o on o.[id_вопроса] = v.id
                WHERE t.id = {SelectedTest.SelectedValue}",
                    Авторизация.sqlcon).
                    Fill(ds);
                dataGridView2.DataSource = ds.Tables[0];
            }
            catch (Exception ex) { 
               // MessageBox.Show(ex.Message + "\n" + new StackFrame(0, true).GetFileLineNumber()); 
            }
            
            if (Авторизация.sqlcon.State == ConnectionState.Open)
                Авторизация.sqlcon.Close();
            //updTestQuestionsNums();
        }
        // Узнаем количество вопросов из теста и его название
        private void updTestQuestionsNums()
        {
            Авторизация.sqlcon.Open();
            try
            {
                DataSet ds = new DataSet();
                new SqlDataAdapter(
                    $@"SELECT [Номер_вопроса], [id]
                FROM [dbo].[Вопрос]
                WHERE [dbo].[Вопрос].[id_теста] = {SelectedTest.SelectedValue}",
                    Авторизация.sqlcon).
                    Fill(ds);
                comboBox1.DataSource = ds.Tables[0];
                comboBox1.DisplayMember = "Номер_вопроса";
                comboBox1.ValueMember = "id";
            }
            catch (Exception ex) { MessageBox.Show(ex.Message + "\n117"); }
            try
            {
                comboBox1.SelectedIndex = 0;
            }
            catch (Exception) { }
            
            if (Авторизация.sqlcon.State == ConnectionState.Open)
                    Авторизация.sqlcon.Close();
        }
        // Добавление нового теста
        private void button1_Click(object sender, EventArgs e)
        {
            if (InputTestName.Text.Length < 1)
            {
                MessageBox.Show("Укажите название теста!");
                return;
            }
            Авторизация.sqlcon.Open();
            new SqlDataAdapter(
                $@"INSERT INTO [dbo].[Тест]([Наименование],[Количество_вопросов])
                VALUES('{InputTestName.Text}',0)",
                Авторизация.sqlcon).
                Fill(new DataSet());
            Авторизация.sqlcon.Close();
            MessageBox.Show("Тест был создан успешно \n Выберите созданный тест в выпадающем списке \nв левом верхнем углу приложения!");
            updDT();
            cleanElements();
            dataGridView2.DataSource = new DataSet().Tables;
            SelectedTest.Text = "";
            InputTestName.Text = "";
            comboBox1.Text = "";
        }
        // Продгрузка данных при выборе теста
        private void SelectedTest_SelectedIndexChanged(object sender, EventArgs e)
        {
            cleanElements();
            try
            {
                comboBox1.Text = "";
                updTestDataTable();
                updTestQuestionsNums();
            }
            catch (Exception) { }
            finally
            {
                if (Авторизация.sqlcon.State == ConnectionState.Open)
                    Авторизация.sqlcon.Close();
            }
            try
            {
                Авторизация.sqlcon.Open();
                SqlDataReader r = new SqlCommand(
                    $@"SELECT [Наименование] from [dbo].[Тест] where [dbo].[Тест].[id] = {SelectedTest.SelectedValue}",
                    Авторизация.sqlcon).
                   ExecuteReader();
                r.Read();
                InputTestName.Text = r.GetString(0);
                r.Close();
            }
            catch (Exception) { }
            finally
            {
                if (Авторизация.sqlcon.State == ConnectionState.Open)
                    Авторизация.sqlcon.Close();
            }
            
        }
        // Подгружение данных при выборе номера вопроса
        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (Авторизация.sqlcon.State == ConnectionState.Open)
                Авторизация.sqlcon.Close();
            Авторизация.sqlcon.Open();
            try
            {
                InputAnswerOption1.Text = "";
                InputAnswerOption2.Text = "";
                InputAnswerOption3.Text = "";
                InputQuestion.Text = "";
                radioButton1.Checked = false;
                radioButton2.Checked = false;
                radioButton3.Checked = false;
                SqlDataReader r = new SqlCommand(
                $@"SELECT [Вариант_ответа], [Наименование], [Статус_ответа]
                FROM [dbo].[Ответ]
                WHERE [id_вопроса] = {comboBox1.SelectedValue}", Авторизация.sqlcon).
                ExecuteReader();
                while (r.Read())
                {
                    switch (r.GetInt32(0))
                    {
                        case 1:
                            InputAnswerOption1.Text = r.GetString(1);
                            radioButton1.Checked = r.GetBoolean(2);
                            break;
                        case 2:
                            InputAnswerOption2.Text = r.GetString(1);
                            radioButton2.Checked = r.GetBoolean(2);
                            break;
                        case 3:
                            InputAnswerOption3.Text = r.GetString(1);
                            radioButton3.Checked = r.GetBoolean(2);
                            break;
                    }
                }
                r.Close();
                r = new SqlCommand($"SELECT [Наименование] FROM [dbo].[Вопрос] WHERE [id] = {comboBox1.SelectedValue}", Авторизация.sqlcon).ExecuteReader();
                r.Read();
                InputQuestion.Text = r.GetString(0);
            }
            catch (Exception ) { }
            
            
            if (Авторизация.sqlcon.State == ConnectionState.Open)
                Авторизация.sqlcon.Close();
        }
        // Создание нового вопроса
        private void button2_Click(object sender, EventArgs e)
        {
            int counts = 0;
            if (Авторизация.sqlcon.State == ConnectionState.Open)
                Авторизация.sqlcon.Close();
            Авторизация.sqlcon.Open();
            try
            {
                new SqlDataAdapter(
                $@"UPDATE [dbo].[Тест]
                SET [Количество_вопросов] = [Количество_вопросов]+1
                WHERE [dbo].[Тест].[id] = {SelectedTest.SelectedValue}",
                Авторизация.sqlcon).
                Fill(new DataSet());
                SqlDataReader r = new SqlCommand(
                    $@"SELECT [Количество_вопросов] FROM [dbo].[Тест] as t WHERE t.[id] = {SelectedTest.SelectedValue}",
                    Авторизация.sqlcon).
                    ExecuteReader();
                r.Read();
                counts = r.GetInt32(0);
                r.Close();

                new SqlDataAdapter(
                    $@"INSERT INTO [dbo].[Вопрос]([id_теста],[Номер_вопроса])
                VALUES({SelectedTest.SelectedValue}, {counts})",
                    Авторизация.sqlcon).
                    Fill(new DataSet());
                r = new SqlCommand(
                    $@"SELECT [id]
                    FROM [dbo].[Вопрос]
                    WHERE [id_теста] ={SelectedTest.SelectedValue} and [Номер_вопроса] = {counts}",
                    Авторизация.sqlcon).
                    ExecuteReader();
                r.Read();
                int QuestionId = r.GetInt32(0);
                r.Close();
                Авторизация.sqlcon.Close();
                try
                {
                    updTestQuestionsNums();
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message + "\n" + new StackFrame(0, true).GetFileLineNumber());
                }
                for (int i = 1; i <= 3; i++)
                {
                    new SqlDataAdapter(
                    $@"INSERT INTO [dbo].[Ответ]([id_вопроса],[Вариант_ответа],[Статус_ответа])
                    VALUES({QuestionId},{i},0)",
                    Авторизация.sqlcon).
                    Fill(new DataSet());
                }
                

                Авторизация.sqlcon.Close();
                try
                {
                    updTestQuestionsNums();
                    comboBox1.SelectedValue = QuestionId;
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message + "\n" + new StackFrame(0, true).GetFileLineNumber());
                }
                finally
                {
                    if (Авторизация.sqlcon.State == ConnectionState.Open)
                        Авторизация.sqlcon.Close();
                }
                MessageBox.Show("Вопрос был успешно создан\nвыберите его номер в выпадающем списке\nдля изменения вариантов ответа");
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message + "\n" + new StackFrame(0, true).GetFileLineNumber());
            }
            finally
            {
                if (Авторизация.sqlcon.State == ConnectionState.Open)
                    Авторизация.sqlcon.Close();
                InputAnswerOption1.Text = "";
                InputAnswerOption2.Text = "";
                InputAnswerOption3.Text = "";
                InputQuestion.Text = "";
                radioButton1.Checked =false;
                radioButton2.Checked = false;
                radioButton3.Checked = false;
                updTestDataTable();
            }

        }
        // Изменение названия теста
        private void button5_Click(object sender, EventArgs e)
        {
            if (InputTestName.Text.Length < 1)
            {
                MessageBox.Show("Укажите название теста!");
                return;
            }
            if (Авторизация.sqlcon.State == ConnectionState.Open)
                Авторизация.sqlcon.Close();
            Авторизация.sqlcon.Open();
            try
            {
                new SqlDataAdapter(
                $@"UPDATE [dbo].[Тест] SET [Наименование] = '{InputTestName.Text}'
                WHERE [dbo].[Тест].[id] = {SelectedTest.SelectedValue}",
                Авторизация.sqlcon).
                Fill(new DataSet());
                updTestDataTable();
                updDT();
                MessageBox.Show("Наименование теста успешно измененно!");

            }
            catch (Exception ex) { MessageBox.Show(ex.Message + "\n" + new StackFrame(0, true).GetFileLineNumber()); }
            finally
            {
                if (Авторизация.sqlcon.State == ConnectionState.Open)
                    Авторизация.sqlcon.Close();
            }
            
            
        }
        // Изменение вопроса
        private void button3_Click(object sender, EventArgs e)
        {
            if(InputQuestion.Text.Length<1 || InputAnswerOption1.Text.Length<1 || InputAnswerOption2.Text.Length<1||
                InputAnswerOption3.Text.Length < 1)
            {
                MessageBox.Show("Заполните все поля для изменения вопроса!");
                return;
            }
            if(!radioButton1.Checked && !radioButton2.Checked && !radioButton3.Checked)
            {
                MessageBox.Show("Выберите правильный ответ!");
                return;
            }
            if (Авторизация.sqlcon.State == ConnectionState.Open)
                Авторизация.sqlcon.Close();
            Авторизация.sqlcon.Open();
            try
            {
                new SqlDataAdapter(
                $@"UPDATE [dbo].[Вопрос] SET [Наименование] = '{InputQuestion.Text}'
                WHERE [dbo].[Вопрос].[Номер_вопроса] = {comboBox1.Text} and [dbo].[Вопрос].[id] = {comboBox1.SelectedValue}",
                Авторизация.sqlcon).
                Fill(new DataSet());
            }
            catch (Exception ex) { MessageBox.Show(ex.Message + "\n" + new StackFrame(0, true).GetFileLineNumber()); }

            try
            {
                SqlDataReader r = new SqlCommand(
                $@"SELECT [id]
                    FROM [dbo].[Вопрос]
                    WHERE [id_теста] ={SelectedTest.SelectedValue} and [Номер_вопроса] = {comboBox1.Text}",
                Авторизация.sqlcon).
                ExecuteReader();
                r.Read();
                int QuestionId = r.GetInt32(0);
                r.Close();
                bool rb = false;
                for (int i = 1; i <= 3; i++)
                {
                    string name = "";
                    
                    switch (i)
                    {
                        case 1:
                            name = InputAnswerOption1.Text;
                            rb = radioButton1.Checked;
                            break;
                        case 2:
                            name = InputAnswerOption2.Text;
                            rb = radioButton2.Checked;
                            break;
                        case 3:
                            name = InputAnswerOption3.Text;
                            rb = radioButton3.Checked;
                            break;
                    }
                    new SqlDataAdapter(
                    $@"UPDATE [dbo].[Ответ] SET [Наименование] = '{name}', [Статус_ответа] = '{rb}'
                    WHERE [dbo].[Ответ].[id_вопроса] = {QuestionId} and [Вариант_ответа] = {i}",
                    Авторизация.sqlcon).
                    Fill(new DataSet());
                }
                MessageBox.Show("Вопрос успешно изменен!");
            }
            catch (Exception ex) { MessageBox.Show(ex.Message + "\n" + new StackFrame(0, true).GetFileLineNumber()); }
            updTestDataTable();

        }
        // Удаление вопроса
        private void button4_Click(object sender, EventArgs e)
        {
            if (Авторизация.sqlcon.State == ConnectionState.Open)
                Авторизация.sqlcon.Close();
            int countQuestionId = Convert.ToInt32(comboBox1.SelectedValue);
            Авторизация.sqlcon.Open();
            int numQ = 0;
            try
            {
                SqlDataReader r = new SqlCommand(
                $@"SELECT [Номер_вопроса]
                FROM [dbo].[Вопрос]
                WHERE [dbo].[Вопрос].[id] = {countQuestionId}",
                Авторизация.sqlcon).
                ExecuteReader();
                r.Read();
                numQ = r.GetInt32(0);
                r.Close();
            }
            catch (Exception ex) { MessageBox.Show(ex.Message + "\n" + new StackFrame(0, true).GetFileLineNumber()); }

            try
            {
                new SqlDataAdapter(
                $@"DELETE FROM [dbo].[Ответ]
                WHERE [id_вопроса] = {countQuestionId}",
                Авторизация.sqlcon).
                Fill(new DataSet());
            }
            catch (Exception ex ) { MessageBox.Show(ex.Message + "\n" + new StackFrame(0, true).GetFileLineNumber()); }

            try
            {
                new SqlDataAdapter(
                $@"DELETE FROM [dbo].[Вопрос]
                WHERE [id] = {countQuestionId}",
                Авторизация.sqlcon).
                Fill(new DataSet());
            }
            catch (Exception ex) { MessageBox.Show(ex.Message + "\n" + new StackFrame(0, true).GetFileLineNumber()); }
            int countQuestions = 0;
            try
            {
                SqlDataReader r = new SqlCommand(
                    $@"SELECT [Количество_вопросов]
                FROM [dbo].[Тест]
                WHERE [dbo].[Тест].[id] = {SelectedTest.SelectedValue}",
                    Авторизация.sqlcon).
                    ExecuteReader();
                r.Read();
                countQuestions = r.GetInt32(0);
                r.Close();
                new SqlDataAdapter(
                    $@"UPDATE [dbo].[Тест] SET [Количество_вопросов]=[Количество_вопросов]-1
                    WHERE [dbo].[Тест].[id] = {SelectedTest.SelectedValue}",
                    Авторизация.sqlcon).
                    Fill(new DataSet());
            }
            catch (Exception ex) { MessageBox.Show(ex.Message + "\n" + new StackFrame(0, true).GetFileLineNumber()); }
            

            int countQForChange = countQuestions-numQ;
            numQ++;
            try
            {
                for (int i = 1; i <= countQForChange; i++)
                {
                    new SqlDataAdapter(
                    $@"UPDATE [dbo].[Вопрос] SET [Номер_вопроса] = {numQ - 1}
                    WHERE [id_теста] = {SelectedTest.SelectedValue} and [Номер_вопроса] = {numQ}",
                    Авторизация.sqlcon).
                    Fill(new DataSet());
                    numQ++;
                }
            }
            catch (Exception ex) { MessageBox.Show(ex.Message + "\n"+ new StackFrame(0,true).GetFileLineNumber()); }
            Авторизация.sqlcon.Close();
            updTestQuestionsNums(); updTestDataTable();
            cleanElements();
            comboBox1.Text = "";
        }

        private void button7_Click(object sender, EventArgs e)
        {
            updTestLogDataTable();
            comboBox2.Text = "";
            comboBox3.Text = "";
        }

        private void button6_Click(object sender, EventArgs e)
        {
            if ((comboBox2.Text.Length < 1 && comboBox3.Text.Length < 1))
            {
                updTestLogDataTable();
                return;
            }
            DataSet ds = new DataSet();
            string query;
            // фильтрация по логину
            if (comboBox3.Text.Length<1)
                query = $@"SELECT h.[Дата], h.[Логин], p.[ФИО], t.[Наименование], h.[Количество_баллов] as [Количество баллов]
                FROM [dbo].[История] as h
                JOIN [dbo].[Тест] as t on h.[Номер_теста] = t.[id]
                JOIN [dbo].[Пользователь] as p on p.[Логин] = h.[Логин]
				where p.Логин = '{comboBox2.SelectedValue}'";
            // фильтрация по тесту
            else if (comboBox2.Text.Length < 1)
                query = $@"SELECT h.[Дата], h.[Логин], p.[ФИО], t.[Наименование], h.[Количество_баллов] as [Количество баллов]
                FROM [dbo].[История] as h
                JOIN [dbo].[Тест] as t on h.[Номер_теста] = t.[id]
                JOIN [dbo].[Пользователь] as p on p.[Логин] = h.[Логин]
				where t.id = {comboBox3.SelectedValue}";
            // фильтрация по логину и тесту
            else
                query = $@"SELECT h.[Дата], h.[Логин], p.[ФИО], t.[Наименование], h.[Количество_баллов] as [Количество баллов]
                FROM [dbo].[История] as h
                JOIN [dbo].[Тест] as t on h.[Номер_теста] = t.[id]
                JOIN [dbo].[Пользователь] as p on p.[Логин] = h.[Логин]
				where t.id = {comboBox3.SelectedValue} and p.Логин = '{comboBox2.SelectedValue}'";

            new SqlDataAdapter(query,Авторизация.sqlcon).Fill(ds);
            dataGridView1.DataSource = ds.Tables[0];

        }
    }
}
