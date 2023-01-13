using BlazorGrpc.Server.Settings;
using Microsoft.Extensions.Options;
using MongoDB.Driver;

namespace BlazorGrpc.Server.Infrastructure
{
    public class MongoDBContext : IMongoDBContext
    {
        private IMongoDatabase _db { get; set; }
        private MongoClient _client { get; set; }

        public MongoDBContext(IOptions<MongoDBSettings> configuration)
        {
            _client = new MongoClient(configuration.Value.ConnectionString);
            _db = _client.GetDatabase(configuration.Value.DatabaseName);
        }

        public IMongoCollection<T> GetCollection<T>(string name)
        {
            if (string.IsNullOrEmpty(name))
            {
                Console.WriteLine("You have given null collection name!");
                return this.GetCollection<T>();
            }
            return _db.GetCollection<T>(name);
        }
        public IMongoCollection<T> GetCollection<T>()
        {
            return _db.GetCollection<T>(typeof(T).Name);
        }
    }
}