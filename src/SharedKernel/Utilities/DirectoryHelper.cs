using System.Security;

namespace SharedKernel.Utilities;

/// <summary>
/// Helper class for directory-related operations.
/// </summary>
public static class DirectoryHelper
{
    /// <summary>
    /// Attempts to find a directory by searching up the directory tree.
    /// </summary>
    /// <param name="targetDir">The name of the directory to find.</param>
    /// <param name="directoryInfo">When this method returns, contains the DirectoryInfo if found, or null if not found.</param>
    /// <param name="currentDir">Optional starting directory. If not provided, starts from current directory.</param>
    /// <param name="maxDepth">Maximum number of parent directories to search. Defaults to 10.</param>
    /// <returns>True if the directory was found, false otherwise.</returns>
    /// <exception cref="ArgumentException">Thrown when targetDir is null or empty.</exception>
    public static bool TryGetDirectoryInfo(
        string targetDir,
        out DirectoryInfo? directoryInfo,
        string? currentDir = default,
        int maxDepth = 10)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(targetDir, nameof(targetDir));

        directoryInfo = null;
        
        try
        {
            var directory = new DirectoryInfo(currentDir ?? Directory.GetCurrentDirectory());
            var depth = 0;

            while (directory is not null && depth < maxDepth)
            {
                if (DirectoryExists(directory, targetDir))
                {
                    directoryInfo = directory;
                    return true;
                }

                directory = directory.Parent;
                depth++;
            }

            return false;
        }
        catch (Exception ex) when (ex is SecurityException or UnauthorizedAccessException)
        {
            // Log the error if you have a logging system
            return false;
        }
    }

    /// <summary>
    /// Gets the absolute path of a directory by searching up the directory tree.
    /// </summary>
    /// <param name="targetDir">The name of the directory to find.</param>
    /// <param name="currentDir">Optional starting directory. If not provided, starts from current directory.</param>
    /// <returns>The absolute path if found, null otherwise.</returns>
    public static string? GetDirectoryPath(string targetDir, string? currentDir = default)
    {
        return TryGetDirectoryInfo(targetDir, out var dirInfo, currentDir) 
            ? dirInfo!.FullName 
            : null;
    }

    /// <summary>
    /// Ensures a directory exists, creating it if necessary.
    /// </summary>
    /// <param name="path">The path of the directory to ensure.</param>
    /// <returns>The DirectoryInfo of the ensured directory.</returns>
    public static DirectoryInfo EnsureDirectory(string path)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(path, nameof(path));

        var directory = new DirectoryInfo(path);
        
        if (!directory.Exists)
        {
            directory.Create();
        }

        return directory;
    }

    /// <summary>
    /// Safely checks if a directory exists within a parent directory.
    /// </summary>
    private static bool DirectoryExists(DirectoryInfo parent, string targetDir)
    {
        try
        {
            return parent.GetDirectories(targetDir).Length != 0;
        }
        catch (Exception ex) when (ex is SecurityException or UnauthorizedAccessException)
        {
            // Log the error if you have a logging system
            return false;
        }
    }
}
