using FindACourse.Api.Entities;
using FindACourse.Api.Models;

namespace FindACourse.Api.Repositories
{
    public interface ICourseRepository
    {
        Task<(IEnumerable<Course> Courses, int TotalItems)> GetCoursesAsync(int offset, int pageSize, string searchFilter);
        Task PostContactUsDataAsync(ContactUsModel contactUs);
    }
}
