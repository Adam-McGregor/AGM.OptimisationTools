namespace AGM.OptimisationTools.CombinatorialReserve;

/// <summary>
/// An interface for managing contiguous memory for all combinations of natural numbers (including 0), up to some value
/// </summary>
/// <typeparam name="T">The object being stored in memory</typeparam>
public interface ICombinatorialMemoryReserve<T> where T : struct
{
    /// <summary>
    /// gets a reference to the object
    /// </summary>
    /// <param name="bucketId">the id of the bucket, i.e. the number of items in a combination</param>
    /// <param name="id">the object index inside the bucket</param>
    /// <returns>a reference to the object</returns>
    ref T this[byte bucketId, int id] { get; }

    /// <summary>
    /// The number of combinations stored in all the buckets
    /// </summary>
    int Combinations { get; init; }

    /// <summary>
    /// Gets the index of the object corresponding to the ids relative to the its bucket
    /// Sets the IReservable properties for the type T that belongs to the index
    /// </summary>
    /// <param name="ids">the unique natural numbers in acending order</param>
    /// <returns>the index of the object corresponding to the ids relative to the its bucket</returns>
    /// <exception cref="IndexOutOfRangeException">the number of ids should correspond to a bucket</exception>
    /// <remarks>ranks combinations in lexicographic order</remarks>
    IReserved<T> RankAndReserve(Span<byte> ids);
}