using MongoDB.Driver;

namespace BlazorGrpc.Server.Infrastructure
{
    public interface IMongoDBContext
    {
        IMongoCollection<T> GetCollection<T>();
        IMongoCollection<T> GetCollection<T>(string name);

    }
}