using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WindowsFormsApp1
{
    public partial class Выбор_теста : Form
    {
        public Выбор_теста()
        {
            InitializeComponent();
        }

        public static int TestId;
        private void button1_Click(object sender, EventArgs e)
        {
            TestId = (int)comboBox1.SelectedValue;

            ActiveForm.Hide();
            new Ученик().ShowDialog();
            Close();
        }

        private void Выбор_теста_Load(object sender, EventArgs e)
        {
            Авторизация.sqlcon.Open();
            DataSet ds = new DataSet();
            new SqlDataAdapter(
                $@"SELECT [id], [Наименование]
                FROM [dbo].[Тест]
                WHERE [Количество_вопросов] > 0",
                Авторизация.sqlcon).Fill(ds);
            Авторизация.sqlcon.Close();
            comboBox1.DisplayMember = "Наименование";
            comboBox1.ValueMember = "id";
            comboBox1.DataSource = ds.Tables[0];
        }
    }
}
