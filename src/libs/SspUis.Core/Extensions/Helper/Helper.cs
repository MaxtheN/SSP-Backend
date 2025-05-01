using System;
using System.Linq;

namespace SspUis.Core.Extensions
{
    public static class Helper
    {
        public const int DEFAULT_LENGTH = 100;
        private const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789!@#$%^&*()-_+=<>?/[]{|}~;:',.";
        private static Random rnd = new Random();
        public static string RndUniqueKey(int length) =>
            new string(Enumerable
                .Repeat(chars, length)
                .Select(s => s[rnd.Next(s.Length)])
                .ToArray());
    }
}
