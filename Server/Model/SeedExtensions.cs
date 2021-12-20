using System.Text.Json;

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
                Console.WriteLine(tags.Count());
                context.Materials.AddRange(item.JSONToMaterials(tags));
            }

            context.SaveChanges();
        }
        else
        {
            throw new JsonException();
        }
    }

    private static JSONItem? LoadJson()
    {
        using (StreamReader r = new StreamReader("Model/data.json"))
        {
            JSONItem res = new JSONItem();
            string json = r.ReadToEnd();
            JsonDocument parsedJson = JsonDocument.Parse(json);
            var tags = parsedJson.RootElement.GetProperty("tags").EnumerateArray();
            res.tags = tags.Select(t => t.ToString()).ToList();

            var materials = parsedJson.RootElement
                                      .GetProperty("materials")
                                      .EnumerateArray()
                                      .Select(mj =>
                                      {
                                          var mTags = mj.GetProperty("tags").EnumerateArray().Select(t => t.ToString()).ToList();
                                          var res = new JSONMaterial(
                                            title: mj.GetProperty("title").ToString(),
                                            date: mj.GetProperty("date").ToString(),
                                            description: mj.GetProperty("description").ToString(),
                                            type: mj.GetProperty("type").ToString()
                                          )
                                          {
                                              Tags = mTags
                                          };

                                          return res;
                                      });

            res.materials = materials;

            return res;
        }

    }

    private class JSONItem
    {
        public IEnumerable<string> tags = new PrimitiveCollection<string>();
        public IEnumerable<JSONMaterial> materials = new PrimitiveCollection<JSONMaterial>();

        public IEnumerable<Tag> JSONToTags()
        {
            List<Tag> output = new List<Tag>() { };
            foreach (var t in tags)
            {
                output.Add(new Tag(name: t));
            }
            return output;
        }

        public IEnumerable<Material> JSONToMaterials(IEnumerable<Tag> AllTags)
        {
            List<Material> output = new List<Material>() { };
            foreach (var m in materials)
            {
                output.Add(new Material(title: m.Title, shortDescription: m.Description, type: m.Type, date: DateTime.Parse(m.Date))
                {
                    Tags = AllTags.Where(t => m.Tags.Contains(t.Name)).ToList<Tag>(),
                    ImageUrl = "/Assets/img/react.jpg"
                });
            }
            return output;
        }
    }


    private class JSONMaterial
    {
        public string Title { get; init; }
        public List<string> Tags { get; init; } = new List<string>();
        public string Date { get; init; }
        public string Description { get; init; }
        public string Type { get; init; }

        public JSONMaterial(string title, string date, string description, string type)
        {
            Title = title;
            Date = date;
            Description = description;
            Type = type;
        }
    }
}
