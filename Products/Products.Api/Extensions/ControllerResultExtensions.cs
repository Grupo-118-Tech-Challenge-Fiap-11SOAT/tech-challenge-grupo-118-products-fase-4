using Microsoft.AspNetCore.Mvc;
using Products.Application.Common.Models;
using System.Diagnostics.CodeAnalysis;
using System.Net;
using Products.Application.Dtos;

namespace Products.Api.Extensions;

[ExcludeFromCodeCoverage]
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
            HttpStatusCode.Created => result.Data is ProductDto product
                ? controller.CreatedAtAction(actionName: "GetDetailedProduct", controllerName: "Products",
                    new { productId = product.Id }, product)
                : controller.Created(string.Empty, result),
            _ => controller.StatusCode(
                (int)HttpStatusCode.InternalServerError,
                result)
        };
    }
}