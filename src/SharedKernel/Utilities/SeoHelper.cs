using System.Text.RegularExpressions;

namespace SharedKernel.Utilities;

public static partial class SeoHelper
{
    public static string GenerateUrlSlug(string input)
    {
        // Convert to lowercase and trim
        string slug = input.Trim().ToLower();
        
        // Replace spaces with hyphens first
        slug = SpaceToHyphenRegex().Replace(slug, "-");
        
        // Split by forward slash to preserve them
        var segments = slug.Split('/', StringSplitOptions.RemoveEmptyEntries);
        
        // Clean each segment
        for (int i = 0; i < segments.Length; i++)
        {
            // Remove special characters but keep hyphens
            segments[i] = NonAlphaNumericRegex().Replace(segments[i], string.Empty)
                                               .Trim('-'); // Remove leading/trailing hyphens
            
            // Replace multiple hyphens with single hyphen
            segments[i] = MultipleHyphenRegex().Replace(segments[i], "-");
        }
        
        // Rejoin with forward slashes
        slug = string.Join("/", segments.Where(s => !string.IsNullOrEmpty(s)));
        
        return slug;
    }

    [GeneratedRegex(@"\s+")]
    private static partial Regex SpaceToHyphenRegex();

    [GeneratedRegex(@"[^a-z0-9-]")]
    private static partial Regex NonAlphaNumericRegex();

    [GeneratedRegex(@"-{2,}")]
    private static partial Regex MultipleHyphenRegex();
}
