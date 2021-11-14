using System.Collections.Generic;

namespace ChallengeWeek8
{
    internal static class Extensions
    {
        public static void AddOrReplace<T>(this IList<T> list, T value)
        {
            if (!list.Contains(value))
            {
                list.Add(value);
            }
            else
            {
                int index = list.IndexOf(value);
                list.Remove(value);
                list.Insert(index, value);
            }
        }
    }
}
