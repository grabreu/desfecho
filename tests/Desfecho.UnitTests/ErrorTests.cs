namespace Desfecho.UnitTests;

public class ErrorTests
{
    [Fact]
    public void Constructor_WithValidData_SetsExpectedProperties()
    {
        // Act
        var error = new Error(ErrorType.NotFound, "Todo item was not found.");

        // Assert
        error.Type.ShouldBe(ErrorType.NotFound);
        error.Description.ShouldBe("Todo item was not found.");
        error.Property.ShouldBeNull();
    }

    [Fact]
    public void Invalid_WithPropertyAndDescription_CreatesValidationError()
    {
        // Act
        var error = Result.Invalid("Title", "Title is required.");

        // Assert
        error.Type.ShouldBe(ErrorType.Validation);
        error.Property.ShouldBe("Title");
        error.Description.ShouldBe("Title is required.");
    }

    [Theory]
    [InlineData(ErrorType.Unauthorized)]
    [InlineData(ErrorType.Forbidden)]
    [InlineData(ErrorType.NotFound)]
    [InlineData(ErrorType.Conflict)]
    [InlineData(ErrorType.Failure)]
    public void Factory_WithDescription_CreatesErrorOfMatchingType(ErrorType type)
    {
        // Act
        var error = type switch
        {
            ErrorType.Unauthorized => Result.Unauthorized("description"),
            ErrorType.Forbidden => Result.Forbidden("description"),
            ErrorType.NotFound => Result.NotFound("description"),
            ErrorType.Conflict => Result.Conflict("description"),
            _ => Result.Failure("description")
        };

        // Assert
        error.Type.ShouldBe(type);
        error.Description.ShouldBe("description");
        error.Property.ShouldBeNull();
    }
}
