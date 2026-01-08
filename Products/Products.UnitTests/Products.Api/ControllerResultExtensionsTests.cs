using FluentAssertions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Products.Api.Extensions;
using Products.Application.Common.Models;
using System.Net;

namespace Products.UnitTests.Products.Api.Extensions;

public class ControllerResultExtensionsTests
{
    private readonly FakeController _controller;

    public ControllerResultExtensionsTests()
    {
        _controller = new FakeController();
    }

    [Fact]
    public void ToActionResult_WhenStatusIsOk_ShouldReturnOkObjectResult()
    {
        // Arrange
        var result = new Result<string>().Ok("success", HttpStatusCode.OK);

        // Act
        var actionResult = _controller.ToActionResult(result);

        // Assert
        var okResult = actionResult.Should().BeOfType<OkObjectResult>().Subject;
        okResult.StatusCode.Should().Be(StatusCodes.Status200OK);
        okResult.Value.Should().Be(result);
    }

    [Fact]
    public void ToActionResult_WhenStatusIsNotFound_ShouldReturnNotFoundObjectResult()
    {
        // Arrange
        var result = new Result<string>().Fail("not found", HttpStatusCode.NotFound);

        // Act
        var actionResult = _controller.ToActionResult(result);

        // Assert
        var notFoundResult = actionResult.Should().BeOfType<NotFoundObjectResult>().Subject;
        notFoundResult.StatusCode.Should().Be(StatusCodes.Status404NotFound);
        notFoundResult.Value.Should().Be(result);
    }

    [Fact]
    public void ToActionResult_WhenStatusIsBadRequest_ShouldReturnBadRequestObjectResult()
    {
        // Arrange
        var result = new Result<string>().Fail("bad request", HttpStatusCode.BadRequest);

        // Act
        var actionResult = _controller.ToActionResult(result);

        // Assert
        var badRequestResult = actionResult.Should().BeOfType<BadRequestObjectResult>().Subject;
        badRequestResult.StatusCode.Should().Be(StatusCodes.Status400BadRequest);
        badRequestResult.Value.Should().Be(result);
    }

    [Fact]
    public void ToActionResult_WhenStatusIsInternalServerError_ShouldReturnObjectResultWith500()
    {
        // Arrange
        var result = new Result<string>().Fail("error", HttpStatusCode.InternalServerError);

        // Act
        var actionResult = _controller.ToActionResult(result);

        // Assert
        var objectResult = actionResult.Should().BeOfType<ObjectResult>().Subject;
        objectResult.StatusCode.Should().Be(StatusCodes.Status500InternalServerError);
        objectResult.Value.Should().Be(result);
    }

    // Controller fake apenas para testar a extension
    private class FakeController : ControllerBase
    {
    }
}
