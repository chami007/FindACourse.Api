using Dapper;
using Microsoft.Data.Sqlite;

namespace FindACourse.Api.Infrastructure
{
    public class ContactUsTableInitializer
    {
        private readonly string sqliteConnectionString = "Data Source=sqlite.db";

        public void InitializeTable()
        {
            using var connection = new SqliteConnection(sqliteConnectionString);
            connection.Open();

            string dropTableQuery = "DROP TABLE IF EXISTS ContactUs;";
            connection.Execute(dropTableQuery);

            string createTableQuery = @"
            CREATE TABLE IF NOT EXISTS ContactUs (
                Id INTEGER PRIMARY KEY AUTOINCREMENT,
                CourseId TEXT,
                FirstName TEXT,
                LastName TEXT,
                Email TEXT,
                Phone TEXT,
                Message TEXT,
                CreatedOn DATE
            );";

            connection.Execute(createTableQuery);
        }
    }
}
