using HotChocolate.Types.Descriptors.Configurations;

namespace HotChocolate.Types.Descriptors;

internal class NamedDependencyDescriptor
    : DependencyDescriptorBase
    , INamedDependencyDescriptor
{
    public NamedDependencyDescriptor(
        ITypeInspector typeInspector,
        OnCompleteTypeSystemConfigurationTask configuration)
        : base(typeInspector, configuration)
    {
    }

    protected override TypeDependencyFulfilled DependencyFulfilled
        => TypeDependencyFulfilled.Named;

    public INamedDependencyDescriptor DependsOn<TType>()
        where TType : ITypeSystemMember
        => DependsOn<TType>(false);

    public new INamedDependencyDescriptor DependsOn<TType>(bool mustBeNamed)
        where TType : ITypeSystemMember
    {
        base.DependsOn<TType>(mustBeNamed);
        return this;
    }

    public INamedDependencyDescriptor DependsOn(Type schemaType)
        => DependsOn(schemaType, false);

    public new INamedDependencyDescriptor DependsOn(Type schemaType, bool mustBeNamed)
    {
        base.DependsOn(schemaType, mustBeNamed);
        return this;
    }

    public INamedDependencyDescriptor DependsOn(string typeName)
        => DependsOn(typeName, false);

    public new INamedDependencyDescriptor DependsOn(string typeName, bool mustBeNamed)
    {
        base.DependsOn(typeName, mustBeNamed);
        return this;
    }

    public INamedDependencyDescriptor DependsOn(TypeReference typeReference)
        => DependsOn(typeReference, false);

    public new INamedDependencyDescriptor DependsOn(TypeReference typeReference, bool mustBeNamed)
    {
        base.DependsOn(typeReference, mustBeNamed);
        return this;
    }
}
