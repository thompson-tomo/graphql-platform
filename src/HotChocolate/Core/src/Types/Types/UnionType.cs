using HotChocolate.Configuration;
using HotChocolate.Internal;
using HotChocolate.Language.Utilities;
using HotChocolate.Properties;
using HotChocolate.Resolvers;
using HotChocolate.Types.Descriptors;
using HotChocolate.Types.Descriptors.Definitions;

#nullable enable

namespace HotChocolate.Types;

/// <summary>
/// GraphQL Unions represent an object that could be one of a list of GraphQL Object types,
/// but provides for no guaranteed fields between those types.
/// They also differ from interfaces in that Object types declare what interfaces
/// they implement, but are not aware of what unions contain them.
///
/// With interfaces and objects, only those fields defined on the type can be queried directly;
/// to query other fields on an interface, typed fragments must be used.
/// This is the same as for unions, but unions do not define any fields,
/// so no fields may be queried on this type without the use of type refining
/// fragments or inline fragments (with the exception of the meta-field __typename).
///
/// For example, we might define the following types:
///
/// <code>
/// union SearchResult = Photo | Person
///
/// type Person {
///   name: String
///   age: Int
/// }
///
/// type Photo {
///   height: Int
///   width: Int
/// }
///
/// type SearchQuery {
///   firstSearchResult: SearchResult
/// }
/// </code>
/// </summary>
public class UnionType
    : NamedTypeBase<UnionTypeConfiguration>
    , IUnionType
{
    private const string _typeReference = "typeReference";

    private readonly Dictionary<string, ObjectType> _typeMap = new();

    private Action<IUnionTypeDescriptor>? _configure;
    private ResolveAbstractType? _resolveAbstractType;

    /// <summary>
    /// Initializes a new  instance of <see cref="UnionType"/>.
    /// </summary>
    protected UnionType()
    {
        _configure = Configure;
    }

    /// <summary>
    /// Initializes a new  instance of <see cref="UnionType"/>.
    /// </summary>
    /// <param name="configure">
    /// A delegate to specify the properties of this type.
    /// </param>
    /// <exception cref="ArgumentNullException">
    /// <paramref name="configure"/> is <c>null</c>.
    /// </exception>
    public UnionType(Action<IUnionTypeDescriptor> configure)
    {
        _configure = configure ?? throw new ArgumentNullException(nameof(configure));
    }

    /// <summary>
    /// Create a union type from a type definition.
    /// </summary>
    /// <param name="definition">
    /// The union type definition that specifies the properties of the
    /// newly created union type.
    /// </param>
    /// <returns>
    /// Returns the newly created union type.
    /// </returns>
    public static UnionType CreateUnsafe(UnionTypeConfiguration definition)
        => new() { Configuration = definition };

    /// <inheritdoc />
    public override TypeKind Kind => TypeKind.Union;

    /// <summary>
    /// Gets the <see cref="IObjectType" /> set of this union type.
    /// </summary>
    public IReadOnlyDictionary<string, ObjectType> Types => _typeMap;

    IReadOnlyCollection<IObjectType> IUnionType.Types => _typeMap.Values;

    /// <inheritdoc />
    public override bool IsAssignableFrom(INamedType namedType)
    {
        switch (namedType.Kind)
        {
            case TypeKind.Union:
                return ReferenceEquals(namedType, this);

            case TypeKind.Object:
                return _typeMap.ContainsKey(((ObjectType)namedType).Name);

            default:
                return false;
        }
    }

    /// <summary>
    /// Checks if the type set of this union type contains the
    /// specified <paramref name="objectType"/>.
    /// </summary>
    /// <param name="objectType">
    /// The object type.
    /// </param>
    /// <returns>
    /// Returns <c>true</c>, if the type set of this union type contains the
    /// specified <paramref name="objectType"/>; otherwise, <c>false</c> is returned.
    /// </returns>
    public bool ContainsType(ObjectType objectType)
    {
        if (objectType is null)
        {
            throw new ArgumentNullException(nameof(objectType));
        }

        return _typeMap.ContainsKey(objectType.Name);
    }

    bool IUnionType.ContainsType(IObjectType objectType)
    {
        if (objectType is null)
        {
            throw new ArgumentNullException(nameof(objectType));
        }

        return _typeMap.ContainsKey(objectType.Name);
    }

    /// <inheritdoc />
    public bool ContainsType(string typeName)
    {
        if (string.IsNullOrEmpty(typeName))
        {
            throw new ArgumentNullException(nameof(typeName));
        }

        return _typeMap.ContainsKey(typeName);
    }

    /// <summary>
    /// Resolves the concrete type for the value of a type
    /// that implements this interface.
    /// </summary>
    /// <param name="context">
    /// The resolver context.
    /// </param>
    /// <param name="resolverResult">
    /// The value for which the type shall be resolved.
    /// </param>
    /// <returns>
    /// Returns <c>null</c> if the value is not of a type
    /// implementing this interface.
    /// </returns>
    public ObjectType? ResolveConcreteType(IResolverContext context, object resolverResult)
        => _resolveAbstractType?.Invoke(context, resolverResult);

    IObjectType? IUnionType.ResolveConcreteType(IResolverContext context, object resolverResult)
        => ResolveConcreteType(context, resolverResult);

    protected override UnionTypeConfiguration CreateConfiguration(ITypeDiscoveryContext context)
    {
        try
        {
            if (Configuration is null)
            {
                var descriptor = UnionTypeDescriptor.FromSchemaType(
                    context.DescriptorContext,
                    GetType());
                _configure!(descriptor);
                return descriptor.CreateConfiguration();
            }

            return Configuration;
        }
        finally
        {
            _configure = null;
        }
    }

    protected virtual void Configure(IUnionTypeDescriptor descriptor) { }

    protected override void OnRegisterDependencies(
        ITypeDiscoveryContext context,
        UnionTypeConfiguration configuration)
    {
        base.OnRegisterDependencies(context, configuration);

        foreach (var typeRef in configuration.Types)
        {
            context.Dependencies.Add(new(typeRef));
        }

        TypeDependencyHelper.CollectDirectiveDependencies(configuration, context.Dependencies);

        SetTypeIdentity(typeof(UnionType<>));
    }

    protected override void OnCompleteType(
        ITypeCompletionContext context,
        UnionTypeConfiguration configuration)
    {
        base.OnCompleteType(context, configuration);

        CompleteTypeSet(context, configuration);
        CompleteResolveAbstractType(configuration.ResolveAbstractType);
    }

    private void CompleteTypeSet(
        ITypeCompletionContext context,
        UnionTypeConfiguration definition)
    {
        var typeSet = new HashSet<ObjectType>();

        OnCompleteTypeSet(context, definition, typeSet);

        foreach (var objectType in typeSet)
        {
            _typeMap[objectType.Name] = objectType;
        }

        if (typeSet.Count is 0)
        {
            context.ReportError(SchemaErrorBuilder.New()
                .SetMessage(TypeResources.UnionType_MustHaveTypes)
                .SetCode(ErrorCodes.Schema.MissingType)
                .SetTypeSystemObject(this)
                .Build());
        }
    }

    protected virtual void OnCompleteTypeSet(
        ITypeCompletionContext context,
        UnionTypeConfiguration definition,
        ISet<ObjectType> typeSet)
    {
        foreach (var typeReference in definition.Types)
        {
            if (context.TryGetType(typeReference, out IType? type))
            {
                if (type is NonNullType nonNullType)
                {
                    type = nonNullType.NullableType;
                }

                if (type is not ObjectType objectType)
                {
                    context.ReportError(SchemaErrorBuilder.New()
                        .SetMessage(
                            "The provided type `{0}` is not an object type and cannot be part of a union type.",
                            type.ToTypeNode().Print())
                        .SetCode(ErrorCodes.Schema.MissingType)
                        .SetTypeSystemObject(this)
                        .SetExtension(_typeReference, typeReference)
                        .Build());
                    continue;
                }

                typeSet.Add(objectType);
            }
            else
            {
                context.ReportError(SchemaErrorBuilder.New()
                    .SetMessage(TypeResources.UnionType_UnableToResolveType)
                    .SetCode(ErrorCodes.Schema.MissingType)
                    .SetTypeSystemObject(this)
                    .SetExtension(_typeReference, typeReference)
                    .Build());
            }
        }
    }

    private void CompleteResolveAbstractType(
        ResolveAbstractType? resolveAbstractType)
    {
        if (resolveAbstractType is null)
        {
            // if there is no custom type resolver we will use this default
            // abstract type resolver.
            _resolveAbstractType = (c, r) =>
            {
                foreach (var type in _typeMap.Values)
                {
                    if (type.IsInstanceOfType(c, r))
                    {
                        return type;
                    }
                }

                return null;
            };
        }
        else
        {
            _resolveAbstractType = resolveAbstractType;
        }
    }
}
