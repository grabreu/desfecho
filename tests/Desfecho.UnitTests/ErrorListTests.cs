namespace Desfecho.UnitTests;

public class ErrorListTests
{
    [Fact]
    public void Constructor_WithErrors_SetsItems()
    {
        // Arrange
        Error[] errors = [Result.NotFound("Not found."), Result.Conflict("Conflict.")];

        // Act
        var errorList = new ErrorList(errors);

        // Assert
        errorList.Items.ShouldBe(errors);
    }

    [Fact]
    public void Constructor_WithNoErrors_ThrowsArgumentException()
    {
        // Act & Assert
        Should.Throw<ArgumentException>(() => new ErrorList([]));
    }

    [Fact]
    public void Invalid_WithFailures_CreatesOneValidationErrorPerMessage()
    {
        // Arrange
        IDictionary<string, string[]> failures = new Dictionary<string, string[]>
        {
            ["Name"] = ["Name is required.", "Name is too long."],
            ["Sku"] = ["SKU is required."]
        };

        // Act
        var errorList = Result.Invalid(failures);

        // Assert
        errorList.Items.ShouldBe(
        [
            Result.Invalid("Name", "Name is required."),
            Result.Invalid("Name", "Name is too long."),
            Result.Invalid("Sku", "SKU is required.")
        ]);
    }

    [Fact]
    public void Invalid_WithNoFailures_ThrowsArgumentException()
    {
        // Act & Assert
        Should.Throw<ArgumentException>(() => Result.Invalid([]));
    }
}
