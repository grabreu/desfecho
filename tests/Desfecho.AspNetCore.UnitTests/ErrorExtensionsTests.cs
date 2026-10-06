namespace Desfecho.AspNetCore.UnitTests;

public class ErrorExtensionsTests
{
    [Fact]
    public void ToProblem_WithValidationErrors_ReturnsValidationProblem()
    {
        // Arrange
        var errors = Result.Invalid(new Dictionary<string, string[]>
        {
            ["Name"] = ["Name is required.", "Name is too long."],
            ["Sku"] = ["SKU is required."]
        }).Items;

        // Act
        var apiResult = errors.ToProblem();

        // Assert
        var problemDetails = apiResult.ShouldBeOfType<ProblemHttpResult>();
        problemDetails.StatusCode.ShouldBe(StatusCodes.Status400BadRequest);

        var validationProblemDetails = problemDetails.ProblemDetails.ShouldBeOfType<HttpValidationProblemDetails>();
        validationProblemDetails.Errors.Keys.ShouldBe(["Name", "Sku"], ignoreOrder: true);
        validationProblemDetails.Errors["Name"].ShouldBe(["Name is required.", "Name is too long."]);
        validationProblemDetails.Errors["Sku"].ShouldBe(["SKU is required."]);
    }

    [Theory]
    [InlineData(ErrorType.Validation, StatusCodes.Status400BadRequest)]
    [InlineData(ErrorType.Unauthorized, StatusCodes.Status401Unauthorized)]
    [InlineData(ErrorType.Forbidden, StatusCodes.Status403Forbidden)]
    [InlineData(ErrorType.NotFound, StatusCodes.Status404NotFound)]
    [InlineData(ErrorType.Conflict, StatusCodes.Status409Conflict)]
    [InlineData(ErrorType.Failure, StatusCodes.Status500InternalServerError)]
    public void ToProblem_WithError_ReturnsExpectedStatusCode(ErrorType errorType, int expectedStatusCode)
    {
        // Arrange
        var error = new Error(errorType, "Error description.");

        // Act
        var apiResult = error.ToProblem();

        // Assert
        var problemDetails = apiResult.ShouldBeOfType<ProblemHttpResult>();
        problemDetails.StatusCode.ShouldBe(expectedStatusCode);
    }

    [Fact]
    public void ToProblem_WithNonValidationError_ReturnsDetailFromDescription()
    {
        // Act
        var apiResult = Result.NotFound("Todo item was not found.").ToProblem();

        // Assert
        apiResult.ShouldBeOfType<ProblemHttpResult>().ProblemDetails.Detail.ShouldBe("Todo item was not found.");
    }

    [Fact]
    public void ToProblem_WithMixedErrors_ReturnsProblemForFirstError()
    {
        // Arrange
        IReadOnlyList<Error> errors = [Result.Conflict("Conflict."), Result.Invalid("Name", "Name is required.")];

        // Act
        var apiResult = errors.ToProblem();

        // Assert
        apiResult.ShouldBeOfType<ProblemHttpResult>().StatusCode.ShouldBe(StatusCodes.Status409Conflict);
    }

    [Fact]
    public void ToProblem_WithUnknownErrorType_ReturnsInternalServerError()
    {
        // Act
        var apiResult = new Error((ErrorType)(-1), "Error description.").ToProblem();

        // Assert
        apiResult.ShouldBeOfType<ProblemHttpResult>().StatusCode.ShouldBe(StatusCodes.Status500InternalServerError);
    }
}
