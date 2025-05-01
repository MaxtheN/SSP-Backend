using Humanizer;
using System;
using System.Globalization;
using WEBASE.i18n;

namespace SspUis.Core.Extensions
{
    public class CultureIdConst
    {
        public const string uz_cyrl = "uz-cyrl";
        public const string uz_latn = "uz-latn";
        public const string en = "en";
        public const string ru = "ru";
    }
    public static class HumanizerExtensions
    {
        public static string DecimalInWord(this object amount, string cultureInfo)
        {
            var cultureInfoSpecial = cultureInfo.NormalizeCulture();
            string result = amount.ToString();
            if (amount is int)
                return Convert.ToInt32(amount).ToWords(new CultureInfo(cultureInfo));
            if (amount is decimal || amount is double)
            {
                string[] parts = null;
                amount = result;
                if (result.Contains(",") || result.Contains("."))
                {
                    if (result.Contains(","))
                        parts = result.Split(',');
                    if (result.Contains("."))
                        parts = result.Split('.');

                    if (parts != null)
                    {
                        if (Convert.ToInt64(parts[1]) != 0)
                        {
                            if (cultureInfo == CultureIdConst.uz_latn)
                            {
                                if (Convert.ToInt64(parts[1]) != 0)
                                    return $"{Convert.ToInt64(parts[0]).ToWords(new CultureInfo(cultureInfoSpecial))} butun {Convert.ToInt64(Math.Pow(10, parts[1].ToString().Length)).ToWords((new CultureInfo(cultureInfoSpecial)))}dan {Convert.ToInt64(parts[1]).ToWords(new CultureInfo(cultureInfoSpecial))}";
                            }
                            if (cultureInfo == CultureIdConst.uz_cyrl)
                            {
                                if (Convert.ToInt64(parts[1]) != 0)
                                    return $"{Convert.ToInt64(parts[0]).ToWords(new CultureInfo(cultureInfoSpecial))} бутун {Convert.ToInt64(Math.Pow(10, parts[1].ToString().Length)).ToWords((new CultureInfo(cultureInfoSpecial)))}дан {Convert.ToInt64(parts[1]).ToWords(new CultureInfo(cultureInfoSpecial))}";
                            }
                            if (cultureInfo == CultureIdConst.en)
                            {
                                if (Convert.ToInt64(parts[1]) != 0)
                                {
                                    string wordinEng = $"{Convert.ToInt64(parts[0]).ToWords(new CultureInfo(cultureInfoSpecial))} point ";

                                    for (int i = 0; i < parts[1].Length; i++)
                                        wordinEng += Convert.ToInt64(parts[1][i].ToString()).ToWords(new CultureInfo(cultureInfoSpecial)) + " ";
                                    return wordinEng;
                                }
                            }
                        }
                        return $"{Convert.ToInt64(parts[0]).ToWords(new CultureInfo(cultureInfoSpecial))}";
                    }
                }
                return Convert.ToInt64(amount).ToWords(new CultureInfo(cultureInfoSpecial));
            }


            return result;
        }
        public static string NormalizeCulture(this string cultureInfo)
        {
            string normalizedCultureInfo = "uz-Cyrl-UZ";
            switch (cultureInfo.ToLower())
            {
                case "uz-cyrl":
                    normalizedCultureInfo = "uz-Cyrl-UZ";
                    break;
                case "uz-latn":
                    normalizedCultureInfo = "uz-Latn-UZ";
                    break;
                case "en":
                    normalizedCultureInfo = "en-US";
                    break;
            }
            return normalizedCultureInfo;
        }

        public static string MoneyInWord(this object amount, CultureModel cultureModel, string mainMoney, string pocketMoney)
        {
            string cultureInfo = "ru-RU";
            switch (cultureModel.Code.ToLower())
            {
                case "uz-cyrl":
                    cultureInfo = "uz-Cyrl-UZ";
                    break;
                case "uz-latn":
                    cultureInfo = "uz-Latn-UZ";
                    break;
                case "en":
                    cultureInfo = "en-US";
                    break;
            }
            return amount.MoneyInWord(cultureInfo, mainMoney, pocketMoney);
        }

        public static string MoneyInWord(this object amount, string cultureInfo, string mainMoney, string pocketMoney)
        {
            try
            {
                string result = amount.ToString();
                if (amount is int)
                    return $"{Convert.ToInt32(amount).ToWords(new CultureInfo(cultureInfo))} {mainMoney}";
                if (amount is decimal || amount is double)
                {
                    string[] parts = null;
                    amount = result;
                    if (result.Contains(",") || result.Contains("."))
                    {
                        if (result.Contains(","))
                            parts = result.Split(',');
                        if (result.Contains("."))
                            parts = result.Split('.');

                        if (parts != null)
                        {
                            if (Convert.ToInt64(parts[1]) != 0)
                            {
                                if (Convert.ToInt64(parts[1]) != 0)
                                    return $"{Convert.ToInt64(parts[0]).ToWords(new CultureInfo(cultureInfo))} {mainMoney} {Convert.ToInt64(parts[1]).ToWords(new CultureInfo(cultureInfo))} {pocketMoney}";
                            }
                            return $"{Convert.ToInt64(parts[0]).ToWords(new CultureInfo(cultureInfo))} {mainMoney}";
                        }
                    }
                    return $"{Convert.ToInt64(amount).ToWords(new CultureInfo(cultureInfo))} {mainMoney}";
                }


                return result;
            }
            catch (NotImplementedException)
            {
                return "...";
            }
        }
    }
}
