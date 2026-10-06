namespace Desfecho.UnitTests;

public class ResultOfTTests
{
    [Fact]
    public void FromValue_WithValue_CreatesSuccessfulResult()
    {
        // Act
        Result<string> result = "value";

        // Assert
        result.IsSuccess.ShouldBeTrue();
        result.IsError.ShouldBeFalse();
        result.Value.ShouldBe("value");
    }

    [Fact]
    public void Success_WithValue_CreatesSuccessfulResult()
    {
        // Act
        var result = Result.Success("value");

        // Assert
        result.Value.ShouldBe("value");
    }

    [Fact]
    public void FromError_WithFactoryError_CreatesErrorResult()
    {
        // Act
        Result<string> result = Result.NotFound("Todo item was not found.");

        // Assert
        result.IsSuccess.ShouldBeFalse();
        result.IsError.ShouldBeTrue();
        result.Error.ShouldBe(Result.NotFound("Todo item was not found."));
    }

    [Fact]
    public void Value_WithErrorResult_ThrowsInvalidOperationException()
    {
        // Arrange
        Result<string> result = Result.NotFound("Todo item was not found.");

        // Act & Assert
        Should.Throw<InvalidOperationException>(() => _ = result.Value)
            .Message.ShouldBe("Result is an error; there is no value.");
    }

    [Fact]
    public void Error_WithSuccessfulResult_ThrowsInvalidOperationException()
    {
        // Arrange
        Result<string> result = "value";

        // Act & Assert
        Should.Throw<InvalidOperationException>(() => _ = result.Error)
            .Message.ShouldBe("Result is successful; there are no errors.");
    }

    [Fact]
    public void TryGetValue_WithSuccessfulResult_ReturnsTrueAndValue()
    {
        // Arrange
        Result<string> result = "value";

        // Act
        var found = result.TryGetValue(out var value);

        // Assert
        found.ShouldBeTrue();
        value.ShouldBe("value");
    }

    [Fact]
    public void TryGetValue_WithErrorResult_ReturnsFalse()
    {
        // Arrange
        Result<string> result = Result.NotFound("Todo item was not found.");

        // Act
        var found = result.TryGetValue(out var value);

        // Assert
        found.ShouldBeFalse();
        value.ShouldBeNull();
    }

    [Fact]
    public void Match_WithSuccessfulResult_ExecutesOnSuccess()
    {
        // Arrange
        Result<string> result = "value";

        // Act
        var matchResult = result.Match(value => $"success:{value}", _ => "error");

        // Assert
        matchResult.ShouldBe("success:value");
    }

    [Fact]
    public void Match_WithErrorResult_ExecutesOnError()
    {
        // Arrange
        Result<string> result = Result.Conflict("Already exists.");

        // Act
        var matchResult = result.Match(_ => "success", errors => $"error:{errors[0].Description}");

        // Assert
        matchResult.ShouldBe("error:Already exists.");
    }

    [Fact]
    public void Map_WithSuccessfulResult_TransformsValue()
    {
        // Arrange
        Result<string> result = "value";

        // Act
        var mapped = result.Map(value => value.Length);

        // Assert
        mapped.Value.ShouldBe(5);
    }

    [Fact]
    public void Map_WithErrorResult_PropagatesErrorWithoutCallingMap()
    {
        // Arrange
        Result<string> result = Result.NotFound("Todo item was not found.");
        var called = false;

        // Act
        var mapped = result.Map(value =>
        {
            called = true;
            return value.Length;
        });

        // Assert
        called.ShouldBeFalse();
        mapped.Error.ShouldBe(Result.NotFound("Todo item was not found."));
    }

    [Fact]
    public void Then_WithSuccessfulResult_ReturnsNextResult()
    {
        // Arrange
        Result<string> result = "value";

        // Act
        var next = result.Then(value => Result.Success(value.Length));

        // Assert
        next.Value.ShouldBe(5);
    }

    [Fact]
    public void Then_WithErrorResult_PropagatesErrorWithoutCallingNext()
    {
        // Arrange
        Result<string> result = Result.NotFound("Todo item was not found.");
        var called = false;

        // Act
        var next = result.Then(value =>
        {
            called = true;
            return Result.Success(value.Length);
        });

        // Assert
        called.ShouldBeFalse();
        next.Error.ShouldBe(Result.NotFound("Todo item was not found."));
    }

    [Fact]
    public void Then_WithNextReturningResult_ReturnsNextResult()
    {
        // Arrange
        Result<string> result = "value";

        // Act
        var next = result.Then(_ => Result.Success());

        // Assert
        next.IsSuccess.ShouldBeTrue();
    }

    [Fact]
    public void Then_WithErrorResultAndNextReturningResult_PropagatesError()
    {
        // Arrange
        Result<string> result = Result.Forbidden("Access denied.");

        // Act
        var next = result.Then(_ => Result.Success());

        // Assert
        next.Error.ShouldBe(Result.Forbidden("Access denied."));
    }
}
