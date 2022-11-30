using AGM.OptimisationTools.CombinatorialReserve;

using System.Numerics;

namespace Tests.CombinatorialReserve;

public class CombinatorialMemoryReserveTests
{
    [Theory]
    [InlineData(5, 1)]
    [InlineData(5, 2)]
    [InlineData(5, 3)]
    [InlineData(5, 4)]
    [InlineData(5, 5)]
    [InlineData(20, 1)]
    [InlineData(20, 2)]
    [InlineData(20, 3)]
    [InlineData(20, 4)]
    [InlineData(20, 5)]
    [InlineData(20, 6)]
    [InlineData(20, 7)]
    [InlineData(20, 8)]
    [InlineData(20, 9)]
    [InlineData(20, 10)]
    [InlineData(20, 11)]
    [InlineData(20, 12)]
    [InlineData(20, 13)]
    [InlineData(20, 14)]
    [InlineData(20, 15)]
    [InlineData(20, 16)]
    [InlineData(20, 17)]
    [InlineData(20, 18)]
    [InlineData(20, 19)]
    [InlineData(20, 20)]
    public void ComprehensiveBucketTest(byte n, int k)
    {
        CombinatorialMemoryReserve<TestStruct> reserve = new(n);

        byte[] nums = new byte[n];
        for (byte i = 0; i < n; i++)
        {
            nums[i] = i;
        }

        Parallel.ForEach(nums.Combinations(k),
            (combo, _) =>
            {
                var id = reserve.GetId(combo);

                byte k = (byte)combo.Length;
                int rank = Choose(n, k);
                for (byte i = 0; i < k; i++)
                {
                    rank -= Choose((byte)( n - combo[i] - 1 ), (byte)( k - i ));
                }
                rank--;
                ref var item = ref reserve[id];
                item.Payload = rank;
            });

        var bucket = reserve.Buckets[k - 1];
        Parallel.For(0, bucket.Length,
            (i) =>
            {
                Assert.Equal(bucket.Span[i].Payload, i);
            });
    }

    public static IEnumerable<object[]> CorrectRankingData()
    {
        yield return new object[] { 0, 1, 2, 0 };
        yield return new object[] { 0, 1, 2, 0 };
        yield return new object[] { 0, 1, 3, 1 };
        yield return new object[] { 0, 1, 4, 2 };
        yield return new object[] { 0, 2, 3, 3 };
        yield return new object[] { 0, 2, 4, 4 };
        yield return new object[] { 0, 3, 4, 5 };
        yield return new object[] { 1, 2, 3, 6 };
        yield return new object[] { 1, 2, 4, 7 };
        yield return new object[] { 1, 3, 4, 8 };
        yield return new object[] { 2, 3, 4, 9 };
    }

    [Theory]
    [MemberData(nameof(CorrectRankingData))]
    public void CorrectRanking(byte a, byte b, byte c, int index)
    {
        byte n = 5;
        byte k = 3;
        CombinatorialMemoryReserve<TestStruct> reserve = new(n, k);

        byte[] ids = new[] { a, b, c };
        var id = reserve.GetId(ids);
        ref var item = ref reserve[id];
        item.Payload = 20;

        int expected = reserve.Buckets[k - 1].Span[index].Payload;
        Assert.Equal(expected, reserve[id].Payload);
    }

    [Theory]
    [InlineData(0, 1, 2)]
    [InlineData(0, 3, 4)]
    [InlineData(2, 3, 4)]
    [InlineData(1, 2, 4)]
    public void CorrectIds(byte a, byte b, byte c)
    {
        byte n = 5;
        byte k = 3;

        CombinatorialMemoryReserve<TestStruct> reserve = new(n, k);

        byte[] ids = new[] { a, b, c };
        var id = reserve.GetId(ids);
        ref var item = ref reserve[id];
        Memory<byte> actual = item.Ids;

        Memory<byte> expected = ids;

        for (int i = 0; i < k; i++)
        {
            Assert.Equal(expected.Span[i], actual.Span[i]);
        }
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

file struct TestStruct : IReserved
{
    public int Payload { get; set; }
    public Memory<byte> Ids { get; set; }

    public void Init()
    {
    }
}