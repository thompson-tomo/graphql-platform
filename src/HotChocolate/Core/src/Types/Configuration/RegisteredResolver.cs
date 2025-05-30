using HotChocolate.Resolvers;

#nullable enable

namespace HotChocolate.Configuration;

internal sealed class RegisteredResolver
{
    public RegisteredResolver(
        Type? resolverType,
        Type sourceType,
        IFieldReference field)
    {
        ResolverType = resolverType ?? sourceType;
        SourceType = sourceType ?? throw new ArgumentNullException(nameof(sourceType));
        Field = field ?? throw new ArgumentNullException(nameof(field));
    }

    public Type ResolverType { get; }

    public Type SourceType { get; }

    public IFieldReference Field { get; }

    public bool IsSourceResolver => ResolverType == SourceType;

    public RegisteredResolver WithField(IFieldReference field)
    {
        ArgumentNullException.ThrowIfNull(field);

        return new RegisteredResolver(
            ResolverType, SourceType,
            field);
    }

    public RegisteredResolver WithSourceType(Type sourceType)
    {
        ArgumentNullException.ThrowIfNull(sourceType);

        return new RegisteredResolver(
            ResolverType, sourceType,
            Field);
    }
}
