using System.Collections.Concurrent;
using HotChocolate.Execution;
using HotChocolate.Types;
using Microsoft.EntityFrameworkCore;

namespace HotChocolate.Data.Projections;

public class SchemaCache
    : ProjectionVisitorTestBase
    , IDisposable
{
    private readonly ConcurrentDictionary<(Type, object), IRequestExecutor> _cache = new();

    public IRequestExecutor CreateSchema<T>(
        T[] entities,
        Action<ModelBuilder>? onModelCreating = null,
        bool usePaging = false,
        bool useOffsetPaging = false,
        ITypeDefinition? objectType = null,
        Action<ISchemaBuilder>? configure = null,
        Type? schemaType = null)
        where T : class
    {
        (Type, T[] entites) key = (typeof(T), entities);

        return _cache.GetOrAdd(
            key,
            _ => base.CreateSchema(
                entities,
                onModelCreating: onModelCreating,
                usePaging: usePaging,
                useOffsetPaging: useOffsetPaging,
                objectType: objectType,
                configure: configure,
                schemaType: schemaType));
    }

    public void Dispose()
    {
    }
}
