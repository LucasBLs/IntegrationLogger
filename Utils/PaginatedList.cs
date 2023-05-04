using IntegrationLogger.Models;
using IntegrationLogger.Extensions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query;

namespace IntegrationLogger.Utils;
public class PaginatedList<T> : List<T>
{
    public int PageIndex { get; private set; }
    public int TotalPages { get; private set; }

    public PaginatedList(List<T> items, int count, int pageIndex, int pageSize)
    {
        PageIndex = pageIndex;
        TotalPages = (int)Math.Ceiling(count / (double)pageSize);

        this.AddRange(items);
    }

    public bool HasPreviousPage => PageIndex > 1;
    public bool HasNextPage => PageIndex < TotalPages;

    public static async Task<PaginatedList<T>> CreateAsync(IQueryable<T> source, int pageIndex = 1, int pageSize = 0, bool pagination = true)
    {
        int count;
        List<T> items;

        if (pagination && pageSize > 0)
        {
            if (source.Provider is IAsyncQueryProvider) // Entity Framework Core
            {
                count = await source.CountAsync();
                items = await source.Skip((pageIndex - 1) * pageSize).Take(pageSize).ToListAsync();
            }
            else // MongoDB
            {
                count = await source.MongoDbCountAsync();
                items = await source.Skip((pageIndex - 1) * pageSize).Take(pageSize).ToMongoDbListAsync();
            }
        }
        else
        {
            if (source.Provider is IAsyncQueryProvider) // Entity Framework Core
            {
                count = await source.CountAsync();
                items = await source.ToListAsync();
            }
            else // MongoDB
            {
                count = await source.MongoDbCountAsync();
                items = await source.ToMongoDbListAsync();
            }
        }

        return new PaginatedList<T>(items, count, pageIndex, pageSize);
    }
}