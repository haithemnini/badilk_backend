namespace BadilkBackend.src.Core.Dtos.Responses;

public class ApiResponse<T>
{
    // Matches: { "message": "...", "data": {}, "status": true, "code": 200 }
    public string Message { get; init; } = "Success";
    public T? Data { get; init; }
    public bool Status { get; init; }
    public int Code { get; init; }

    public static ApiResponse<T> Ok(T? data = default, string message = "Success", int code = 200) =>
        new() { Status = true, Message = message, Data = data, Code = code };

    public static ApiResponse<T> Fail(string message, int code = 400, T? data = default) =>
        new() { Status = false, Message = message, Data = data, Code = code };
}

public sealed class ApiResponse : ApiResponse<object>
{
    public static ApiResponse Ok(string message = "Success", int code = 200) =>
        new() { Status = true, Message = message, Data = new { }, Code = code };

    public static ApiResponse Fail(string message, int code = 400) =>
        new() { Status = false, Message = message, Data = new { }, Code = code };
}
