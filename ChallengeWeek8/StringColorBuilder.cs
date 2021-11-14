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
        public string Text { get { return strText; } set { strText = value; BuildString(); } }

        private string strText;

        public List<StringColor> StringColors { get; private set; }

        public const string RE_COLOR_SPLITTER_PATTERN = @"(\[[^\[][^\]]*\])";
        public const string RE_COLOR_START_PATTERN = @"\[([^\[\/][^\]]*)\]";
        public const string RE_COLOR_END_PATTERN = @"(\[[\/][^\]]*\])";

        public StringColorBuilder(string str)
        {
            Text = str;
        }

        private void BuildString()
        {
            string[] pieces = Regex.Split(Text, RE_COLOR_SPLITTER_PATTERN);
            StringColors = new List<StringColor>();

            List<ConsoleColor> previousColors = new List<ConsoleColor>();

            ConsoleColor currentColor = ConsoleColor.White;
            string currentText = "";
            foreach (string piece in pieces)
            {
                if (Regex.IsMatch(piece, RE_COLOR_END_PATTERN))
                {
                    StringColors.Add(new StringColor(currentText, currentColor));
                    currentText = "";
                    if (previousColors.Count > 0)
                    {
                        currentColor = previousColors[previousColors.Count - 1];
                        previousColors.RemoveAt(previousColors.Count - 1);
                    }
                    else
                        currentColor = ConsoleColor.White;
                }
                else if (Regex.IsMatch(piece, RE_COLOR_START_PATTERN))
                {
                    // Removing [ and ] from the string piece
                    string colorString = Regex.Split(piece, RE_COLOR_START_PATTERN)[1];

                    // Try convert it to a ConsoleColor
                    if (Enum.TryParse(colorString, true, out ConsoleColor color))
                    {
                        previousColors.Add(currentColor);
                        StringColors.Add(new StringColor(currentText, currentColor));
                        currentText = "";
                        currentColor = color;
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

            if (!string.IsNullOrEmpty(currentText))
                StringColors.Add(new StringColor(currentText, currentColor));
        }

        public List<List<StringColor>> Split(string separtor)
        {
            List<List<StringColor>> toReturn = new List<List<StringColor>>();
            List<StringColor> currentLine = new List<StringColor>();

            foreach (StringColor stringColor in StringColors)
            {
                string[] texts = stringColor.Text.Split(separtor);
                for (int i = 0; i < texts.Length; i++)
                {
                    if (i > 0)
                    {
                        toReturn.Add(currentLine);
                        currentLine = new List<StringColor>();
                    }
                    currentLine.Add(new StringColor(texts[i], stringColor.ForegroundColor));
                }
            }
            toReturn.Add(currentLine);

            return toReturn;
        }
    }

    internal struct StringColor
    {
        public string Text { get; }
        public ConsoleColor ForegroundColor { get; }

        public StringColor(string text, ConsoleColor foregroundColor)
        {
            Text = text;
            ForegroundColor = foregroundColor;
        }
    }
}
