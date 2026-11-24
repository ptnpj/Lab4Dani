using System;
using System.Data;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using System.Xml.Linq;
using Microsoft.Data.SqlClient;

namespace FormLb4
{
    public partial class Form1 : Form
    {
        private string connectionString = @"Server=localhost\SQLEXPRESS;Database=library;Trusted_Connection=True;TrustServerCertificate=True;";

        public Form1()
        {
            InitializeComponent();
        }

        // кнопка створити з таблиці
        private void btnExportTable_Click(object sender, EventArgs e)
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    SqlDataAdapter adapter = new SqlDataAdapter("SELECT * FROM member", conn);
                    DataTable dt = new DataTable("member");
                    adapter.Fill(dt);

                    dataGridView1.DataSource = dt;

                    DataSet ds = new DataSet("LibraryData");
                    ds.Tables.Add(dt);
                    ds.WriteXml("all_members.xml", XmlWriteMode.WriteSchema);

                    MessageBox.Show("Готово !!! Таблицю збережено у all_members.xml");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Помилка: " + ex.Message);
            }
        }

        // кнопка створити з вибірки
        private void btnExportSelect_Click(object sender, EventArgs e)
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    string query = "SELECT member_no, lastname, firstname FROM member WHERE member_no <= 10";
                    SqlDataAdapter adapter = new SqlDataAdapter(query, conn);
                    DataTable dt = new DataTable("member");
                    adapter.Fill(dt);

                    dataGridView1.DataSource = dt;

                    DataSet ds = new DataSet("SelectedData");
                    ds.Tables.Add(dt);
                    ds.WriteXml("selected_members.xml", XmlWriteMode.IgnoreSchema);

                    MessageBox.Show("Готово !!! Вибірку збережено у selected_members.xml");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Помилка: " + ex.Message);
            }
        }

        // кнопка пошук
        private void btnSearch_Click(object sender, EventArgs e)
        {
            if (!File.Exists("all_members.xml"))
            {
                MessageBox.Show("Спершу створіть файл all_members.xml!");
                return;
            }

            string searchId = txtSearchId.Text.Trim();
            XDocument doc = XDocument.Load("all_members.xml");

            var result = doc.Descendants("member")
                            .FirstOrDefault(el => el.Element("member_no")?.Value == searchId);

            if (result != null)
            {
                string info = $"{result.Element("lastname")?.Value} {result.Element("firstname")?.Value}";
                MessageBox.Show($"Знайдено: {info}");
            }
            else
            {
                MessageBox.Show("Даний запис відсутній !!!");
            }
        }

        // кнопка фільтрація даних
        private void btnFilter_Click(object sender, EventArgs e)
        {
            if (!File.Exists("all_members.xml"))
            {
                MessageBox.Show("Спершу створіть файл all_members.xml!");
                return;
            }

            if (!int.TryParse(txtFilterId.Text.Trim(), out int minId))
            {
                MessageBox.Show("Введіть коректне число у поле фільтра!");
                return;
            }

            XDocument doc = XDocument.Load("all_members.xml");
            var filtered = doc.Descendants("member")
                              .Where(el => el.Element("member_no") != null && int.Parse(el.Element("member_no").Value) > minId);

            XDocument newDoc = new XDocument(new XElement("FilteredMembers", filtered));
            newDoc.Save("filtered_members.xml");

            DataSet ds = new DataSet();
            ds.ReadXml("filtered_members.xml");
            if (ds.Tables.Count > 0)
                dataGridView1.DataSource = ds.Tables[0];

            MessageBox.Show("Готово !!! Відфільтровано у filtered_members.xml");
        }

        // кнопка запис даних
        private void btnImportToDb_Click(object sender, EventArgs e)
        {
            if (!File.Exists("filtered_members.xml"))
            {
                MessageBox.Show("Сппочатку створіть файл filtered_members.xml!");
                return;
            }

            try
            {
                XDocument doc = XDocument.Load("filtered_members.xml");
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();

                    using (SqlCommand onCmd = new SqlCommand("SET IDENTITY_INSERT member ON;", conn))
                        onCmd.ExecuteNonQuery();

                    int newId = 15001;
                    using (SqlCommand maxCmd = new SqlCommand("SELECT ISNULL(MAX(member_no), 15000) + 1 FROM member WHERE member_no >= 15000 AND member_no < 24000", conn))
                    {
                        newId = Convert.ToInt32(maxCmd.ExecuteScalar());
                    }

                    foreach (var elem in doc.Descendants("member").Take(5))
                    {
                        string rawLast = (elem.Element("lastname")?.Value ?? "Test") + "_xml";
                        string lastname = rawLast.Length > 15 ? rawLast.Substring(0, 15) : rawLast;

                        string rawFirst = elem.Element("firstname")?.Value ?? "User";
                        string firstname = rawFirst.Length > 15 ? rawFirst.Substring(0, 15) : rawFirst;

                        string insertSql = "INSERT INTO member (member_no, lastname, firstname) VALUES (@id, @lastname, @firstname)";
                        using (SqlCommand cmd = new SqlCommand(insertSql, conn))
                        {
                            cmd.Parameters.AddWithValue("@id", newId++);
                            cmd.Parameters.AddWithValue("@lastname", lastname);
                            cmd.Parameters.AddWithValue("@firstname", firstname);
                            cmd.ExecuteNonQuery();
                        }
                    }

                    using (SqlCommand offCmd = new SqlCommand("SET IDENTITY_INSERT member OFF;", conn))
                        offCmd.ExecuteNonQuery();

                    SqlDataAdapter adapter = new SqlDataAdapter("SELECT * FROM member WHERE member_no >= 25000", conn);
                    DataTable dt = new DataTable();
                    adapter.Fill(dt);
                    dataGridView1.DataSource = dt;
                }

                MessageBox.Show("Готово !!! Записи додано у БД!");
            }
            catch (Exception ex)
            {
                MessageBox.Show("Помилка: " + ex.Message);
            }
        }
    }
}