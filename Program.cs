using System;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Xml.Linq;

namespace XmlSqlLab
{
    class Program
    {
        private static string connectionString = @"Server=localhost\SQLEXPRESS;Database=library;Trusted_Connection=True;TrustServerCertificate=True;";

        static void Main(string[] args)
        {
            Console.OutputEncoding = System.Text.Encoding.UTF8;

            try
            {
                Console.WriteLine("\n 1. Створення XML з таблиці member");
                ExportTableToXml("member", "all_members.xml");

                Console.WriteLine("\n 2. Створення XML з SELECT-запиту");
                string selectQuery = "SELECT member_no, lastname, firstname FROM member WHERE member_no <= 10";
                ExportQueryToXml(selectQuery, "selected_members.xml");

                Console.WriteLine("\n 3. Пошук у XML-файлі");
                // Успішний пошук за member_no = 1
                SearchInXml("all_members.xml", "1");
                // Неуспішний пошук
                SearchInXml("all_members.xml", "999999");

                Console.WriteLine("\n 4. Фільтрація XML та збереження в новий файл");
                // Фільтрація записів за ID member_no > 5
                FilterAndSaveXml("all_members.xml", "filtered_members.xml", 5);

                Console.WriteLine("\n 5. Заповнення БД даними з XML");
                ImportXmlToDatabase("filtered_members.xml");

                Console.WriteLine("\nУсі пункти успішно виконано!");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Помилка: {ex.Message}");
            }

            Console.ReadLine();
        }

        static void ExportTableToXml(string tableName, string outputXmlPath)
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                string query = $"SELECT * FROM {tableName}";
                SqlDataAdapter adapter = new SqlDataAdapter(query, conn);
                DataTable dt = new DataTable(tableName);
                adapter.Fill(dt);

                DataSet ds = new DataSet("LibraryData");
                ds.Tables.Add(dt);
                ds.WriteXml(outputXmlPath, XmlWriteMode.WriteSchema);

                Console.WriteLine($"Дані таблиці '{tableName}' збережено в {outputXmlPath}");
            }
        }

        // ПУНКТ 3: Експорт результату SELECT у XML
        static void ExportQueryToXml(string query, string outputXmlPath)
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                SqlDataAdapter adapter = new SqlDataAdapter(query, conn);
                DataTable dt = new DataTable("QueryResult");
                adapter.Fill(dt);

                DataSet ds = new DataSet("FilteredData");
                ds.Tables.Add(dt);
                ds.WriteXml(outputXmlPath, XmlWriteMode.IgnoreSchema);

                Console.WriteLine($"Результат SELECT-запиту збережено в {outputXmlPath}");
            }
        }

        static void SearchInXml(string xmlPath, string searchId)
        {
            XDocument doc = XDocument.Load(xmlPath);

            var result = doc.Descendants("member")
                            .FirstOrDefault(e => e.Element("member_no")?.Value.Equals(searchId) == true);

            Console.WriteLine($"---> Пошук за member_no: '{searchId}'");
            if (result != null)
            {
                Console.WriteLine($"[УСПІШНО] Знайдено запис:");
                Console.WriteLine(result.ToString());
            }
            else
            {
                Console.WriteLine($"[НЕУСПІШНО] Запис з ID '{searchId}' не знайдено.");
            }
        }

        static void FilterAndSaveXml(string inputXml, string outputXml, int minId)
        {
            XDocument doc = XDocument.Load(inputXml);

            var filteredElements = doc.Descendants("member")
                .Where(e => e.Element("member_no") != null && int.Parse(e.Element("member_no").Value) > minId);

            XDocument newDoc = new XDocument(
                new XElement("FilteredMembers", filteredElements)
            );

            newDoc.Save(outputXml);
            Console.WriteLine($"Збережено відфільтровані XML-дані (member_no > {minId}) у файл {outputXml}");
        }

        static void ImportXmlToDatabase(string xmlPath)
        {
            XDocument doc = XDocument.Load(xmlPath);

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();
                foreach (var elem in doc.Descendants("member"))
                {
                    string lastname = (elem.Element("lastname")?.Value ?? "Test") + "_xml";
                    string firstname = elem.Element("firstname")?.Value ?? "User";

                    // Простий INSERT в таблицю member
                    string insertSql = "INSERT INTO member (lastname, firstname) VALUES (@lastname, @firstname)";
                    using (SqlCommand cmd = new SqlCommand(insertSql, conn))
                    {
                        cmd.Parameters.AddWithValue("@lastname", lastname);
                        cmd.Parameters.AddWithValue("@firstname", firstname);

                        cmd.ExecuteNonQuery();
                    }
                }
                Console.WriteLine("Дані з XML успішно додано в таблицю member!");
            }
        }
    }
}