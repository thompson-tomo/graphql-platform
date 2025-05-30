using HotChocolate.Language;
using HotChocolate.Types.Descriptors;
using HotChocolate.Types.Descriptors.Configurations;

namespace HotChocolate.Types.Factories;

internal sealed class EnumTypeFactory
    : ITypeFactory<EnumTypeDefinitionNode, EnumType>
    , ITypeFactory<EnumTypeExtensionNode, EnumTypeExtension>
{
    public EnumType Create(IDescriptorContext context, EnumTypeDefinitionNode node)
    {
        var path = context.GetOrCreateConfigurationStack();
        path.Clear();

        var typeDefinition = new EnumTypeConfiguration(
            node.Name.Value,
            node.Description?.Value)
        {
            BindTo = node.GetBindingValue()
        };

        SdlToTypeSystemHelper.AddDirectives(context, typeDefinition, node, path);

        DeclareValues(context, typeDefinition, node.Values, path);

        return EnumType.CreateUnsafe(typeDefinition);
    }

    public EnumTypeExtension Create(IDescriptorContext context, EnumTypeExtensionNode node)
    {
        var path = context.GetOrCreateConfigurationStack();
        path.Clear();

        var typeDefinition = new EnumTypeConfiguration(node.Name.Value)
        {
            BindTo = node.GetBindingValue()
        };

        SdlToTypeSystemHelper.AddDirectives(context, typeDefinition, node, path);

        DeclareValues(context, typeDefinition, node.Values, path);

        return EnumTypeExtension.CreateUnsafe(typeDefinition);
    }

    private static void DeclareValues(
        IDescriptorContext context,
        EnumTypeConfiguration parent,
        IReadOnlyCollection<EnumValueDefinitionNode> values,
        Stack<ITypeSystemConfiguration> path)
    {
        path.Push(parent);

        foreach (var value in values)
        {
            var valueDefinition = new EnumValueConfiguration(
                value.Name.Value,
                value.Description?.Value,
                value.Name.Value)
            {
                BindTo = value.GetBindingValue()
            };

            SdlToTypeSystemHelper.AddDirectives(context, valueDefinition, value, path);

            if (value.DeprecationReason() is { Length: > 0 } reason)
            {
                valueDefinition.DeprecationReason = reason;
            }

            parent.Values.Add(valueDefinition);
        }

        path.Pop();
    }
}
