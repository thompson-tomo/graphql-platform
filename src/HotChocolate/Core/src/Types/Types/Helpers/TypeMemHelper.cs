#nullable enable

using System.Reflection;
using HotChocolate.Types.Descriptors.Configurations;

namespace HotChocolate.Types.Helpers;

/// <summary>
/// This internal helper is used to centralize rented maps and list during type initialization.
/// This ensures that we can release these helper objects when the schema is created.
/// </summary>
internal static class TypeMemHelper
{
    private static Dictionary<string, ObjectFieldConfiguration>? _objectFieldConfigurationMap;
    private static Dictionary<string, InterfaceFieldConfiguration>? _interfaceFieldConfigurationMap;
    private static Dictionary<string, InputFieldConfiguration>? _inputFieldConfigurationMap;
    private static Dictionary<string, InputField>? _inputFieldMap;
    private static Dictionary<string, InputField>? _inputFieldMapOrdinalIgnoreCase;
    private static Dictionary<string, DirectiveArgument>? _directiveArgumentMap;
    private static Dictionary<string, DirectiveArgument>? _directiveArgumentMapOrdinalIgnoreCase;
    private static Dictionary<ParameterInfo, string>? _argumentNameMap;
    private static HashSet<MemberInfo>? _memberSet;
    private static HashSet<string>? _nameSet;
    private static HashSet<string>? _nameSetOrdinalIgnoreCase;

    public static Dictionary<string, ObjectFieldConfiguration> RentObjectFieldConfigurationMap()
        => Interlocked.Exchange(ref _objectFieldConfigurationMap, null) ??
            new Dictionary<string, ObjectFieldConfiguration>(StringComparer.Ordinal);

    public static void Return(Dictionary<string, ObjectFieldConfiguration> map)
    {
        map.Clear();
        Interlocked.CompareExchange(ref _objectFieldConfigurationMap, map, null);
    }

    public static Dictionary<string, InterfaceFieldConfiguration> RentInterfaceFieldConfigurationMap()
        => Interlocked.Exchange(ref _interfaceFieldConfigurationMap, null) ??
            new Dictionary<string, InterfaceFieldConfiguration>(StringComparer.Ordinal);

    public static void Return(Dictionary<string, InterfaceFieldConfiguration> map)
    {
        map.Clear();
        Interlocked.CompareExchange(ref _interfaceFieldConfigurationMap, map, null);
    }

    public static Dictionary<string, InputFieldConfiguration> RentInputFieldConfigurationMap()
        => Interlocked.Exchange(ref _inputFieldConfigurationMap, null) ??
            new Dictionary<string, InputFieldConfiguration>(StringComparer.Ordinal);

    public static void Return(Dictionary<string, InputFieldConfiguration> map)
    {
        map.Clear();
        Interlocked.CompareExchange(ref _inputFieldConfigurationMap, map, null);
    }

    public static Dictionary<string, InputField> RentInputFieldMap()
        => Interlocked.Exchange(ref _inputFieldMap, null) ??
            new Dictionary<string, InputField>(StringComparer.Ordinal);

    public static Dictionary<string, InputField> RentInputFieldMapOrdinalIgnoreCase()
        => Interlocked.Exchange(ref _inputFieldMapOrdinalIgnoreCase, null) ??
            new Dictionary<string, InputField>(StringComparer.OrdinalIgnoreCase);

    public static void Return(Dictionary<string, InputField> map)
    {
        map.Clear();

        if (map.Comparer.Equals(StringComparer.Ordinal))
        {
            Interlocked.CompareExchange(ref _inputFieldMap, map, null);
        }

        if (map.Comparer.Equals(StringComparer.OrdinalIgnoreCase))
        {
            Interlocked.CompareExchange(ref _inputFieldMapOrdinalIgnoreCase, map, null);
        }
    }

    public static Dictionary<string, DirectiveArgument> RentDirectiveArgumentMap()
        => Interlocked.Exchange(ref _directiveArgumentMap, null) ??
            new Dictionary<string, DirectiveArgument>(StringComparer.Ordinal);

    public static Dictionary<string, DirectiveArgument> RentDirectiveArgumentMapOrdinalIgnoreCase()
        => Interlocked.Exchange(ref _directiveArgumentMapOrdinalIgnoreCase, null) ??
            new Dictionary<string, DirectiveArgument>(StringComparer.OrdinalIgnoreCase);

    public static void Return(Dictionary<string, DirectiveArgument> map)
    {
        map.Clear();

        if (map.Comparer.Equals(StringComparer.Ordinal))
        {
            Interlocked.CompareExchange(ref _directiveArgumentMap, map, null);
        }

        if (map.Comparer.Equals(StringComparer.OrdinalIgnoreCase))
        {
            Interlocked.CompareExchange(ref _directiveArgumentMapOrdinalIgnoreCase, map, null);
        }
    }

    public static HashSet<MemberInfo> RentMemberSet()
        => Interlocked.Exchange(ref _memberSet, null) ??
            [];

    public static void Return(HashSet<MemberInfo> set)
    {
        set.Clear();
        Interlocked.CompareExchange(ref _memberSet, set, null);
    }

    public static HashSet<string> RentNameSet()
        => Interlocked.Exchange(ref _nameSet, null) ??
            new HashSet<string>(StringComparer.Ordinal);

    public static HashSet<string> RentNameSetOrdinalIgnoreCase()
        => Interlocked.Exchange(ref _nameSetOrdinalIgnoreCase, null) ??
            new HashSet<string>(StringComparer.OrdinalIgnoreCase);

    public static void Return(HashSet<string> set)
    {
        set.Clear();

        if (set.Comparer.Equals(StringComparer.Ordinal))
        {
            Interlocked.CompareExchange(ref _nameSet, set, null);
        }

        if (set.Comparer.Equals(StringComparer.OrdinalIgnoreCase))
        {
            Interlocked.CompareExchange(ref _nameSetOrdinalIgnoreCase, set, null);
        }
    }

    public static Dictionary<ParameterInfo, string> RentArgumentNameMap()
        => Interlocked.Exchange(ref _argumentNameMap, null) ??
            new Dictionary<ParameterInfo, string>();

    public static void Return(Dictionary<ParameterInfo, string> map)
    {
        map.Clear();
        Interlocked.CompareExchange(ref _argumentNameMap, map, null);
    }

    // We allow the helper to clear all pooled objects so that after
    // building the schema, we can release the memory.
    // There is a risk of extra allocation here if we build
    // multiple schemas at the same time.
    public static void Clear()
    {
        Interlocked.Exchange(ref _objectFieldConfigurationMap, null);
        Interlocked.Exchange(ref _interfaceFieldConfigurationMap, null);
        Interlocked.Exchange(ref _inputFieldConfigurationMap, null);
        Interlocked.Exchange(ref _inputFieldMap, null);
        Interlocked.Exchange(ref _inputFieldMapOrdinalIgnoreCase, null);
        Interlocked.Exchange(ref _directiveArgumentMap, null);
        Interlocked.Exchange(ref _directiveArgumentMapOrdinalIgnoreCase, null);
        Interlocked.Exchange(ref _argumentNameMap, null);
        Interlocked.Exchange(ref _memberSet, null);
        Interlocked.Exchange(ref _nameSet, null);
        Interlocked.Exchange(ref _nameSetOrdinalIgnoreCase, null);
    }
}
