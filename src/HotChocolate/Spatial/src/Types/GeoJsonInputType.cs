using HotChocolate.Configuration;
using HotChocolate.Types.Descriptors.Configurations;
using HotChocolate.Types.Spatial.Serialization;

namespace HotChocolate.Types.Spatial;

public abstract class GeoJsonInputType<T>
    : InputObjectType<T>
    , IGeoJsonInputType
{
    private readonly IGeoJsonSerializer _serializer;

    protected GeoJsonInputType(GeoJsonGeometryType geometryType)
    {
        _serializer = GeoJsonSerializers.Serializers[geometryType];
    }

    protected override Func<object?[], object> OnCompleteCreateInstance(
        ITypeCompletionContext context,
        InputObjectTypeConfiguration definition)
        => CreateInstance;

    private object CreateInstance(object?[] fieldValues)
        => _serializer.CreateInstance(this, fieldValues);

    protected override Action<object, object?[]> OnCompleteGetFieldValues(
        ITypeCompletionContext context,
        InputObjectTypeConfiguration definition)
        => GetFieldData;

    private void GetFieldData(object runtimeValue, object?[] fieldValues)
        => _serializer.GetFieldData(this, runtimeValue, fieldValues);
}
