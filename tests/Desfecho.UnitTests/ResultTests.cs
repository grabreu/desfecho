namespace Desfecho.UnitTests;

public class ResultTests
{
    private static readonly Error s_validationError = Error.Validation("Name.Required", "Name is required.");

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
    public void FromError_WithError_CreatesErrorResult()
    {
        // Act
        Result<string> result = s_validationError;

        // Assert
        result.IsSuccess.ShouldBeFalse();
        result.IsError.ShouldBeTrue();
        result.Errors.ShouldHaveSingleItem().ShouldBe(s_validationError);
    }

    [Fact]
    public void FromErrors_WithMultipleErrors_CreatesErrorResult()
    {
        // Arrange
        var errors = new List<Error>
        {
            Error.Validation("Name.Required", "Name is required."),
            Error.Validation("Name.Length", "Name is too long.")
        };

        // Act
        Result<string> result = errors;

        // Assert
        result.IsSuccess.ShouldBeFalse();
        result.IsError.ShouldBeTrue();
        result.Errors.ShouldBe(errors);
    }

    [Fact]
    public void Value_WithErrorResult_ThrowsInvalidOperationException()
    {
        // Arrange
        Result<string> result = s_validationError;

        // Act & Assert
        Should.Throw<InvalidOperationException>(() => _ = result.Value)
            .Message.ShouldBe("Result is an error; there is no value.");
    }

    [Fact]
    public void Errors_WithSuccessfulResult_ThrowsInvalidOperationException()
    {
        // Arrange
        Result<string> result = "value";

        // Act & Assert
        Should.Throw<InvalidOperationException>(() => _ = result.Errors)
            .Message.ShouldBe("Result is successful; there are no errors.");
    }

    [Fact]
    public void Match_WithSuccessfulResult_ExecutesOnSuccess()
    {
        // Arrange
        Result<string> result = "value";

        // Act
        var matchResult = result.Match(
            value => $"success:{value}",
            _ => "error");

        // Assert
        matchResult.ShouldBe("success:value");
    }

    [Fact]
    public void Match_WithErrorResult_ExecutesOnError()
    {
        // Arrange
        Result<string> result = s_validationError;

        // Act
        var matchResult = result.Match(
            _ => "success",
            errors => $"error:{errors[0].Code}");

        // Assert
        matchResult.ShouldBe("error:Name.Required");
    }
}
