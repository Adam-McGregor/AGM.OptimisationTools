using AGM.OptimisationTools.CombinatorialReserve;

namespace AGM.OptimisationTools.Metrics;

/// <summary>
/// a distance metric to for two reserved objects
/// </summary>
/// <typeparam name="T">The object being stored in memory</typeparam>
/// <param name="x">the first object</param>
/// <param name="y">the second object</param>
/// <returns>the distance between the two objects</returns>
public delegate double Distance<T>(ref T x, ref T y) where T : struct, IReserved;