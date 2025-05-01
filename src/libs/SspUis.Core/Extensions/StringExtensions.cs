using System;
using System.Linq;
using System.Text.RegularExpressions;
using Newtonsoft.Json;
using WEBASE.Utility;

namespace SspUis.Core
{
    public static class StringExtensions
    {
        //thanks to https://stackoverflow.com/questions/3565015/bestpractice-transform-first-character-of-a-string-into-lower-case
        public static string ToLowerFirstChar(this string input)
        {
            if (string.IsNullOrEmpty(input))
                throw new ArgumentNullException(nameof(input));
            return input.First().ToString().ToLower() + input.Substring(1);
        }

        public static string CapitalizeEachWord(this string input, string separator)
        {
            if (string.IsNullOrEmpty(input))
                throw new ArgumentNullException(nameof(input));
            var result = input.Split(separator);
            for (int i = 0; i < result.Length; i++)
            {
                result[i] = result[i].CapitalizeFirstChar();
            }
            return string.Join(separator, result);
        }
        public static string SafeSubstring(this string input, int length)
        {
            if (length < 0)
                throw new ArgumentOutOfRangeException(nameof(length), "Substring length cannot be less than 0");

            if (string.IsNullOrEmpty(input) || input.Length <= length)
                return input;

            return input.Substring(0, length);
        }

        public static string NormalizeOracleError(this string errorMessage)
        {
            try
            {
                errorMessage = errorMessage.Replace("\n", "");

                string pattern = @"ORA-\d+: (.*?)\b(?=ORA-\d+:|$)";

                // Отображаем только текст ошибки RAISE_APPLICATION_ERROR
                MatchCollection matches = Regex.Matches(errorMessage, pattern);

                // Отображаем полный текст ошибки Oracle
                return matches[0].Groups[1].Value;
            }
            catch
            {
            }

            return errorMessage;
        }
        public class TrimmingStringConverter : JsonConverter

        {

            public override bool CanRead => true;

            public override bool CanWrite => false;

            public override bool CanConvert(Type objectType) => objectType == typeof(string);

            public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)

            {

                if (reader.Value is string value)

                {

                    return value.Trim();

                }

                return reader.Value;

            }

            public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)

            {

            }

        }
    }
}
