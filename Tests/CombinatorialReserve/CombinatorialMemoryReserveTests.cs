using AGM.OptimisationTools.CombinatorialReserve;

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
                var reserved = reserve.RankAndReserve(combo);
                ref var item = ref reserved.Get();
                item.Payload = reserved.Id;
            });

        var bucket = reserve.Buckets[k - 1];
        Parallel.For(0, bucket.Length,
            (i) =>
            {
                Assert.Equal(bucket.Span[i].Payload, i);
            });
    }

    public static IEnumerable<object[]> CorrectMappingData()
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
    [MemberData(nameof(CorrectMappingData))]
    public void CorrectMapping1(byte a, byte b, byte c, int index)
    {
        byte n = 5;
        byte k = 3;
        CombinatorialMemoryReserve<TestStruct> reserve = new(n, k);

        byte[] ids = new[] { a, b, c };
        var reserved = reserve.RankAndReserve(ids);
        ref var item = ref reserved.Get();
        item.Payload = 20;

        int expected = reserve.Buckets[k - 1].Span[index].Payload;
        Assert.Equal(expected, reserve[(byte)ids.Length, reserved.Id].Payload);
    }

    [Theory]
    [MemberData(nameof(CorrectMappingData))]
    public void CorrectMapping2(byte a, byte b, byte c, int index)
    {
        byte n = 5;
        byte k = 3;
        CombinatorialMemoryReserve<TestStruct> reserve = new(n, k);

        byte[] ids = new[] { a, b, c };
        var reserved = reserve.RankAndReserve(ids);
        ref var item = ref reserved.Get();
        item.Payload = 20;

        int expected = reserve.Buckets[k - 1].Span[index].Payload;
        Assert.Equal(expected, reserve[reserved.BucketId, reserved.Id].Payload);
    }

    [Theory]
    [MemberData(nameof(CorrectMappingData))]
    public void CorrectMapping3(byte a, byte b, byte c, int index)
    {
        byte n = 5;
        byte k = 3;
        CombinatorialMemoryReserve<TestStruct> reserve = new(n, k);

        byte[] ids = new[] { a, b, c };
        var reserved = reserve.RankAndReserve(ids);
        ref var item = ref reserved.Get();
        item.Payload = 20;

        int expected = reserve.Buckets[k - 1].Span[index].Payload;
        Assert.Equal(expected, reserved.Get().Payload);
    }
}

file struct TestStruct
{
    public int Payload { get; set; }
}