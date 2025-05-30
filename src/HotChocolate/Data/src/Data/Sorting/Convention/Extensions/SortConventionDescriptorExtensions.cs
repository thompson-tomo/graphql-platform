using HotChocolate.Data.Sorting;

namespace HotChocolate.Data;

public static class SortConventionDescriptorExtensions
{
    public static ISortConventionDescriptor AddDefaults(
        this ISortConventionDescriptor descriptor) =>
        descriptor.AddDefaultOperations().BindDefaultTypes().UseQueryableProvider();

    public static ISortConventionDescriptor AddDefaultOperations(
        this ISortConventionDescriptor descriptor)
    {
        ArgumentNullException.ThrowIfNull(descriptor);

        descriptor.Operation(DefaultSortOperations.Ascending).Name("ASC");
        descriptor.Operation(DefaultSortOperations.Descending).Name("DESC");
        return descriptor;
    }

    public static ISortConventionDescriptor BindDefaultTypes(
        this ISortConventionDescriptor descriptor)
    {
        ArgumentNullException.ThrowIfNull(descriptor);

        // bind string as it is a class to avoid SortFilterInputType<string>
        descriptor.BindRuntimeType<string, DefaultSortEnumType>();

        descriptor.DefaultBinding<DefaultSortEnumType>();

        return descriptor;
    }
}
