using System.Numerics;

namespace AGM.OptimisationTools.CombinatorialReserve;

/// <summary>
/// A class for reserving contiguous memory for all combinations of natural numbers (including 0), up to some value
/// </summary>
/// <typeparam name="T">The object being stored in memory</typeparam>
public sealed class CombinatorialMemoryReserve<T> : ICombinatorialMemoryReserve<T> where T : struct
{
    private readonly Memory<T> _data;
    private readonly Memory<int> _tiers;
    public byte N { get; init; }
    /// <summary>
    /// references all of the k combinations (zero indexed)
    /// That is,    k = 0 => all single combinations,
    ///             k = 1 => all double combinations,
    ///             etc.
    /// </summary>
    public Memory<T>[] Buckets { get; init; }
    public int Combinations { get; init; }

    public ref T this[int id]
    {
        get { return ref _data.Span[id]; }
    }

    /// <summary>
    /// Resereves contiguous memory for all the combinations and slices the each k combination into buckets
    /// </summary>
    /// <param name="n">The number of natural numbers to use (exclusive, i.e., starts at 0)</param>
    /// <param name="limit">Prevents memory for all combinations sizes over the limit from being stored (i.e. saves memory)</param>
    /// <remarks>if limit is larger than n, then largest combination size is n</remarks>
    public CombinatorialMemoryReserve(byte n, byte limit = byte.MaxValue)
    {
        if (n < limit)
            limit = n;

        int size = 0;
        // store information on how large each bucket has to be
        Span<int> kth = stackalloc int[limit];
        if (limit == n)
        {
            kth[n - 1] = 1;
            size++;
        }
        for (byte i = 0, j = (byte)( n - 2 ); i < ( n - 1 ) / 2 && i < limit; i++, j--)
        {
            kth[i] = Choose(n, (byte)( i + 1 ));
            if (j < limit)
            {
                size += 2 * kth[i];
                kth[j] = kth[i];
            }
            else
            {
                size += kth[i];
            }
        }
        if (n % 2 == 0 && ( n - 1 ) / 2 < limit)
        {
            kth[( n - 1 ) / 2] = Choose(n, (byte)( ( n - 1 ) / 2 + 1 ));
            size += kth[( n - 1 ) / 2];
        }

        // store the fields
        Combinations = size;
        _data = new T[size];
        N = n;
        Buckets = new Memory<T>[limit];
        _tiers = new int[limit];

        // make the buckets
        int u = 0;
        for (byte i = 0; i < limit; i++)
        {
            int v = kth[i];
            Buckets[i] = _data.Slice(u, v);
            _tiers.Span[i] = u;
            u += v;
        }
    }

    public int GetId(scoped Span<byte> ids)
    {
        if (ids.Length > Buckets.Length)
            throw new IndexOutOfRangeException(nameof(ids));

        // rank the combination
        byte k = (byte)ids.Length;
        int index = Choose(N, k);
        for (byte i = 0; i < k; i++)
        {
            index -= Choose((byte)( N - ids[i] - 1 ), (byte)( k - i ));
        }
        index--;
        return _tiers.Span[ids.Length - 1] + index;
    }

    /// <summary>
    /// code for n choose k
    /// </summary>
    /// <param name="n">number of items</param>
    /// <param name="k">number of items to choose</param>
    /// <returns>n choose k</returns>
    /// <see cref="https://visualstudiomagazine.com/articles/2022/07/20/math-combinations-using-csharp.aspx"/>
    /// <remarks>code modified from link</remarks>
    private static int Choose(byte n, byte k)
    {
        int delta = n - k;
        BigInteger ans = delta + 1;
        for (int i = 2; i <= k; ++i)
            ans = ans * ( delta + i ) / i;

        return (int)ans;
    }

}