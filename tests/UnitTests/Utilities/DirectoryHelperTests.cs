using FluentAssertions;
using SharedKernel.Utilities;

namespace eCommerceWeb.UnitTests.Utilities;

public class DirectoryHelperTests : IDisposable
{
    private readonly string _testDir = Path.Combine(Path.GetTempPath(), "DirectoryHelperTests");

    [Fact]
    public void TryGetDirectoryInfo_WithExistingDirectory_ShouldReturnTrue()
    {
        // Arrange
        var targetDir = "target";
        var path = CreateTestDirectory(targetDir);

        // Act
        var result = DirectoryHelper.TryGetDirectoryInfo(targetDir, out var dirInfo, _testDir);

        // Assert
        result.Should().BeTrue();
        dirInfo.Should().NotBeNull();
        dirInfo!.FullName.Should().Be(Path.GetDirectoryName(path));
    }

    [Fact]
    public void TryGetDirectoryInfo_WithNonExistentDirectory_ShouldReturnFalse()
    {
        // Act
        var result = DirectoryHelper.TryGetDirectoryInfo("nonexistent", out var dirInfo);

        // Assert
        result.Should().BeFalse();
        dirInfo.Should().BeNull();
    }

    [Theory]
    [InlineData("")]
    [InlineData(" ")]
    public void TryGetDirectoryInfo_WithInvalidInput_ShouldThrowException(string invalidDir)
    {
        // Act & Assert
        FluentActions.Invoking(() => DirectoryHelper.TryGetDirectoryInfo(invalidDir, out _))
            .Should().Throw<ArgumentException>();
    }

    [Fact]
    public void GetDirectoryPath_WithExistingDirectory_ShouldReturnPath()
    {
        // Arrange
        var targetDir = "target";
        var expectedPath = CreateTestDirectory(targetDir);

        // Act
        var result = DirectoryHelper.GetDirectoryPath(targetDir, _testDir);

        // Assert
        result.Should().NotBeNull();
        result.Should().Be(Path.GetDirectoryName(expectedPath));
    }

    [Fact]
    public void EnsureDirectory_WithNewPath_ShouldCreateDirectory()
    {
        // Arrange
        var newPath = Path.Combine(_testDir, "new_directory");

        // Act
        var result = DirectoryHelper.EnsureDirectory(newPath);

        // Assert
        result.Should().NotBeNull();
        result.Exists.Should().BeTrue();
        Directory.Exists(newPath).Should().BeTrue();
    }

    public void Dispose()
    {
        if (Directory.Exists(_testDir))
        {
            Directory.Delete(_testDir, true);
        }

        GC.SuppressFinalize(this);
    }

    private string CreateTestDirectory(string targetDir)
    {
        var path = Path.Combine(_testDir, targetDir);
        Directory.CreateDirectory(path);
        return path;
    }
} 