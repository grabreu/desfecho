namespace Desfecho.AspNetCore.UnitTests;

public class ResultExtensionsTests
{
    [Fact]
    public void ToOk_WithSuccessfulResult_ReturnsOkWithValue()
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
    public void ToOk_WithErrorResult_ReturnsProblem()
    {
        // Arrange
        Result<string> result = Result.NotFound("Todo item was not found.");

        // Act
        var apiResult = result.ToOk();

        // Assert
        var problemDetails = apiResult.ShouldBeOfType<ProblemHttpResult>();
        problemDetails.StatusCode.ShouldBe(StatusCodes.Status404NotFound);
    }

    [Fact]
    public async Task ToOk_WithTaskOfSuccessfulResult_ReturnsOkWithValue()
    {
        // Arrange
        var result = Task.FromResult<Result<string>>("value");

        // Act
        var apiResult = await result.ToOk();

        // Assert
        apiResult.ShouldBeOfType<Ok<string>>().Value.ShouldBe("value");
    }

    [Fact]
    public void ToCreated_WithSuccessfulResult_ReturnsCreatedWithLocationAndValue()
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
    public void ToCreated_WithErrorResult_ReturnsProblem()
    {
        // Arrange
        Result<string> result = Result.Conflict("Todo item already exists.");

        // Act
        var apiResult = result.ToCreated(value => $"/items/{value}");

        // Assert
        var problemDetails = apiResult.ShouldBeOfType<ProblemHttpResult>();
        problemDetails.StatusCode.ShouldBe(StatusCodes.Status409Conflict);
    }

    [Fact]
    public async Task ToCreated_WithTaskOfSuccessfulResult_ReturnsCreated()
    {
        // Arrange
        var result = Task.FromResult<Result<string>>("value");

        // Act
        var apiResult = await result.ToCreated(value => $"/items/{value}");

        // Assert
        apiResult.ShouldBeOfType<Created<string>>().Location.ShouldBe("/items/value");
    }

    [Fact]
    public void ToNoContent_WithSuccessfulResult_ReturnsNoContent()
    {
        // Act
        var apiResult = Result.Success().ToNoContent();

        // Assert
        var noContent = apiResult.ShouldBeOfType<NoContent>();
        noContent.StatusCode.ShouldBe(StatusCodes.Status204NoContent);
    }

    [Fact]
    public void ToNoContent_WithSuccessfulResultOfValue_ReturnsNoContent()
    {
        // Arrange
        Result<string> result = "value";

        // Act
        var apiResult = result.ToNoContent();

        // Assert
        apiResult.ShouldBeOfType<NoContent>();
    }

    [Fact]
    public void ToNoContent_WithErrorResult_ReturnsProblem()
    {
        // Arrange
        Result result = Result.Invalid("Name", "Name is required.");

        // Act
        var apiResult = result.ToNoContent();

        // Assert
        var problemDetails = apiResult.ShouldBeOfType<ProblemHttpResult>();
        problemDetails.StatusCode.ShouldBe(StatusCodes.Status400BadRequest);
    }

    [Fact]
    public async Task ToNoContent_WithTaskOfResult_ReturnsNoContent()
    {
        // Arrange
        var result = Task.FromResult(Result.Success());

        // Act
        var apiResult = await result.ToNoContent();

        // Assert
        apiResult.ShouldBeOfType<NoContent>();
    }

    [Fact]
    public async Task ToNoContent_WithTaskOfResultOfValue_ReturnsNoContent()
    {
        // Arrange
        var result = Task.FromResult<Result<string>>("value");

        // Act
        var apiResult = await result.ToNoContent();

        // Assert
        apiResult.ShouldBeOfType<NoContent>();
    }

    [Fact]
    public async Task ToOk_WithValueTaskOfSuccessfulResult_ReturnsOkWithValue()
    {
        // Arrange
        var result = ValueTask.FromResult<Result<string>>("value");

        // Act
        var apiResult = await result.ToOk();

        // Assert
        apiResult.ShouldBeOfType<Ok<string>>().Value.ShouldBe("value");
    }

    [Fact]
    public async Task ToOk_WithValueTaskOfErrorResult_ReturnsProblem()
    {
        // Arrange
        var result = ValueTask.FromResult<Result<string>>(Result.NotFound("Todo item was not found."));

        // Act
        var apiResult = await result.ToOk();

        // Assert
        apiResult.ShouldBeOfType<ProblemHttpResult>().StatusCode.ShouldBe(StatusCodes.Status404NotFound);
    }

    [Fact]
    public async Task ToCreated_WithValueTaskOfSuccessfulResult_ReturnsCreated()
    {
        // Arrange
        var result = ValueTask.FromResult<Result<string>>("value");

        // Act
        var apiResult = await result.ToCreated(value => $"/items/{value}");

        // Assert
        apiResult.ShouldBeOfType<Created<string>>().Location.ShouldBe("/items/value");
    }

    [Fact]
    public async Task ToNoContent_WithValueTaskOfResult_ReturnsNoContent()
    {
        // Arrange
        var result = ValueTask.FromResult(Result.Success());

        // Act
        var apiResult = await result.ToNoContent();

        // Assert
        apiResult.ShouldBeOfType<NoContent>();
    }

    [Fact]
    public async Task ToNoContent_WithValueTaskOfResultOfValue_ReturnsNoContent()
    {
        // Arrange
        var result = ValueTask.FromResult<Result<string>>("value");

        // Act
        var apiResult = await result.ToNoContent();

        // Assert
        apiResult.ShouldBeOfType<NoContent>();
    }

    [Fact]
    public async Task Map_ThenToOk_WithValueTaskOfSuccessfulResult_ReturnsOkWithMappedValue()
    {
        // Arrange
        var result = ValueTask.FromResult<Result<string>>("value");

        // Act
        var apiResult = await result.Map(value => value.Length).ToOk();

        // Assert
        apiResult.ShouldBeOfType<Ok<int>>().Value.ShouldBe(5);
    }
}
