namespace Desfecho.UnitTests;

public class ErrorTests
{
    [Fact]
    public void Constructor_WithValidData_SetsExpectedProperties()
    {
        // Act
        var error = new Error("TodoItem.NotFound", "Todo item was not found.", ErrorType.NotFound);

        // Assert
        error.Code.ShouldBe("TodoItem.NotFound");
        error.Description.ShouldBe("Todo item was not found.");
        error.Type.ShouldBe(ErrorType.NotFound);
    }

    [Fact]
    public void Validation_WithValidData_SetsExpectedProperties()
    {
        // Act
        var error = Error.Validation("TodoItem.Title", "Title is required.");

        // Assert
        error.Code.ShouldBe("TodoItem.Title");
        error.Description.ShouldBe("Title is required.");
        error.Type.ShouldBe(ErrorType.Validation);
    }

    [Fact]
    public void Unauthorized_WithValidData_SetsExpectedProperties()
    {
        // Act
        var error = Error.Unauthorized("TodoItem.Unauthorized", "Authentication is required.");

        // Assert
        error.Code.ShouldBe("TodoItem.Unauthorized");
        error.Description.ShouldBe("Authentication is required.");
        error.Type.ShouldBe(ErrorType.Unauthorized);
    }

    [Fact]
    public void Forbidden_WithValidData_SetsExpectedProperties()
    {
        // Act
        var error = Error.Forbidden("TodoItem.Forbidden", "You do not have access to this todo item.");

        // Assert
        error.Code.ShouldBe("TodoItem.Forbidden");
        error.Description.ShouldBe("You do not have access to this todo item.");
        error.Type.ShouldBe(ErrorType.Forbidden);
    }

    [Fact]
    public void NotFound_WithValidData_SetsExpectedProperties()
    {
        // Act
        var error = Error.NotFound("TodoItem.NotFound", "Todo item was not found.");

        // Assert
        error.Code.ShouldBe("TodoItem.NotFound");
        error.Description.ShouldBe("Todo item was not found.");
        error.Type.ShouldBe(ErrorType.NotFound);
    }

    [Fact]
    public void Conflict_WithValidData_SetsExpectedProperties()
    {
        // Act
        var error = Error.Conflict("TodoItem.Conflict", "Todo item already exists.");

        // Assert
        error.Code.ShouldBe("TodoItem.Conflict");
        error.Description.ShouldBe("Todo item already exists.");
        error.Type.ShouldBe(ErrorType.Conflict);
    }
}
