namespace Desfecho.AspNetCore.UnitTests;

public class MinimalApiResultExtensionsTests
{
    [Fact]
    public async Task ToProblem_WithEmptyErrors_ReturnsInternalServerError()
    {
        // Arrange
        IReadOnlyList<Error> errors = [];

        // Act
        var apiResult = errors.ToProblem();

        // Assert
        var problemDetails = apiResult.ShouldBeOfType<ProblemHttpResult>();
        problemDetails.StatusCode.ShouldBe(StatusCodes.Status500InternalServerError);
    }

    [Fact]
    public async Task ToProblem_WithValidationErrors_ReturnsValidationProblem()
    {
        // Arrange
        IReadOnlyList<Error> errors =
        [
            Error.Validation("Name", "Name is required."),
            Error.Validation("Name", "Name is too long."),
            Error.Validation("Sku", "SKU is required.")
        ];

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
    public async Task ToProblem_WithError_ReturnsExpectedStatusCode(ErrorType errorType, int expectedStatusCode)
    {
        // Arrange
        IReadOnlyList<Error> errors =
        [
            new Error("Error.Code", "Error description.", errorType)
        ];

        // Act
        var apiResult = errors.ToProblem();

        // Assert
        var problemDetails = apiResult.ShouldBeOfType<ProblemHttpResult>();
        problemDetails.StatusCode.ShouldBe(expectedStatusCode);
    }

    [Fact]
    public async Task ToProblem_WithUnknownErrorType_ReturnsInternalServerError()
    {
        // Arrange
        IReadOnlyList<Error> errors =
        [
            new Error("Error.Code", "Error description.", (ErrorType)(-1))
        ];

        // Act
        var apiResult = errors.ToProblem();

        // Assert
        var problemDetails = apiResult.ShouldBeOfType<ProblemHttpResult>();
        problemDetails.StatusCode.ShouldBe(StatusCodes.Status500InternalServerError);
    }

    [Fact]
    public async Task ToOk_WithSuccessfulResult_ReturnsOkWithValue()
    {
        // Arrange
        Result<string> result = "value";

        // Act
        var apiResult = result.ToOk();

        // Assert
        var ok = apiResult.ShouldBeOfType<Ok<string>>();
        ok.StatusCode.ShouldBe(StatusCodes.Status200OK);
        ok.Value.ShouldBe("value");
    }

    [Fact]
    public async Task ToOk_WithErrorResult_ReturnsProblem()
    {
        // Arrange
        Result<string> result = Error.NotFound("TodoItem.NotFound", "Todo item was not found.");

        // Act
        var apiResult = result.ToOk();

        // Assert
        var problemDetails = apiResult.ShouldBeOfType<ProblemHttpResult>();
        problemDetails.StatusCode.ShouldBe(StatusCodes.Status404NotFound);
    }

    [Fact]
    public async Task ToCreated_WithSuccessfulResult_ReturnsCreatedWithLocationAndValue()
    {
        // Arrange
        Result<string> result = "value";

        // Act
        var apiResult = result.ToCreated(value => $"/items/{value}");

        // Assert
        var created = apiResult.ShouldBeOfType<Created<string>>();
        created.StatusCode.ShouldBe(StatusCodes.Status201Created);
        created.Location.ShouldBe("/items/value");
        created.Value.ShouldBe("value");
    }

    [Fact]
    public async Task ToCreated_WithErrorResult_ReturnsProblem()
    {
        // Arrange
        Result<string> result = Error.Conflict("TodoItem.Conflict", "Todo item already exists.");

        // Act
        var apiResult = result.ToCreated(value => $"/items/{value}");

        // Assert
        var problemDetails = apiResult.ShouldBeOfType<ProblemHttpResult>();
        problemDetails.StatusCode.ShouldBe(StatusCodes.Status409Conflict);
    }

    [Fact]
    public async Task ToNoContent_WithSuccessfulResult_ReturnsNoContent()
    {
        // Arrange
        Result<string> result = "value";

        // Act
        var apiResult = result.ToNoContent();

        // Assert
        var noContent = apiResult.ShouldBeOfType<NoContent>();
        noContent.StatusCode.ShouldBe(StatusCodes.Status204NoContent);
    }

    [Fact]
    public async Task ToNoContent_WithErrorResult_ReturnsProblem()
    {
        // Arrange
        Result<string> result = Error.Validation("Name.Required", "Name is required.");

        // Act
        var apiResult = result.ToNoContent();

        // Assert
        var problemDetails = apiResult.ShouldBeOfType<ProblemHttpResult>();
        problemDetails.StatusCode.ShouldBe(StatusCodes.Status400BadRequest);
    }
}
