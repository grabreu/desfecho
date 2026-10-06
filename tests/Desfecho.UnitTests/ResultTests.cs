namespace Desfecho.UnitTests;

public class ResultTests
{
    [Fact]
    public void Success_CreatesSuccessfulResult()
    {
        // Act
        var result = Result.Success();

        // Assert
        result.IsSuccess.ShouldBeTrue();
        result.IsError.ShouldBeFalse();
    }

    [Fact]
    public void FromError_WithError_CreatesErrorResult()
    {
        // Act
        Result result = Result.NotFound("Todo item was not found.");

        // Assert
        result.IsSuccess.ShouldBeFalse();
        result.IsError.ShouldBeTrue();
        result.Error.ShouldBe(Result.NotFound("Todo item was not found."));
        result.Errors.ShouldHaveSingleItem();
    }

    [Fact]
    public void FromErrorList_WithErrors_CreatesErrorResultWithAllErrors()
    {
        // Arrange
        var errorList = Result.Invalid(new Dictionary<string, string[]> { ["Name"] = ["Required.", "Too long."] });

        // Act
        Result result = errorList;

        // Assert
        result.Errors.ShouldBe(errorList.Items);
        result.Error.ShouldBe(errorList.Items[0]);
    }

    [Fact]
    public void Errors_WithSuccessfulResult_ThrowsInvalidOperationException()
    {
        // Arrange
        var result = Result.Success();

        // Act & Assert
        Should.Throw<InvalidOperationException>(() => _ = result.Errors)
            .Message.ShouldBe("Result is successful; there are no errors.");
    }

    [Fact]
    public void Error_WithSuccessfulResult_ThrowsInvalidOperationException()
    {
        // Arrange
        var result = Result.Success();

        // Act & Assert
        Should.Throw<InvalidOperationException>(() => _ = result.Error);
    }

    [Fact]
    public void TryGetErrors_WithErrorResult_ReturnsTrueAndErrors()
    {
        // Arrange
        Result result = Result.Conflict("Already exists.");

        // Act
        var found = result.TryGetErrors(out var errors);

        // Assert
        found.ShouldBeTrue();
        errors.ShouldHaveSingleItem().ShouldBe(Result.Conflict("Already exists."));
    }

    [Fact]
    public void TryGetErrors_WithSuccessfulResult_ReturnsFalse()
    {
        // Act
        var found = Result.Success().TryGetErrors(out var errors);

        // Assert
        found.ShouldBeFalse();
        errors.ShouldBeNull();
    }

    [Fact]
    public void Match_WithSuccessfulResult_ExecutesOnSuccess()
    {
        // Act
        var matchResult = Result.Success().Match(() => "success", _ => "error");

        // Assert
        matchResult.ShouldBe("success");
    }

    [Fact]
    public void Match_WithErrorResult_ExecutesOnError()
    {
        // Arrange
        Result result = Result.Forbidden("Access denied.");

        // Act
        var matchResult = result.Match(() => "success", errors => $"error:{errors[0].Description}");

        // Assert
        matchResult.ShouldBe("error:Access denied.");
    }
}
