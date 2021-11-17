using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace ChallengeWeek8
{
    internal static class Extensions
    {
        /// <summary>
        /// Add or replace an item in a list
        /// </summary>
        /// <typeparam name="T">The type of the list</typeparam>
        /// <param name="list">The list to be affected</param>
        /// <param name="value">The item to add or replace</param>
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

        /// <summary>
        /// Trims a string to remove color data from it
        /// </summary>
        /// <param name="str">The string to trim</param>
        /// <returns>String without color data</returns>
        public static string TrimColor(this string str)
        {
            str = Regex.Replace(str, StringColorBuilder.RE_COLOR_START_PATTERN, "");
            str = Regex.Replace(str, StringColorBuilder.RE_COLOR_END_PATTERN, "");
            return str;
        }
    }
}
