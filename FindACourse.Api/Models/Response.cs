namespace FindACourse.Api.Models
{
    public class Response<T>
    {
        public T Data { get; set; }
        public int TotalItems { get; set; }
        public string Message { get; set; }

        public Response(T data, int totalItems, string message)
        {
            this.Data = data;
            this.TotalItems = totalItems;
            this.Message = message;
        }
    }
}
