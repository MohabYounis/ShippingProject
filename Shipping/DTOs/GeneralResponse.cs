namespace Shipping.DTOs
{
    public class GeneralResponse
    {
        public bool IsSuccess { get; }
        public dynamic Data { get; }
        public string? Message { get; }
        public string? Error { get; }

        public GeneralResponse(bool isSuccess, dynamic data, string? error, string? message)
        {
            IsSuccess = isSuccess;
            Data = data;
            Message = message;
            Error = error;
        }

        public static GeneralResponse Success(dynamic data, string? message = "") => new(true, data, null, message);
        public static GeneralResponse Failure(string error) => new(false, default!, error, null);
    }
}
