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
        JSONItem? item = LoadJson();

        if (item != null)
        {
            var tags = item.JSONToTags();

            context.Database.Migrate();

            if (!context.Tags.Any())
            {
                context.Tags.AddRange(tags);
            }

            if (!context.Materials.Any())
            {
                context.Materials.AddRange(item.JSONToMaterials(tags));
            }

            context.SaveChanges();

        }
    }

    private static JSONItem? LoadJson()
    {
        using (StreamReader r = new StreamReader("Model/data.json"))
        {
            string json = r.ReadToEnd();
            return JsonConvert.DeserializeObject<JSONItem>(json);
        }

    }

    private class JSONItem
    {
        public IEnumerable<string> jsontags;
        public IEnumerable<JSONMaterial> jsonmaterials;

        public IEnumerable<Tag> JSONToTags()
        {
            List<Tag> output = new List<Tag>() { };
            foreach (var t in jsontags)
            {
                output.Add(new Tag() { Name = t });
            }
            return output;
        }

        public IEnumerable<Material> JSONToMaterials(IEnumerable<Tag> AllTags)
        {
            List<Material> output = new List<Material>() { };
            foreach (var m in jsonmaterials)
            {
                output.Add(new Material()
                {
                    Title = m.title,
                    Tags = AllTags.Where(t => m.tags.Contains(t.Name)).ToList<Tag>(),
                    ShortDescription = m.description,
                    Type = m.type,
                    Date = DateTime.Parse(m.date),
                    ImageUrl = "/Assets/img/react.jpg"
                });
            }
            return output;
        }
    }


    private class JSONMaterial
    {
        public string title;
        public List<string> tags;
        public string date;
        public string description;
        public string type;
    }
}
