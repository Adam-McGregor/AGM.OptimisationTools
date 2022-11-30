using AGM.OptimisationTools.CombinatorialReserve;
using AGM.OptimisationTools.ModelBasedLearning;

namespace Tests.ModelBasedLearning;

public class LinkageTreeTests
{
    [Fact]
    public void IdsCorrectlySorted()
    {
        byte n = 5;
        CombinatorialMemoryReserve<TestStruct> reserve = new(n);
        LinkageTree<TestStruct> linkageTree = new(reserve, TestStruct.Distance);

        var stack = linkageTree.Stack;
        foreach (var item in stack)
        {
            var ids = reserve[item].Ids.Span;
            for (int i = 0; i < ids.Length - 1; i++)
            {
                Assert.True(ids[i] < ids[i + 1]);
            }
        }
    }


    [Fact]
    // https://medium.com/@rohanjoseph_91119/learn-with-an-example-hierarchical-clustering-873b5b50890c
    public void CorrectDendogram()
    {
        const byte n = 5;
        CombinatorialMemoryReserve<TestStruct> reserve = new(n);

        int[] ReserveIds = new int[2 * n - 1];
        byte[][] combos = new byte[2 * n - 1][]
        {
            new byte[]{ 0, 1, 2, 3, 4 },
            new byte[]{ 2, 3, 4 },
            new byte[]{ 3, 4 },
            new byte[]{ 0, 1 },
            new byte[]{ 4 },
            new byte[]{ 3 },
            new byte[]{ 2 },
            new byte[]{ 1 },
            new byte[]{ 0 }
        };

        LinkageTree<TestStruct> linkageTree = new(reserve, TestStruct.Distance);

        for (int i = 0; i < ReserveIds.Length; i++)
        {
            ReserveIds[i] = reserve.GetId(combos[i]);
        }

        var stack = linkageTree.Stack;
        for (int i = 0; i < ReserveIds.Length; i++)
        {
            Assert.Equal(ReserveIds[i], stack.Pop());
        }
    }
}

file static class Data
{
    public static int[] Points = new[] { 7, 10, 20, 28, 35 };
}

struct TestStruct : IReserved
{
    public int Point { get; set; }
    public Memory<byte> Ids { get; set; }

    public void Initialise()
    {
        int sum = 0;
        Span<byte> ids = Ids.Span;
        for (int i = 0; i < ids.Length; i++)
        {
            sum += Data.Points[ids[i]];
        }
        Point = sum / ids.Length;
    }

    public static double Distance(ref TestStruct x, ref TestStruct y) => y.Point - x.Point;
}