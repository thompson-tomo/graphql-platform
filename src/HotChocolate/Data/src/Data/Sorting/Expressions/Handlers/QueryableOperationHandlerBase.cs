using System.Diagnostics.CodeAnalysis;
using HotChocolate.Configuration;
using HotChocolate.Language;
using HotChocolate.Language.Visitors;
using HotChocolate.Types.Descriptors.Configurations;

namespace HotChocolate.Data.Sorting.Expressions;

public abstract class QueryableOperationHandlerBase
    : SortOperationHandler<QueryableSortContext, QueryableSortOperation>
{
    private readonly int _operation;

    protected QueryableOperationHandlerBase(int operation)
    {
        _operation = operation;
    }

    public override bool CanHandle(
        ITypeCompletionContext context,
        EnumTypeConfiguration typeDefinition,
        SortEnumValueConfiguration valueConfiguration)
    {
        return valueConfiguration.Operation == _operation;
    }

    public override bool TryHandleEnter(
        QueryableSortContext context,
        ISortField field,
        SortEnumValue? sortEnumValue,
        EnumValueNode valueNode,
        [NotNullWhen(true)] out ISyntaxVisitorAction? action)
    {
        if (sortEnumValue is null)
        {
            context.ReportError(
                ErrorHelper.CreateNonNullError(field, valueNode, context));

            action = null!;
            return false;
        }

        if (context.Instance.Peek() is QueryableFieldSelector fieldSelector)
        {
            context.Operations.Enqueue(
                HandleOperation(
                    context,
                    fieldSelector,
                    field,
                    sortEnumValue));

            action = SyntaxVisitor.Continue;
            return true;
        }

        throw new InvalidOperationException(
            DataResources.QueryableSortHandler_InvalidSelector);
    }

    protected abstract QueryableSortOperation HandleOperation(
        QueryableSortContext context,
        QueryableFieldSelector fieldSelector,
        ISortField field,
        SortEnumValue? sortEnumValue);
}
