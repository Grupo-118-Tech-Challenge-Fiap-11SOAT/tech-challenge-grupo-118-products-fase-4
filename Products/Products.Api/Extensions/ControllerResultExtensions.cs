using Microsoft.AspNetCore.Mvc;
using Products.Application.Common.Models;
using System.Net;

namespace Products.Api.Extensions;

public static class ControllerResultExtensions
{
    public static IActionResult ToActionResult<T>(
        this ControllerBase controller,
        Result<T> result)
    {
        return result.StatusCode switch
        {
            HttpStatusCode.OK => controller.Ok(result),
            HttpStatusCode.NotFound => controller.NotFound(result),
            HttpStatusCode.BadRequest => controller.BadRequest(result),
            _ => controller.StatusCode(
                    (int)HttpStatusCode.InternalServerError,
                    result)
        };
    }
}
