using System.Text.RegularExpressions;

namespace Aspiregregator.Frontend;

public static partial class SlugGenerator
{
    public static string GenerateSlug(string incoming)
    {
        var slug = incoming.ToLowerInvariant();

        slug = MatchCharacterInSetRegex().Replace(slug, "");
        slug = WhitespaceRegex().Replace(slug, " ").Trim();
        slug = slug.Replace(" ", "-");
        
        if (slug.Length > 50)
        {
            slug = slug[..50].Trim('-');
        }

        return slug;
    }

    [GeneratedRegex(@"[^a-z0-9\s-]")]
    private static partial Regex MatchCharacterInSetRegex();
    
    [GeneratedRegex(@"\s+")]    
    private static partial Regex WhitespaceRegex();
}
