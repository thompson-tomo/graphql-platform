using HotChocolate.Configuration;
using HotChocolate.Types.Descriptors;
using HotChocolate.Types.Descriptors.Configurations;

#nullable enable

namespace HotChocolate.Types;

public class InputObjectType<T> : InputObjectType
{
    private Action<IInputObjectTypeDescriptor<T>>? _configure;

    public InputObjectType(Action<IInputObjectTypeDescriptor<T>> configure)
    {
        _configure = configure ?? throw new ArgumentNullException(nameof(configure));
    }

    [ActivatorUtilitiesConstructor]
    public InputObjectType()
    {
        _configure = Configure;
    }

    protected override InputObjectTypeConfiguration CreateConfiguration(
        ITypeDiscoveryContext context)
    {
        var descriptor = InputObjectTypeDescriptor.New<T>(context.DescriptorContext);

        _configure!(descriptor);
        _configure = null;

        return descriptor.CreateConfiguration();
    }

    protected virtual void Configure(
        IInputObjectTypeDescriptor<T> descriptor)
    {
    }

    protected sealed override void Configure(
        IInputObjectTypeDescriptor descriptor)
        => throw new NotSupportedException();
}
