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
    /// <param name="id">the id of the object</param>
    /// <returns>a reference to the object</returns>
    ref T this[int id] { get; }

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
    int GetId(Span<byte> ids);
}