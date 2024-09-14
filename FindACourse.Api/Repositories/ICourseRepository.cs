using FindACourse.Api.Controllers;
using FindACourse.Api.Entities;

namespace FindACourse.Api.Repositories
{
    public interface ICourseRepository
    {
        Task<(IEnumerable<Course> Courses, int TotalItems)> GetCoursesAsync(int offset, int pageSize, string searchFilter);
        Task PostContactUsDataAsync(ContactUs contactUs);
    }
}
