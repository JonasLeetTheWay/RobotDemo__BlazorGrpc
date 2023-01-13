using BlazorGrpc.Shared.Domain;
using MongoDB.Bson;

namespace BlazorGrpc.Shared.DataGenerator;

public static class DomainDataGenerator
{
    public static Location GenerateLocation(string name, double x, double y, string? id = default)
    {
        return new Location
        {
            Id = id ?? ObjectId.GenerateNewId().ToString(),
            Name = name,
            X = x,
            Y = y
        };
    }

    public static List<Location> GenerateRandomLocations(int quantity = 5)
    {
        string name = "random";
        var rng = new Random();
        List<Location> list = new();
        // generate random double value, only amount to 2 decimal
        for (int i = 0; i < quantity; i++)
        {
            list.Add(GenerateLocation(name + i, rng.NextDouble() * 10, rng.NextDouble() * 10));
        }
        return list;
    }

}