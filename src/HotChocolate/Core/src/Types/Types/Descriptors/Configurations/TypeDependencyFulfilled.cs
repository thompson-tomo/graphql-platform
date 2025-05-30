namespace HotChocolate.Types.Descriptors.Configurations;

/// <summary>
/// Defines when the type dependency has to be fulfilled.
/// </summary>
public enum TypeDependencyFulfilled
{
    /// <summary>
    /// The dependency instance does not need to be completed.
    /// </summary>
    Default,

    /// <summary>
    /// The dependency instance needs to have it`s name completed.
    /// </summary>
    Named,

    /// <summary>
    /// The dependency instance needs to be fully completed.
    /// </summary>
    Completed
}
