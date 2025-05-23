using System.Linq.Expressions;
using HotChocolate.Configuration;
using HotChocolate.Data.Filters.Expressions;

namespace HotChocolate.Data.Filters;

public class MatchAnyQueryableFieldHandler : FilterFieldHandler<QueryableFilterContext, Expression>
{
    public override bool CanHandle(
        ITypeCompletionContext context,
        IFilterInputTypeConfiguration typeConfiguration,
        IFilterFieldConfiguration fieldConfiguration) => true;
}
