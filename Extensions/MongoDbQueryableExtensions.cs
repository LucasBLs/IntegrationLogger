using MongoDB.Driver;
using MongoDB.Driver.Linq;

namespace IntegrationLogger.Extensions;
public static class MongoDbQueryableExtensions
{
    public static async Task<List<T>> ToMongoDbListAsync<T>(this IQueryable<T> source)
    {
        if (source is IMongoQueryable<T> mongoQueryable)
        {
            return await mongoQueryable.ToListAsync();
        }

        throw new System.InvalidOperationException("A fonte IQueryable<T> não é uma consulta do MongoDB.");
    }

    public static async Task<int> MongoDbCountAsync<T>(this IQueryable<T> source)
    {
        if (source is IMongoQueryable<T> mongoQueryable)
        {
            return await mongoQueryable.CountAsync();
        }

        throw new System.InvalidOperationException("A fonte IQueryable<T> não é uma consulta do MongoDB.");
    }
}