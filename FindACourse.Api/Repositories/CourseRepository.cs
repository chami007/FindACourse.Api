using Dapper;
using FindACourse.Api.Controllers;
using FindACourse.Api.Entities;
using Microsoft.Data.Sqlite;

namespace FindACourse.Api.Repositories
{
    public class CourseRepository : ICourseRepository
    {
        private readonly string connectionString;

        public CourseRepository(string connectionString)
        {
            this.connectionString = connectionString;
        }

        public async Task<(IEnumerable<Course> Courses, int TotalItems)> GetCoursesAsync(int offset, int pageSize, string searchFilter)
        {
            using var connection = new SqliteConnection(connectionString);
            connection.Open();

            var query = @"
                SELECT COUNT(*) FROM Courses 
                WHERE (@searchFilter IS NULL OR CourseName LIKE @searchFilter OR Category LIKE @searchFilter);

                SELECT * FROM Courses 
                WHERE (@searchFilter IS NULL OR CourseName LIKE @searchFilter OR Category LIKE @searchFilter) 
                LIMIT @pageSize OFFSET @offset;
            ";

            var results = await connection.QueryMultipleAsync(query, new { pageSize, offset = offset * pageSize, searchFilter = $"%{searchFilter}%" });

            var totalCount = results.Read<int>().Single();
            var courses = results.Read<Course>().ToList();
            return (Courses: courses, TotalItems: totalCount);
        }

        public async Task PostContactUsDataAsync(ContactUs contactUs)
        {
            using var connection = new SqliteConnection(connectionString);
            connection.Open();

            var query = @"
                    INSERT INTO ContactUs (CourseId, FirstName, LastName, Email, Phone, Message, CreatedOn)
			        VALUES (@CourseId, @FirstName, @LastName, @Email, @Phone, @Message, @CreatedOn);";

            var parameters = new
            {
                contactUs.CourseId,
                contactUs.FirstName,
                contactUs.LastName,
                contactUs.Email,
                contactUs.Phone,
                contactUs.Message,
                CreatedOn = DateTime.UtcNow
            };

            await connection.ExecuteAsync(query, parameters);
        }
    }
}
