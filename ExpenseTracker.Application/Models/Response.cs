namespace ExpenseTracker.Application.Models;

public class Response<T>
{
    private Response(int statusCode, string message, T? data = default, List<string>? errors = null)
    {
        StatusCode = statusCode;
        Message = message;
        Data = data;
        Errors = errors;
    }
    
    public int StatusCode { get; init; }
    public string Message { get; init; }
    public T? Data { get; init; }
    public List<string>?  Errors { get; init; }

    public static Response<T> Success(T data, string message = "Success", int statusCode = 200)
        => new(statusCode, message, data);

    public static Response<T> Success(string message = "Success", int statusCode = 200)
        => new(statusCode, message);

    public static Response<T> Fail(string errorMessage, int statusCode = 400)
        => new(statusCode, "Error", default, new List<string> { errorMessage });

    public static Response<T> Fail(IEnumerable<string> errors, int statusCode = 400)
        => new(statusCode, "Error", default, errors.ToList());
}