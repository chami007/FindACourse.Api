using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace FindACourse.Api.Models
{
    public class CourseSearchQueryParameters
    {
        [BindRequired]
        public int Offset { get; set; }

        [BindRequired]
        public int PageSize { get; set; }

        public string? SearchFilter { get; set; }
    }
}
