using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace ChallengeWeek8
{
    /// <summary>
    /// Separate an input string into a <see cref="List{T}"/> of <see cref="StringColor"/> which can then be used to
    /// print out text in different colors
    /// </summary>
    internal class StringColorBuilder
    {
        /// <summary>
        /// The original text before being processed
        /// </summary>
        public string Text { get { return text; } set { text = value; BuildString(); } }

        private string text;

        /// <summary>
        /// The processed <see cref="List{T}"/> of <see cref="StringColor"/>
        /// </summary>
        public List<StringColor> StringColors { get; private set; }

        public const string RE_COLOR_SPLITTER_PATTERN = @"(\[[^\[][^\]]*\])";
        public const string RE_COLOR_START_PATTERN = @"\[([^\[\/][^\]]*)\]";
        public const string RE_COLOR_END_PATTERN = @"(\[[\/][^\]]*\])";

        /// <summary>
        /// 
        /// </summary>
        /// <param name="text">The text to be processed</param>
        public StringColorBuilder(string text)
        {
            Text = text;
        }

        /// <summary>
        /// Process <see cref="Text"/> into <see cref="StringColors"/>
        /// </summary>
        private void BuildString()
        {
            string[] pieces = Regex.Split(Text, RE_COLOR_SPLITTER_PATTERN);
            StringColors = new List<StringColor>();

            List<ConsoleColor> previousForegroundColors = new List<ConsoleColor>();
            List<ConsoleColor> previousBackgroundColors = new List<ConsoleColor>();

            List<ColorType> colorTypes = new List<ColorType>();

            ConsoleColor currentForegroundColor = ConsoleColor.White;
            ConsoleColor currentBackgroundColor = ConsoleColor.Black;
            string currentText = "";
            foreach (string piece in pieces)
            {
                if (Regex.IsMatch(piece, RE_COLOR_END_PATTERN))
                {
                    StringColors.Add(new StringColor(currentText, currentForegroundColor, currentBackgroundColor));
                    currentText = "";

                    // Allow for going back to the previous color, instead of resetting to the default color
                    if (colorTypes.Count > 0)
                    {
                        ColorType colorType = colorTypes[colorTypes.Count - 1];
                        colorTypes.RemoveAt(colorTypes.Count - 1);
                        switch (colorType)
                        {
                            case ColorType.Foreground:
                                {
                                    if (previousForegroundColors.Count > 0)
                                    {
                                        currentForegroundColor = previousForegroundColors[previousForegroundColors.Count - 1];
                                        previousForegroundColors.RemoveAt(previousForegroundColors.Count - 1);
                                    }
                                    else
                                    {
                                        currentForegroundColor = ConsoleColor.White;
                                    }
                                }
                                break;
                            case ColorType.Background:
                                {
                                    if (previousBackgroundColors.Count > 0)
                                    {
                                        currentBackgroundColor = previousBackgroundColors[previousBackgroundColors.Count - 1];
                                        previousBackgroundColors.RemoveAt(previousBackgroundColors.Count - 1);
                                    }
                                    else
                                    {
                                        currentBackgroundColor = ConsoleColor.Black;
                                    }
                                }
                                break;
                        }
                    }
                }
                else if (Regex.IsMatch(piece, RE_COLOR_START_PATTERN))
                {
                    // Removing [ and ] from the string piece
                    string colorString = Regex.Split(piece, RE_COLOR_START_PATTERN)[1];

                    string[] parts = colorString.Split("=");

                    ColorType colorType = ColorType.Foreground;

                    if (parts.Length > 1)
                    {
                        colorString = parts[1];
                        switch (parts[0].ToLower().Trim())
                        {
                            case "b":
                                {
                                    colorType = ColorType.Background;
                                }
                                break;
                        }
                    }

                    // Try convert it to a ConsoleColor
                    if (Enum.TryParse(colorString.Trim(), true, out ConsoleColor color))
                    {
                        switch (colorType)
                        {
                            case ColorType.Foreground:
                                {
                                    previousForegroundColors.Add(currentForegroundColor);
                                }
                                break;
                            case ColorType.Background:
                                {
                                    previousBackgroundColors.Add(currentBackgroundColor);
                                }
                                break;
                        }
                        StringColors.Add(new StringColor(currentText, currentForegroundColor, currentBackgroundColor));
                        currentText = "";
                        switch (colorType)
                        {
                            case ColorType.Foreground:
                                {
                                    currentForegroundColor = color;
                                }
                                break;
                            case ColorType.Background:
                                {
                                    currentBackgroundColor = color;
                                }
                                break;
                        }
                        colorTypes.Add(colorType);
                    }
                    else // If it fails, append the text to the currentText
                    {
                        currentText += piece;
                    }
                }
                else
                {
                    currentText += piece;
                }
            }


            // Adds the end of the string and whatever the current foreground and background color, allows to end color without putting [/]
            if (!string.IsNullOrEmpty(currentText))
                StringColors.Add(new StringColor(currentText, currentForegroundColor, currentBackgroundColor));
        }

        /// <summary>
        /// Split by <paramref name="separator"/>
        /// </summary>
        /// <param name="separator">A string that delimits the substrings in this string.</param>
        /// <returns>A list containing a list of each <see cref="StringColor"/> that are delimeted by <paramref name="separator"/></returns>
        public List<List<StringColor>> Split(string separator)
        {
            List<List<StringColor>> toReturn = new List<List<StringColor>>();
            List<StringColor> currentLine = new List<StringColor>();

            foreach (StringColor stringColor in StringColors)
            {
                string[] texts = stringColor.Text.Split(separator);
                for (int i = 0; i < texts.Length; i++)
                {
                    // if the current line is higher than the first line, add the previous line to the list
                    if (i > 0)
                    {
                        toReturn.Add(currentLine);
                        currentLine = new List<StringColor>();
                    }
                    currentLine.Add(new StringColor(texts[i], stringColor.ForegroundColor, stringColor.BackgroundColor));
                }
            }
            toReturn.Add(currentLine);

            return toReturn;
        }
    }

    /// <summary>
    /// Data struct to contain information to allow different colored text
    /// </summary>
    internal struct StringColor
    {
        public string Text { get; }
        public ConsoleColor ForegroundColor { get; }
        public ConsoleColor BackgroundColor { get; }

        public StringColor(string text, ConsoleColor foregroundColor, ConsoleColor backgroundColor)
        {
            Text = text;
            ForegroundColor = foregroundColor;
            BackgroundColor = backgroundColor;
        }
    }

    internal enum ColorType
    {
        Foreground,
        Background
    }
}
