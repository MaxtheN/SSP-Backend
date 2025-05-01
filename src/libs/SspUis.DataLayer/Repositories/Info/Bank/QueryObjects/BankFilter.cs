using SspUis.DataLayer.EfClasses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WEBASE;
using WEBASE.Utility;

namespace SspUis.DataLayer
{
    public static class BankFilter
    {
        public static IQueryable<Bank> ByBankCode(this IQueryable<Bank> source, string bankCode, int? bankCodeId, bool isIncludePassive = false)
        {
            if (bankCode.NullOrEmpty())
                throw new ArgumentException($"{nameof(bankCode)} cannot be null or empty string", nameof(bankCode));

            if (!isIncludePassive)
                source = source.IsActive();

            if (bankCodeId.Value == 0 || !bankCodeId.HasValue)
            {
                return source.Where(a => a.Code.ToLower() == bankCode.ToLower());
            }
            else
            {
               return source.Where(a => a.Code.ToLower() == bankCode.ToLower() && a.BankCodeId != bankCodeId);
            }
        }

        public static IQueryable<Bank> ByBankName(this IQueryable<Bank> source, string bankName, int? bankCodeId, bool isIncludePassive = false)
        {
            if (bankName.NullOrEmpty())
                throw new ArgumentException($"{nameof(bankName)} cannot be null or empty string", nameof(bankName));

            if (!isIncludePassive)
                source = source.IsActive();
            if (bankCodeId.Value == 0 || !bankCodeId.HasValue)
            {
                return source.Where(a => a.BankName.ToLower() == bankName.ToLower());
            }
            else
            {
                return source.Where(a => a.BankName.ToLower() == bankName.ToLower() && a.BankCodeId != bankCodeId);
            }
        }
    }
}
