using AutoFixture;
using FindACourse.Api.Controllers;
using FindACourse.Api.Entities;
using FindACourse.Api.Models;
using FindACourse.Api.Repositories;
using Microsoft.AspNetCore.Mvc;
using Moq;

namespace FindACourse.Test
{
    public class CoursesControllerTests
    {
        private readonly Mock<ICourseRepository> mockRepo;
        private readonly CoursesController controller;
        private static Fixture Fixture = new();

        public CoursesControllerTests()
        {
            mockRepo = new Mock<ICourseRepository>();
            controller = new CoursesController(mockRepo.Object);
        }

        [Fact]
        public async Task Get_ReturnsOk_WithCourses()
        {
            // Arrange
            var courses = Fixture.CreateMany<Course>(2);
            var totalItems = courses.Count();
            var message = string.Empty;
            mockRepo.Setup(r => r.GetCoursesAsync(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<string>()))
                .ReturnsAsync((courses, totalItems));

            // Act
            var result = await controller.Get(new CourseSearchQueryParameters { Offset = 0, PageSize = 10, SearchFilter = null });

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var response = okResult.Value as Response<IEnumerable<Course>>;
            Assert.NotNull(response);
            Assert.Equal(courses, response.Data);
            Assert.Equal(totalItems, response.TotalItems);
            Assert.Equal(message, response.Message);
        }

        [Fact]
        public async Task Get_ReturnsOk_WithNoCourses()
        {
            // Arrange
            var courses = new List<Course>();
            var totalItems = 0;
            var message = "No courses found.";
            mockRepo.Setup(r => r.GetCoursesAsync(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<string>()))
                .ReturnsAsync((courses, totalItems));

            // Act
            var result = await controller.Get(new CourseSearchQueryParameters { Offset = 0, PageSize = 10, SearchFilter = null });

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var response = okResult.Value as Response<IEnumerable<Course>>;
            Assert.NotNull(response);
            Assert.Empty(response.Data);
            Assert.Equal(totalItems, response.TotalItems);
            Assert.Equal(message, response.Message);
        }

        [Fact]
        public async Task Post_ReturnsOk_WhenContactUsDataIsPosted()
        {
            // Arrange
            var contactUs = Fixture.Create<ContactUsModel>();

            // Act
            var result = await controller.Post(contactUs);

            // Assert
            mockRepo.Verify(repo => repo.PostContactUsDataAsync(contactUs), Times.Once);
            Assert.IsType<OkResult>(result);
        }
    }
}
