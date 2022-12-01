using AGM.OptimisationTools.CombinatorialReserve;
using AGM.OptimisationTools.Metrics;

namespace AGM.OptimisationTools.ModelBasedLearning;

/// <summary>
/// A linkage tree or hierarchical cluster
/// </summary>
/// <typeparam name="T">The object being stored in memory</typeparam>
public sealed class LinkageTree<T> where T : struct, IReserved
{
    private readonly ICombinatorialMemoryReserve<T> _reserve;
    public Stack<int> Stack { get; }

    /// <summary>
    /// initialises a linkage tree
    /// </summary>
    /// <param name="reserve">the meomry reserve the objects are inside</param>
    /// <param name="distance">the distance metric to use</param>
    public LinkageTree(ICombinatorialMemoryReserve<T> reserve, Distance<T> distance)
    {
        _reserve = reserve;
        Span<int> clusters = stackalloc int[2 * reserve.N - 1];
        byte ClustersCount = reserve.N;
        Stack = new Stack<int>(clusters.Length);
        for (byte i = 0; i < ClustersCount; i++)
        {
            Stack.Push(i);
            clusters[i] = i;
            ref var reserved = ref reserve[i];
            if (!reserved.Initialised)
            {
                reserved.Ids = new byte[] { i };
                reserved.Initialise();
            }
        }


        Span<double> proximity = stackalloc double[reserve.N * ( reserve.N + 1 ) / 2];
        Dictionary<int, byte> dict = new(reserve.N + 1);

        double min = double.MaxValue;
        int x = 0, y = 0; // min value keys
                          // initialise proximity matrix
        for (byte i = 0; i < reserve.N; i++)
        {
            dict.Add(i, i);
            for (byte j = (byte)( i + 1 ); j < reserve.N; j++)
            {
                int index = GetIndex(i, j, reserve.N);
                proximity[index] = distance(ref reserve[i], ref reserve[j]);
                if (proximity[index] < min)
                {
                    min = proximity[index];
                    x = i;
                    y = j;
                }
            }
        }
        // build clusters while all clusters are not merged or are not able to be mergered
        Span<byte> cluster = stackalloc byte[reserve.N];
        while (dict.Count > 1 && min != double.MaxValue)
        {
            if (distance(ref reserve[x], ref reserve[y]) == double.MaxValue)
            {
                proximity[GetIndex(dict[x], dict[y], reserve.N)] = double.MaxValue;
            }
            else
            {
                // build new cluster
                var cx = reserve[clusters[x]].Ids.Span;
                var cy = reserve[clusters[y]].Ids.Span;
                Span<byte> newCluster = cluster[..( cx.Length + cy.Length )];
                for (int i = 0, j = 0, k = 0; i < cx.Length + cy.Length; i++)
                {
                    newCluster[i] = j < cx.Length && ( k >= cy.Length || cx[j] < cy[k] ) ? cx[j++] : cy[k++];
                }
                int id = reserve.GetId(newCluster);
                Stack.Push(id);
                clusters[ClustersCount++] = id;

                // update proximity matrix
                var c1 = clusters[dict[x]];
                foreach (var i in dict.Keys.Where(p => dict[p] > dict[x]))
                {
                    var c2 = clusters[dict[i]];
                    int index = GetIndex(dict[x], dict[i], reserve.N);
                    proximity[index] = distance(ref reserve[c1], ref reserve[c2]);
                }
                // add cluster c_ij to matrix
                dict.Add(ClustersCount - 1, dict[x]);
                // remove clusters c_i and c_j to matrix
                dict.Remove(x);
                dict.Remove(y);
                // if there is more than one cluster, no more to merge
                if (dict.Count <= 1)
                    break;
            }

            min = double.MaxValue;
            foreach (var i in dict.Keys)
            {
                foreach (var j in dict.Keys.Where(p => dict[p] > dict[i]))
                {
                    int index = GetIndex(dict[i], dict[j], reserve.N);
                    if (proximity[index] < min)
                    {
                        min = proximity[index];
                        x = i;
                        y = j;
                    }
                }
            }
        }
    }


    private static int GetIndex(byte i, byte j, byte length) => i * length + j - ( i + 1 ) * ( i + 2 ) / 2;
}