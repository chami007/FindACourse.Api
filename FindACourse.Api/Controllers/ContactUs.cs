using System.ComponentModel.DataAnnotations;

namespace FindACourse.Api.Controllers
{
    public class ContactUs
    {
        [Required]
        public required string CourseId { get; set; }

        [Required]
        public required string FirstName { get; set; }

        [Required]
        public required string LastName { get; set; }

        [Required]
        public required string Email { get; set; }

        public string? Phone { get; set; }

        public string? Message { get; set; }
    }
}
