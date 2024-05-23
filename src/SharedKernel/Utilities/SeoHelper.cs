using System.Text.RegularExpressions;

namespace SharedKernel.Utilities;

public static partial class SeoHelper
{
    public static string GenerateUrlSlug(string input)
    {
        string slug = input.Trim().ToLower();
        slug = SpaceToHyphenRegex().Replace(slug, "-");
        slug = NonAlphaNumericRegex().Replace(slug, string.Empty);

        return slug;
    }

    [GeneratedRegex(@"\s+")]
    private static partial Regex SpaceToHyphenRegex();
    [GeneratedRegex(@"[^a-z0-9-]")]
    private static partial Regex NonAlphaNumericRegex();
}
