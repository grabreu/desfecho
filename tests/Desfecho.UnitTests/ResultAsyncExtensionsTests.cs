namespace Desfecho.UnitTests;

public class ResultAsyncExtensionsTests
{
    [Fact]
    public async Task Map_WithTaskOfSuccessfulResult_TransformsValue()
    {
        // Arrange
        var result = Task.FromResult<Result<string>>("value");

        // Act
        var mapped = await result.Map(value => value.Length);

        // Assert
        mapped.Value.ShouldBe(5);
    }

    [Fact]
    public async Task Map_WithTaskOfErrorResult_PropagatesErrorWithoutCallingMap()
    {
        // Arrange
        var result = Task.FromResult<Result<string>>(Result.NotFound("Todo item was not found."));
        var called = false;

        // Act
        var mapped = await result.Map(value =>
        {
            called = true;
            return value.Length;
        });

        // Assert
        called.ShouldBeFalse();
        mapped.Error.ShouldBe(Result.NotFound("Todo item was not found."));
    }

    [Fact]
    public async Task Map_WithValueTaskOfSuccessfulResult_TransformsValue()
    {
        // Arrange
        var result = ValueTask.FromResult<Result<string>>("value");

        // Act
        var mapped = await result.Map(value => value.Length);

        // Assert
        mapped.Value.ShouldBe(5);
    }

    [Fact]
    public async Task Map_WithValueTaskOfErrorResult_PropagatesErrorWithoutCallingMap()
    {
        // Arrange
        var result = ValueTask.FromResult<Result<string>>(Result.NotFound("Todo item was not found."));
        var called = false;

        // Act
        var mapped = await result.Map(value =>
        {
            called = true;
            return value.Length;
        });

        // Assert
        called.ShouldBeFalse();
        mapped.Error.ShouldBe(Result.NotFound("Todo item was not found."));
    }
}
