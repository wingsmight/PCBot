using System;

namespace NetCore.Docker
{
    public static class StringExt
    {
        public static string ExtractFromBetweenChars(this string input, string startString, string finishString)
        {
            int innerTextStart = input.IndexOf(startString) + startString.Length;
            int innerTextLength = input.Substring(innerTextStart).IndexOf(finishString);
            return input.Substring(innerTextStart, innerTextLength);
        }
    }
}