using Slugify;

namespace RipperdocShop.Api.Utils;

public static class SlugGenerator
{
    public static string GenerateSlug(string input)
    {
        var helper = new SlugHelper();
        return helper.GenerateSlug(input);
    }
}
