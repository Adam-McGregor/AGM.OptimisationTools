namespace AGM.OptimisationTools.CombinatorialReserve;

/// <summary>
/// Manages a reference to an IReservable item on the CombinatorialMemoryReserve{T}
/// </summary>
/// <typeparam name="T">The object being stored in memory</typeparam>
public interface IReserved<T> where T : struct
{
    /// <summary>
    /// The number of items in a combination
    /// </summary>
    byte BucketId { get; init; }
    /// <summary>
    /// The object index inside the bucket
    /// </summary>
    int Id { get; init; }

    /// <summary>
    /// Gets the reference to the reserved item
    /// </summary>
    /// <returns>the reference to the reserved item</returns>
    ref T Get();
}