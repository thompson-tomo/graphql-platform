using HotChocolate.Fusion.Types.Collections;
using HotChocolate.Language;
using HotChocolate.Types;
using DirectiveLocation = HotChocolate.Types.DirectiveLocation;
using static HotChocolate.Serialization.SchemaDebugFormatter;

namespace HotChocolate.Fusion.Types;

/// <summary>
/// Represents a GraphQL directive definition.
/// </summary>
public sealed class FusionDirectiveDefinition : IDirectiveDefinition
{
    /// <summary>
    /// Represents a GraphQL directive definition.
    /// </summary>
    public FusionDirectiveDefinition(string name,
        string? description,
        bool isRepeatable,
        FusionInputFieldDefinitionCollection arguments,
        DirectiveLocation locations)
    {
        Name = name;
        Description = description;
        IsRepeatable = isRepeatable;
        Arguments = arguments;
        Locations = locations;
    }

    /// <summary>
    /// Gets the name of the directive.
    /// </summary>
    /// <value>
    /// The name of the directive.
    /// </value>
    public string Name { get; }

    /// <summary>
    /// Gets the description of the directive.
    /// </summary>
    /// <value>
    /// The description of the directive.
    /// </value>
    public string? Description { get; }

    /// <inheritdoc />
    public SchemaCoordinate Coordinate => new(Name, ofDirective: true);

    /// <summary>
    /// Defines if this directive is repeatable and can be applied multiple times.
    /// </summary>
    public bool IsRepeatable { get; }

    /// <summary>
    /// Gets the arguments that are defined on this directive.
    /// </summary>
    public FusionInputFieldDefinitionCollection Arguments { get; }

    IReadOnlyFieldDefinitionCollection<IInputValueDefinition> IDirectiveDefinition.Arguments
        => Arguments;

    /// <summary>
    /// Gets the locations where this directive can be applied.
    /// </summary>
    /// <value>
    /// The locations where this directive can be applied.
    /// </value>
    public DirectiveLocation Locations { get; }

    /// <summary>
    /// Gets the runtime type of the directive.
    /// </summary>
    public Type RuntimeType { get; } = typeof(object);

    /// <summary>
    /// Gets a string that represents the current object.
    /// </summary>
    /// <returns>
    /// A string that represents the current object.
    /// </returns>
    public override string ToString() => Format(this).ToString(true);

    /// <summary>
    /// Creates a <see cref="DirectiveDefinitionNode"/>
    /// from a <see cref="FusionDirectiveDefinition"/>.
    /// </summary>
    public DirectiveDefinitionNode ToSyntaxNode() => Format(this);

    ISyntaxNode ISyntaxNodeProvider.ToSyntaxNode() => Format(this);
}
