using HotChocolate.Types;

namespace HotChocolate.Data.Sorting;

public interface ISortEnumValue : IEnumValue
{
    ISortOperationHandler Handler { get; }

    int Operation { get; }
}
