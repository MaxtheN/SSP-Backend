using SspUis.Core;
using SspUis.DataLayer.EfClasses;
using System;
using System.Linq;

namespace SspUis.BizLogicLayer.Extensions
{
    public static class StringExtension
    {
        public static string FormatNumber(this string input, int groupSize)
        {
            if (input.Length <= groupSize)
                return input;

            int remainder = input.Length % groupSize;
            string formatted = input.Substring(0, remainder);

            for (int i = remainder; i < input.Length; i += groupSize)
            {
                if (formatted.Length > 0)
                    formatted += " ";

                formatted += input.Substring(i, groupSize);
            }

            return formatted;
        }

        public static string CorrectPhoneNumber(this string phoneNumber)
        {
            string trimmedNumber = new string(phoneNumber.Where(char.IsDigit).ToArray());

            if (trimmedNumber.Length < Constants.FIXED_PHONE_NUMBER_LENGTH1)
                throw new Exception($"Телефон номер тўғри форматда емас ! {trimmedNumber}  //  LIKE: [997771122] or [998997771122]");

            else if (trimmedNumber.Length > Constants.FIXED_PHONE_NUMBER_LENGTH1)
            {
                if (trimmedNumber.Length > Constants.FIXED_PHONE_NUMBER_LENGTH2)
                    throw new Exception("Телефон номер узунлиги [12] да катта бўла олмайди ! MAX LENGTH: [998907771122]");

                else if (trimmedNumber.Length < Constants.FIXED_PHONE_NUMBER_LENGTH2)
                {
                    trimmedNumber = ReversePhoneNumber(ReversePhoneNumber(trimmedNumber).Substring(0, Constants.FIXED_PHONE_NUMBER_LENGTH1));
                    trimmedNumber = BusinessmanUser.GetCorrectUserName(trimmedNumber);
                }
                else
                {
                    if (!trimmedNumber.StartsWith("998"))
                        throw new Exception($"Телефон номер узунлиги [12] {trimmedNumber} аммо [998] дан бошланмаган !");
                }
            }
            else
                trimmedNumber = BusinessmanUser.GetCorrectUserName(trimmedNumber);

            return trimmedNumber;
        }
        private static string ReversePhoneNumber(string phoneNumber)
        {
            char[] reverse = phoneNumber.ToCharArray();

            Array.Reverse(reverse);

            return new string(reverse);
        }
    }
}
