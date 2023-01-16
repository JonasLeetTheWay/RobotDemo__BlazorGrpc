using BlazorGrpc.Shared.Domain;
using MongoDB.Bson;

namespace BlazorGrpc.Shared.DataGenerator;

public static class DomainDataGenerator
{

    public static Location GenerateLocation(string? name = "Test Location", double? x = 1.0, double? y = 2.0, string? id = default)
    {
        id = id ?? ObjectId.GenerateNewId().ToString();
        return new Location
        {
            Id = id,
            Name = name,
            X = x,
            Y = y
        };
    }
    public static Location GenerateLocationOnlyWithName(string? name)
    {
        return GenerateLocation(name, null, null);
    }
    public static Location GenerateLocationOnlyWithXY(double? x, double? y)
    {
        return GenerateLocation(null, x, y);
    }

    public static Location GenerateLocationOnlyWithId(string id)
    {
        return GenerateLocation(null, null, null, id);
    }
    public static Location GenerateLocationOnlyWithId(ObjectId id)
    {
        return GenerateLocation(null, null, null, id.ToString());
    }
    public static Location GenerateLocationOnlyWithId()
    {
        return GenerateLocation(null, null, null, default);
    }


    public static List<Location> GenerateRandomLocations(int quantity = 5, string? name = null, double? x = null, double? y = null)
    {
        List<Location> list = new();
        var rng = new Random();

        string name_for_nullName = "random";

        for (int i = 0; i < quantity; i++)
        {
            double x_val = x ?? Math.Round(rng.NextDouble() * 10, 1);
            double y_val = y ?? Math.Round(rng.NextDouble() * 10, 1);

            string locationName = name ?? name_for_nullName + (name == null ? i : "");
            list.Add(GenerateLocation(locationName, x_val, y_val));
        }
        return list;
    }

}
