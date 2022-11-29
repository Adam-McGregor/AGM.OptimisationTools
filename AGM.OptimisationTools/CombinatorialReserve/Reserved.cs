namespace AGM.OptimisationTools.CombinatorialReserve;

/// <summary>
/// Manages a reference to an IReservable item on the CombinatorialMemoryReserve{T}
/// </summary>
/// <typeparam name="T">The object being stored in memory</typeparam>
public sealed class Reserved : IReserved
{
    public required int Id { get; init; }
}
