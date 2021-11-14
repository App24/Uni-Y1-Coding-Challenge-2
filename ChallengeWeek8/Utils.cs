using System;
using System.Collections.Generic;

namespace ChallengeWeek8
{
    internal static class Utils
    {
        public static List<string> ValidYes { get; } = new List<string>() { "y", "yes", "yeah", "yea" };
        public static List<string> ValidNo { get; } = new List<string>() { "n", "no", "nah" };


        /// <summary>
        /// Get the user to select an option from an array, they are also able to type the name of the item to select it
        /// </summary>
        /// <param name="options">The options available to choose from</param>
        /// <returns>Returns a number between 0 (inclusive) and length of <paramref name="options"/> (exclusive)</returns>
        public static int GetSelection(params string[] options)
        {
            int index = -1;
            List<string> optionsList = new List<string>(options);
            for (int i = 0; i < options.Length; i++)
            {
                WriteColor($"{i + 1}. {options[i]}[/]");
            }
            Console.WriteLine();
            while (index < 0 || index >= options.Length)
            {
                string selection = Console.ReadLine().ToLower().Trim();
                if (!int.TryParse(selection, out index))
                {
                    index = optionsList.FindIndex(option => selection == option.ToLower()) + 1;
                }
                index -= 1;
                if (index < 0 || index >= options.Length)
                {
                    WriteColor($"[{ColorConstants.BAD_COLOR}]Please enter a valid choice!");
                }
            }
            return index;
        }

        /// <summary>
        /// Get the user to confirm something
        /// </summary>
        /// <param name="inputText">Text to be displayed to the user for them to confirm</param>
        /// <returns><see langword="true"/> if the user said yes, <see langword="false"/> if they said no</returns>
        public static bool GetConfirmation(string inputText)
        {
            while (true)
            {
                WriteColor(inputText);
                string response = Console.ReadLine().ToLower().Trim();

                if (ValidYes.Contains(response))
                {
                    return true;
                }
                else if (ValidNo.Contains(response))
                {
                    return false;
                }
            }
        }

        /// <summary>
        /// Write colored text
        /// </summary>
        /// <param name="text">Text with colored formatting</param>
        public static void WriteColor(string text)
        {
            StringColorBuilder stringColorBuilder = new StringColorBuilder(text);

            foreach (List<StringColor> stringColors in stringColorBuilder.Split("\n"))
            {
                foreach (StringColor stringColor in stringColors)
                {
                    Console.ForegroundColor = stringColor.ForegroundColor;
                    Console.Write(stringColor.Text);
                }
                Console.WriteLine();
            }
            Console.ResetColor();
        }
    }
}
