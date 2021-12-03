namespace blueberry.Server.Model;

public static class SeedExtensions
{
    public static IHost Seed(this IHost host)
    {
        using (var scope = host.Services.CreateScope())
        {
            var context = scope.ServiceProvider.GetRequiredService<BlueberryContext>();

            SeedMaterialsAndTags(context);
        }
        return host;
    }

    private static void SeedMaterialsAndTags(BlueberryContext context)
    {
        context.Database.Migrate();

        var react = new Tag { Name = "React" };
        var blazor = new Tag { Name = "Blazor" };
        var bootstrap = new Tag { Name = "Bootstrap" };
        var framework = new Tag { Name = "Framework" };

        if (!context.Tags.Any())
        {
            context.Tags.AddRange(
                react, blazor, bootstrap, framework
            );
        }

        if (!context.Materials.Any())
        {
            context.Materials.AddRange(
            new Material { Title = "Introduction to React", Tags = new List<Tag>() { react, framework }, Date = new DateTime(2020, 4, 11), Type = "Video", ShortDescription = "" },
            new Material { Title = "Introduction to Blazor", Tags = new List<Tag>() { blazor, framework }, Date = new DateTime(2021, 2, 8), Type = "Article", ShortDescription = "" },
            new Material { Title = "Introduction to Bootstrap", Tags = new List<Tag>() { bootstrap, framework }, Date = new DateTime(2018, 11, 23), Type = "Article", ShortDescription = "" },
            new Material { Title = "Using Blazor with Bootstrap", Tags = new List<Tag>() { blazor, bootstrap, framework }, Date = new DateTime(2021, 8, 29), Type = "Article", ShortDescription = "" },
            new Material { Title = "Using React with Bootstrap", Tags = new List<Tag>() { react, bootstrap, framework }, Date = new DateTime(2019, 9, 4), Type = "Video", ShortDescription = "" }
            );
        }

        context.SaveChanges();
    }
}
