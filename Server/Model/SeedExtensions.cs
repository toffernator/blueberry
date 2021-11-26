namespace blueberry.Server.Model;

public static class SeedExtensions
{
    public static IHost Seed(this IHost host)
    {
        using (var scope = host.Services.CreateScope())
        {
            var context = scope.ServiceProvider.GetRequiredService<BlueberryContext>();

            SeedMaterials(context);
        }
        return host;
    }

    private static void SeedMaterials(BlueberryContext context)
    {
        context.Database.Migrate();

        var react = new Tag("React");
        var blazor = new Tag("Blazor");
        var bootstrap = new Tag("Bootstrap");
        var framework = new Tag("Framework");

        if (!context.Tags.Any())
        {
            context.Tags.AddRange(
                react, blazor, bootstrap, framework
            );
        }

        if (!context.Materials.Any())
        {
            context.Materials.AddRange(
            new Material { Title = "Introduction to React", Tags = new List<Tag>() { react, framework }, Date = new DateTime(2020, 4, 11) },
            new Material { Title = "Introduction to Blazor", Tags = new List<Tag>() { blazor, framework }, Date = new DateTime(2021, 2, 8) },
            new Material { Title = "Introduction to Bootstrap", Tags = new List<Tag>() { bootstrap, framework }, Date = new DateTime(2018, 11, 23) },
            new Material { Title = "Using Blazor with Bootstrap", Tags = new List<Tag>() { blazor, bootstrap, framework }, Date = new DateTime(2021, 8, 29) },
            new Material { Title = "Using React with Bootstrap", Tags = new List<Tag>() { react, bootstrap, framework }, Date = new DateTime(2019, 9, 4) }
            );
        }

        context.SaveChanges();
    }
}
