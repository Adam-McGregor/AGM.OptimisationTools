namespace AGM.OptimisationTools.CombinatorialReserve;

/// <summary>
/// Manages a reference to an IReservable item on the CombinatorialMemoryReserve{T}
/// </summary>
/// <typeparam name="T">The object being stored in memory</typeparam>
public sealed class Reserved<T> : IReserved<T> where T : struct
{
    private readonly CombinatorialMemoryReserve<T> _reserve;
    public required int Id { get; init; }
    public required byte BucketId { get; init; }


    public Reserved(CombinatorialMemoryReserve<T> reserve)
    {
        _reserve = reserve;
    }

    public ref T Get() => ref _reserve[BucketId, Id];
}
