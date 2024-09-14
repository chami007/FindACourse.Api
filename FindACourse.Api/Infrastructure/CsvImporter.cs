using CsvHelper;
using Dapper;
using FindACourse.Api.Entities;
using Microsoft.Data.Sqlite;
using System.Globalization;

namespace FindACourse.Api.Infrastructure
{
    public class CsvImporter
    {
        private readonly string sqliteConnectionString = "Data Source=sqlite.db";
        private readonly string csvFilePath = Path.Combine(Directory.GetCurrentDirectory(), "technical_assignment_input_data.csv");

        public void ImportCsvData()
        {
            using var connection = new SqliteConnection(sqliteConnectionString);
            connection.Open();

            string dropTableQuery = "DROP TABLE IF EXISTS Courses;";
            connection.Execute(dropTableQuery);

            string createTableQuery = @"
            CREATE TABLE IF NOT EXISTS Courses (
                Id INTEGER PRIMARY KEY AUTOINCREMENT,
                CourseId TEXT,
                InstituteName TEXT,
                CourseName TEXT,
                Category TEXT,
                DeliveryMethod TEXT,
                Location TEXT,
                Language TEXT,
                StartDate DATE
            );";

            connection.Execute(createTableQuery);

            using var reader = new StreamReader(csvFilePath);
            using var csv = new CsvReader(reader, CultureInfo.InvariantCulture);
            var records = csv.GetRecords<CSVCourseModel>().ToList();

            using var transaction = connection.BeginTransaction();

            foreach (var record in records)
            {
                string insertQuery = @"
                INSERT INTO Courses 
                (CourseId, InstituteName, CourseName, Category, DeliveryMethod, Location, Language, StartDate) 
                VALUES 
                (@CourseId, @InstituteName, @CourseName, @Category, @DeliveryMethod, @Location, @Language, @StartDate);";

                connection.Execute(insertQuery, new
                {
                    record.CourseId,
                    record.InstituteName,
                    record.CourseName,
                    record.Category,
                    record.DeliveryMethod,
                    record.Location,
                    Language = record.Language.Equals("null", StringComparison.CurrentCultureIgnoreCase) ? null : record.Language,
                    StartDate = DateTime.ParseExact(record.StartDate, "dd/MM/yyyy HH:mm", CultureInfo.InvariantCulture)
                }, transaction: transaction);
            }

            transaction.Commit();
        }
    }
}
