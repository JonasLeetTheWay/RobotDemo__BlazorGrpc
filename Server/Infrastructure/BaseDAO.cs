using BlazorGrpc.Shared.Domain;
using MongoDB.Bson;
using MongoDB.Driver;

namespace BlazorGrpc.Server.Infrastructure;


public class BaseDAO<T> where T : Entity
{
    private readonly IMongoCollection<T> _collection;
    private readonly ILogger<T>? _logger;

    public BaseDAO(IMongoDBContext context, ILogger<T>? logger)
    {
        _collection = context.GetCollection<T>();
        _logger = logger;
    }

    public async Task<IEnumerable<T>> GetAll()
    {
        try
        {
            return await _collection.Find(new BsonDocument()).ToListAsync();
        }
        catch (Exception ex)
        {
            _logger?.LogError(ex, $"Error getting all {typeof(T).Name}");
            throw;
        }
    }

    public async Task<T> GetById(string id)
    {
        try
        {
            return await _collection.Find(x => x.Id == id).FirstOrDefaultAsync();
        }
        catch (Exception ex)
        {
            _logger?.LogError(ex, $"Error getting {typeof(T).Name} by Id: {id}");
            throw;
        }
    }

    public async Task<T> Create(T data)
    {
        try
        {
            await _collection.InsertOneAsync(data);
            return data;
        }
        catch (Exception ex)
        {
            _logger?.LogError(ex, $"Error creating {typeof(T).Name}");
            throw;
        }
    }

    public async Task<bool> Update(string id, T data)
    {
        try
        {
            var result = await _collection.ReplaceOneAsync(x => x.Id == id, data);
            return result.ModifiedCount > 0;
        }
        catch (Exception ex)
        {
            _logger?.LogError(ex, $"Error updating {typeof(T).Name} with Id: {id}");
            throw;
        }
    }

    public async Task<bool> Delete(string id)
    {
        try
        {
            var result = await _collection.DeleteOneAsync(x => x.Id == id);
            return result.DeletedCount > 0;
        }
        catch (Exception ex)
        {
            _logger?.LogError(ex, $"Error deleting {typeof(T).Name} with Id: {id}");
            throw;
        }
    }

    public async Task<bool> ClearCollection()
    {
        try
        {
            var result = await _collection.DeleteManyAsync(new BsonDocument());
            return result.DeletedCount > 0;
        }
        catch (Exception ex)
        {
            _logger?.LogError(ex, $"Error clearing {typeof(T).Name} collection");
            throw;
        }
    }
}


