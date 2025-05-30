namespace HotChocolate.Fusion.Logging;

public static class LogEntryCodes
{
    public const string DisallowedInaccessible = "DISALLOWED_INACCESSIBLE";
    public const string EmptyMergedEnumType = "EMPTY_MERGED_ENUM_TYPE";
    public const string EmptyMergedInputObjectType = "EMPTY_MERGED_INPUT_OBJECT_TYPE";
    public const string EmptyMergedInterfaceType = "EMPTY_MERGED_INTERFACE_TYPE";
    public const string EmptyMergedObjectType = "EMPTY_MERGED_OBJECT_TYPE";
    public const string EmptyMergedUnionType = "EMPTY_MERGED_UNION_TYPE";
    public const string EnumTypeDefaultValueInaccessible = "ENUM_TYPE_DEFAULT_VALUE_INACCESSIBLE";
    public const string EnumValuesMismatch = "ENUM_VALUES_MISMATCH";
    public const string ExternalArgumentDefaultMismatch = "EXTERNAL_ARGUMENT_DEFAULT_MISMATCH";
    public const string ExternalMissingOnBase = "EXTERNAL_MISSING_ON_BASE";
    public const string ExternalOnInterface = "EXTERNAL_ON_INTERFACE";
    public const string ExternalUnused = "EXTERNAL_UNUSED";
    public const string FieldArgumentTypesNotMergeable = "FIELD_ARGUMENT_TYPES_NOT_MERGEABLE";
    public const string FieldWithMissingRequiredArgument = "FIELD_WITH_MISSING_REQUIRED_ARGUMENT";
    public const string ImplementedByInaccessible = "IMPLEMENTED_BY_INACCESSIBLE";
    public const string InputFieldDefaultMismatch = "INPUT_FIELD_DEFAULT_MISMATCH";
    public const string InputFieldTypesNotMergeable = "INPUT_FIELD_TYPES_NOT_MERGEABLE";
    public const string InputWithMissingRequiredFields = "INPUT_WITH_MISSING_REQUIRED_FIELDS";
    public const string InterfaceFieldNoImplementation = "INTERFACE_FIELD_NO_IMPLEMENTATION";
    public const string InvalidGraphQL = "INVALID_GRAPHQL";
    public const string InvalidShareableUsage = "INVALID_SHAREABLE_USAGE";
    public const string IsInvalidField = "IS_INVALID_FIELD";
    public const string IsInvalidFieldType = "IS_INVALID_FIELD_TYPE";
    public const string IsInvalidSyntax = "IS_INVALID_SYNTAX";
    public const string IsInvalidUsage = "IS_INVALID_USAGE";
    public const string KeyDirectiveInFieldsArg = "KEY_DIRECTIVE_IN_FIELDS_ARG";
    public const string KeyFieldsHasArgs = "KEY_FIELDS_HAS_ARGS";
    public const string KeyFieldsSelectInvalidType = "KEY_FIELDS_SELECT_INVALID_TYPE";
    public const string KeyInvalidFields = "KEY_INVALID_FIELDS";
    public const string KeyInvalidFieldsType = "KEY_INVALID_FIELDS_TYPE";
    public const string KeyInvalidSyntax = "KEY_INVALID_SYNTAX";
    public const string LookupReturnsList = "LOOKUP_RETURNS_LIST";
    public const string LookupReturnsNonNullableType = "LOOKUP_RETURNS_NON_NULLABLE_TYPE";
    public const string NonNullInputFieldIsInaccessible = "NON_NULL_INPUT_FIELD_IS_INACCESSIBLE";
    public const string NoQueries = "NO_QUERIES";
    public const string OutputFieldTypesNotMergeable = "OUTPUT_FIELD_TYPES_NOT_MERGEABLE";
    public const string OverrideFromSelf = "OVERRIDE_FROM_SELF";
    public const string OverrideOnInterface = "OVERRIDE_ON_INTERFACE";
    public const string ProvidesDirectiveInFieldsArg = "PROVIDES_DIRECTIVE_IN_FIELDS_ARG";
    public const string ProvidesFieldsHasArgs = "PROVIDES_FIELDS_HAS_ARGS";
    public const string ProvidesFieldsMissingExternal = "PROVIDES_FIELDS_MISSING_EXTERNAL";
    public const string ProvidesInvalidFields = "PROVIDES_INVALID_FIELDS";
    public const string ProvidesInvalidFieldsType = "PROVIDES_INVALID_FIELDS_TYPE";
    public const string ProvidesInvalidSyntax = "PROVIDES_INVALID_SYNTAX";
    public const string ProvidesOnNonCompositeField = "PROVIDES_ON_NON_COMPOSITE_FIELD";
    public const string QueryRootTypeInaccessible = "QUERY_ROOT_TYPE_INACCESSIBLE";
    public const string RequireInvalidFields = "REQUIRE_INVALID_FIELDS";
    public const string RequireInvalidFieldType = "REQUIRE_INVALID_FIELD_TYPE";
    public const string RequireInvalidSyntax = "REQUIRE_INVALID_SYNTAX";
    public const string RootMutationUsed = "ROOT_MUTATION_USED";
    public const string RootQueryUsed = "ROOT_QUERY_USED";
    public const string RootSubscriptionUsed = "ROOT_SUBSCRIPTION_USED";
    public const string TypeDefinitionInvalid = "TYPE_DEFINITION_INVALID";
    public const string TypeKindMismatch = "TYPE_KIND_MISMATCH";
    public const string Unsatisfiable = "UNSATISFIABLE";
}
