using Bogus;
using DotCruz.Tenants.Domain.ValueObjects.Tenants;

namespace CommonTestUtilities.ValueObjects.Tenants;

public class TenantSlugBuilder
{
    public static TenantSlug Build(
        bool isEmpty = false,
        bool isTooShort = false,
        bool isTooLong = false,
        bool isInvalidFormat = false,
        bool isReserved = false)
    {
        var faker = new Faker();

        var slug = faker.Internet.DomainWord();

        if (slug.Length < 3)
            slug = "slug-" + slug;
        if (slug.Length > 50)
            slug = slug.Substring(0, 50);

        if (isEmpty)
            slug = string.Empty;

        if (isTooShort)
            slug = faker.Random.String2(2, "abcdefghijklmnopqrstuvwxyz");

        if (isTooLong)
            slug = faker.Random.String2(51, "abcdefghijklmnopqrstuvwxyz");

        if (isInvalidFormat)
            slug = "invalid_slug_format!";

        if (isReserved)
            slug = faker.Random.ListItem(["admin", "api", "support", "billing", "mail", "portal", "auth", "root", "system"]);

        return new TenantSlug(slug);
    }
}
