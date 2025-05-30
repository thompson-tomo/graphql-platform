using HotChocolate.Language;
using HotChocolate.Types.Descriptors.Configurations;

namespace HotChocolate.Types.Descriptors;

public class EnumTypeDescriptor<T>
    : EnumTypeDescriptor
    , IEnumTypeDescriptor<T>
{
    protected internal EnumTypeDescriptor(IDescriptorContext context)
        : base(context, typeof(T))
    {
    }

    protected internal EnumTypeDescriptor(
        IDescriptorContext context,
        EnumTypeConfiguration definition)
        : base(context, definition)
    {
    }

    public new IEnumTypeDescriptor<T> Name(string value)
    {
        base.Name(value);
        return this;
    }

    public new IEnumTypeDescriptor<T> Description(string value)
    {
        base.Description(value);
        return this;
    }

    public new IEnumTypeDescriptor<T> BindValues(BindingBehavior behavior)
    {
        base.BindValues(behavior);
        return this;
    }

    public new IEnumTypeDescriptor<T> BindValuesExplicitly() =>
        BindValues(BindingBehavior.Explicit);

    public new IEnumTypeDescriptor<T> BindValuesImplicitly() =>
        BindValues(BindingBehavior.Implicit);

    public IEnumValueDescriptor Value(T value)
    {
        return base.Value(value);
    }

    public new IEnumTypeDescriptor<T> Directive<TDirective>(
        TDirective directiveInstance)
        where TDirective : class
    {
        base.Directive(directiveInstance);
        return this;
    }

    public new IEnumTypeDescriptor<T> Directive<TDirective>()
        where TDirective : class, new()
    {
        base.Directive<TDirective>();
        return this;
    }

    public new IEnumTypeDescriptor<T> Directive(
        string name,
        params ArgumentNode[] arguments)
    {
        base.Directive(name, arguments);
        return this;
    }
}
