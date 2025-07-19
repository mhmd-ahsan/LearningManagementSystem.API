namespace LMS.API.Helpers
{
    public class ResponseHelper<T>
    {
        public int StatusCode { get; set; }
        public string Message { get; set; } = string.Empty;
        public T? Data { get; set; }

        public static ResponseHelper<T> Success(T? data, string message = "Success", int statusCode = 200)
        {
            return new ResponseHelper<T> { StatusCode = statusCode, Message = message, Data = data };
        }

        public static ResponseHelper<T> Fail(string message, int statusCode = 400)
        {
            return new ResponseHelper<T> { StatusCode = statusCode, Message = message };
        }
    }
}
