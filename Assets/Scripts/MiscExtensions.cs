using System.Collections.Generic;

/// <summary>
/// A class containing extensions to the List class.
/// </summary>
static class ListExtensions
{
    /// <summary>
    /// I hate how Unity prints the list adress instead of the contents of the list, like Java. It complicates
    /// debugging. This method prints the contents of a list so you don't need to put a for loop inline as you
    /// trace debug.
    /// </summary>
    /// <typeparam name="T">The type contained by the list.</typeparam>
    /// <param name="list">A string representing the contents of the list.</param>
    /// <returns></returns>
    public static string PrintAll<T>(this List<T> list)
    {
        string toReturn = "";
        foreach (T t in list)
        {
            toReturn += (t + ", ");
        }
        toReturn.TrimEnd(new char[] { ' ', ',' });
        return toReturn;
    }

    /// <summary>
    /// Shuffles the element order of the specified list. Obtained online, not my code.
    /// </summary>
    public static void Shuffle<T>(this IList<T> ts)
    {
        var count = ts.Count;
        var last = count - 1;
        for (var i = 0; i < last; ++i)
        {
            var r = UnityEngine.Random.Range(i, count);
            var tmp = ts[i];
            ts[i] = ts[r];
            ts[r] = tmp;
        }
    }
}