using System.Net;
using FluentAssertions;
using Products.Application.Common.Models;

namespace UnitTests.Products.Application.Common.Models;

public class ResultTests
{
    [Fact]
    public void Ok_ShouldReturnResultWithDataAndStatusCode()
    {
        // Arrange
        var result = new Result<string>();
        var expectedData = "Produto criado com sucesso";
        var expectedMessage = "Operação concluída";
        var expectedStatusCode = HttpStatusCode.OK;

        // Act
        var okResult = result.Ok(expectedData, expectedStatusCode, expectedMessage);

        // Assert
        okResult.Should().NotBeNull();
        okResult.Data.Should().Be(expectedData);
        okResult.Message.Should().Be(expectedMessage);
        okResult.StatusCode.Should().Be(expectedStatusCode);
    }

    [Fact]
    public void Ok_ShouldAllowEmptyMessage()
    {
        // Arrange
        var result = new Result<int>();
        var expectedData = 123;
        var expectedStatusCode = HttpStatusCode.Created;

        // Act
        var okResult = result.Ok(expectedData, expectedStatusCode);

        // Assert
        okResult.Message.Should().BeEmpty();
        okResult.Data.Should().Be(expectedData);
        okResult.StatusCode.Should().Be(expectedStatusCode);
    }

    [Fact]
    public void Fail_ShouldReturnResultWithoutData()
    {
        // Arrange
        var result = new Result<object>();
        var expectedMessage = "Erro ao processar requisição";
        var expectedStatusCode = HttpStatusCode.BadRequest;

        // Act
        var failResult = result.Fail(expectedMessage, expectedStatusCode);

        // Assert
        failResult.Data.Should().BeNull();
        failResult.Message.Should().Be(expectedMessage);
        failResult.StatusCode.Should().Be(expectedStatusCode);
    }

    [Fact]
    public void Ok_And_Fail_ShouldReturnDifferentInstances()
    {
        // Arrange
        var result = new Result<string>();

        // Act
        var ok = result.Ok("Sucesso", HttpStatusCode.OK);
        var fail = result.Fail("Erro", HttpStatusCode.BadRequest);

        // Assert
        ok.Should().NotBeSameAs(fail);
        ok.Data.Should().Be("Sucesso");
        fail.Message.Should().Be("Erro");
    }
}
