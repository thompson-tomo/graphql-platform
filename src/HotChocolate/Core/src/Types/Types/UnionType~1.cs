using HotChocolate.Configuration;
using HotChocolate.Types.Descriptors;
using HotChocolate.Types.Descriptors.Configurations;

#nullable enable

namespace HotChocolate.Types;

public class UnionType<T> : UnionType
{
    private Action<IUnionTypeDescriptor>? _configure;

    public UnionType(Action<IUnionTypeDescriptor> configure)
    {
        _configure = configure
            ?? throw new ArgumentNullException(nameof(configure));
    }

    [ActivatorUtilitiesConstructor]
    public UnionType()
    {
        _configure = Configure;
    }

    protected override UnionTypeConfiguration CreateConfiguration(ITypeDiscoveryContext context)
    {
        var descriptor =
            UnionTypeDescriptor.New(context.DescriptorContext, typeof(T));

        _configure!(descriptor);
        _configure = null;

        return descriptor.CreateConfiguration();
    }
}
