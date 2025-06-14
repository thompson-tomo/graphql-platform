using HotChocolate.Features;
using HotChocolate.Language;
using HotChocolate.Language.Visitors;
using HotChocolate.Types;

namespace HotChocolate.Validation.Rules;

/// <summary>
/// Every argument provided to a field or directive must be defined
/// in the set of possible arguments of that field or directive.
///
/// http://facebook.github.io/graphql/June2018/#sec-Argument-Names
///
/// AND
///
/// Fields and directives treat arguments as a mapping of argument name
/// to value.
///
/// More than one argument with the same name in an argument set
/// is ambiguous and invalid.
///
/// http://facebook.github.io/graphql/June2018/#sec-Argument-Uniqueness
///
/// AND
///
/// Arguments can be required. An argument is required if the argument
/// type is non‐null and does not have a default value. Otherwise,
/// the argument is optional.
///
/// http://facebook.github.io/graphql/June2018/#sec-Required-Arguments
/// </summary>
internal sealed class ArgumentVisitor()
    : TypeDocumentValidatorVisitor(new SyntaxVisitorOptions { VisitDirectives = true })
{

    protected override ISyntaxVisitorAction Enter(
        DocumentNode node,
        DocumentValidatorContext context)
    {
        // The document node is the root node that is entered once per visitation.
        // We use this hook to ensure that the argument visitor feature is created
        // and we can use it in consecutive visits of child nodes without extra
        // checks at each point.
        // We do use a GetOrSet here because the context is a pooled object.
        context.Features.GetOrSet<ArgumentVisitorFeature>();
        return base.Enter(node, context);
    }

    protected override ISyntaxVisitorAction Enter(
        FieldNode node,
        DocumentValidatorContext context)
    {
        if (IntrospectionFieldNames.TypeName.Equals(node.Name.Value, StringComparison.Ordinal))
        {
            var typeName = context.Types.Peek().NamedType().Name;

            ValidateArguments(
                context,
                node,
                node.Arguments,
                EmptyCollections.InputFieldDefinitions,
                field: new SchemaCoordinate(typeName, IntrospectionFieldNames.TypeName));

            return Skip;
        }

        if (context.Types.TryPeek(out var type)
            && type.NamedType() is IComplexTypeDefinition ot
            && ot.Fields.TryGetField(node.Name.Value, out var of))
        {
            ValidateArguments(context, node, node.Arguments, of.Arguments, field: of.Coordinate);
            context.OutputFields.Push(of);
            context.Types.Push(of.Type);
            return Continue;
        }

        context.UnexpectedErrorsDetected = true;
        return Skip;
    }

    protected override ISyntaxVisitorAction Leave(
        FieldNode node,
        DocumentValidatorContext context)
    {
        context.Types.Pop();
        context.OutputFields.Pop();
        return Continue;
    }

    protected override ISyntaxVisitorAction Enter(
        DirectiveNode node,
        DocumentValidatorContext context)
    {
        if (context.Schema.DirectiveDefinitions.TryGetDirective(node.Name.Value, out var d))
        {
            context.Directives.Push(d);

            ValidateArguments(
                context, node, node.Arguments,
                d.Arguments, directive: d);

            return Continue;
        }
        else
        {
            context.UnexpectedErrorsDetected = true;
            return Skip;
        }
    }

    protected override ISyntaxVisitorAction Leave(
        DirectiveNode node,
        DocumentValidatorContext context)
    {
        context.Directives.Pop();
        return Continue;
    }

    private static void ValidateArguments(
        DocumentValidatorContext context,
        ISyntaxNode node,
        IReadOnlyList<ArgumentNode> argumentNodes,
        IReadOnlyFieldDefinitionCollection<IInputValueDefinition> arguments,
        SchemaCoordinate? field = null,
        IDirectiveDefinition? directive = null)
    {
        var argumentNames = context.Features.GetRequired<ArgumentVisitorFeature>().ArgumentNames;
        argumentNames.Clear();

        foreach (var argument in argumentNodes)
        {
            if (arguments.TryGetField(argument.Name.Value, out var arg))
            {
                if (!argumentNames.Add(argument.Name.Value))
                {
                    context.ReportError(
                        context.ArgumentNotUnique(
                            argument,
                            field,
                            directive));
                }

                if (arg.Type.IsNonNullType()
                    && arg.DefaultValue.IsNull()
                    && argument.Value.IsNull())
                {
                    context.ReportError(
                        context.ArgumentRequired(
                            argument,
                            argument.Name.Value,
                            field,
                            directive));
                }
            }
            else
            {
                context.ReportError(
                    context.ArgumentDoesNotExist(
                        argument,
                        field,
                        directive));
            }
        }

        foreach (var argument in arguments)
        {
            if (argument.Type.IsNonNullType()
                && argument.DefaultValue.IsNull()
                && argumentNames.Add(argument.Name))
            {
                context.ReportError(
                    context.ArgumentRequired(
                        node,
                        argument.Name,
                        field,
                        directive));
            }
        }
    }

    private sealed class ArgumentVisitorFeature : ValidatorFeature
    {
        public HashSet<string> ArgumentNames { get; } = [];

        protected internal override void Reset() => ArgumentNames.Clear();
    }
}
