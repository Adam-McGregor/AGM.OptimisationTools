namespace AGM.OptimisationTools.CombinatorialReserve;

/// <summary>
/// Manages a reference to an IReservable item on the CombinatorialMemoryReserve{T}
/// </summary>
/// <typeparam name="T">The object being stored in memory</typeparam>
public interface IReserved
{
    /// <summary>
    /// The object id i.e. the index for the memory reserve
    /// </summary>
    int Id { get; init; }

    ///// <summary>
    ///// Gets the reference to the reserved item
    ///// </summary>
    ///// <returns>the reference to the reserved item</returns>
    //ref T Get();
}