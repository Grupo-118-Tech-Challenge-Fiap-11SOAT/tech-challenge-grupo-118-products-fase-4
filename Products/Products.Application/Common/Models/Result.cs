using System.Net;

namespace Products.Application.Common.Models;

public class Result<T>
{
    public HttpStatusCode StatusCode { get; set; }
    public string Message { get; set; } = string.Empty;
    
    public T? Data { get; set; }

    public Result<T> Ok(T data, HttpStatusCode statusCode, string message = "") =>
        new() { StatusCode = statusCode, Data = data, Message = message };

    public Result<T> Fail(string message, HttpStatusCode statusCode) =>
        new() { StatusCode = statusCode, Message = message };
}
