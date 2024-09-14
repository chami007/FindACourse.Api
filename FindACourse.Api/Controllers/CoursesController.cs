using FindACourse.Api.Entities;
using FindACourse.Api.Models;
using FindACourse.Api.Repositories;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace FindACourse.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CoursesController : ControllerBase
    {
        private readonly ICourseRepository courseRepository;

        public CoursesController(ICourseRepository courseRepository)
        {
            this.courseRepository = courseRepository;
        }

        // GET: api/<CoursesController>
        [HttpGet]
        public async Task<IActionResult> Get([FromQuery] CourseSearchQueryParameters parameters)
        {
            var (Courses, TotalItems) = await this.courseRepository.GetCoursesAsync(parameters.Offset, parameters.PageSize, parameters.SearchFilter ?? string.Empty);
            var message = string.Empty;
            if (!Courses.Any())
            {
                message = "No courses found.";
            }

            return Ok(new Response<IEnumerable<Course>>(Courses, TotalItems, message));
        }

        // GET api/<CoursesController>/5
        [HttpGet("{id}")]
        public string Get(int id)
        {
            return "value";
        }

        // POST api/<CoursesController>
        [HttpPost]
        public async Task<IActionResult> Post([FromBody] ContactUs contactUs)
        {
            await this.courseRepository.PostContactUsDataAsync(contactUs);
            return Ok();
        }

        // PUT api/<CoursesController>/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE api/<CoursesController>/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
