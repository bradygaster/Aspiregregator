using System.Text.RegularExpressions;

namespace Aspiregregator.Frontend;

public static class SlugGenerator
{
    public static string GenerateSlug(string incoming)
    {
        string slug = incoming.ToLowerInvariant();
        slug = Regex.Replace(slug, @"[^a-z0-9\s-]", "");
        slug = Regex.Replace(slug, @"\s+", " ").Trim();
        slug = slug.Replace(" ", "-");
        if (slug.Length > 50)
        {
            slug = slug.Substring(0, 50).Trim('-');
        }
        return slug;
    }
}
