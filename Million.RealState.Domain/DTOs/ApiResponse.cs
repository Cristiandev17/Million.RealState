namespace Million.RealState.Domain.DTOs;

public class ApiResponse<T>
{
    public bool IsSuccess { get; set; }
    public string? Message { get; set; }
    public T? Data { get; set; }
    public string? Error { get; set; }

    public static ApiResponse<T> SuccessResult(T data, string? message = null)
    {
        return new ApiResponse<T>
        {
            IsSuccess = true,
            Data = data,
            Message = message
        };
    }

    public static ApiResponse<T> ErrorResult(string error, string? message = null)
    {
        return new ApiResponse<T>
        {
            IsSuccess = false,
            Error = error,
            Message = message
        };
    }
}
