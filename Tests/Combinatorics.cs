// Accord Math Library
// The Accord.NET Framework
// http://accord-framework.net
//
// Copyright © César Souza, 2009-2017
// cesarsouza at gmail.com
//
//    This library is free software; you can redistribute it and/or
//    modify it under the terms of the GNU Lesser General Public
//    License as published by the Free Software Foundation; either
//    version 2.1 of the License, or (at your option) any later version.
//
//    This library is distributed in the hope that it will be useful,
//    but WITHOUT ANY WARRANTY; without even the implied warranty of
//    MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the GNU
//    Lesser General Public License for more details.
//
//    You should have received a copy of the GNU Lesser General Public
//    License along with this library; if not, write to the Free Software
//    Foundation, Inc., 51 Franklin St, Fifth Floor, Boston, MA  02110-1301  USA
//

namespace Tests;

/// <summary>
///   Static class for combinatorics functions.
/// </summary>
/// 
public static class Combinatorics
{
    /// <summary>
    ///   Enumerates all possible value combinations for a given array.
    /// </summary>
    /// 
    /// <param name="values">The array whose combinations need to be generated.</param>
    /// <param name="k">The length of the combinations to be generated.</param>
    /// <param name="inPlace">
    ///   If set to true, the different generated combinations will be stored in 
    ///   the same array, thus preserving memory. However, this may prevent the
    ///   samples from being stored in other locations without having to clone
    ///   them. If set to false, a new memory block will be allocated for each
    ///   new object in the sequence.</param>
    ///   
    /// <example>
    /// <code source="Unit Tests\Accord.Tests.Math\CombinatoricsTest.cs" region="doc_combinations_k" />
    /// </example>
    /// 
    public static IEnumerable<T[]> Combinations<T>(this T[] values, int k, bool inPlace = false)
    {
        // Based on the Knuth algorithm implementation by
        // http://seekwell.wordpress.com/2007/11/17/knuth-generating-all-combinations/

        int n = values.Length;

        int t = k;

        int[] c = new int[t + 3];
        T[] current = new T[t];
        int j, x;

        for (j = 1; j <= t; j++)
            c[j] = j - 1;
        c[t + 1] = n;
        c[t + 2] = 0;

        j = t;

        do
        {
            for (int i = 0; i < current.Length; i++)
                current[i] = values[c[i + 1]];

            yield return inPlace ? current : (T[])current.Clone();

            if (j > 0)
            {
                x = j;
            }
            else
            {
                if (c[1] + 1 < c[2])
                {
                    c[1]++;
                    continue;
                }
                else
                {
                    j = 2;
                }
            }

            while (true)
            {
                c[j - 1] = j - 2;
                x = c[j] + 1;
                if (x == c[j + 1])
                    j++;
                else
                    break;
            }

            c[j] = x;
            j--;
        } while (j < t);
    }
}