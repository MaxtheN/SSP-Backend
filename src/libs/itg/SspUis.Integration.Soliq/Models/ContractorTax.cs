using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SspUis.Integration.Soliq.Models
{
    public class ContractorTax
    {
        public DateTime regDate { get; set; }
        public int? na1Code { get; set; }
        public string na1Name { get; set; }
        public int? ns10Code { get; set; }
        public int? ns11Code { get; set; }
        public string shortName { get; set; }
        public string name { get; set; }
        public string tin { get; set; }
        public int? statusCode { get; set; }
        public string statusName { get; set; }
        public string mfo { get; set; }
        public string account { get; set; }
        public string address { get; set; }
        public string oked { get; set; }
        public string directorTin { get; set; }
        public string director { get; set; }
        public string accountant { get; set; }
        public int? isBudget { get; set; }
        public bool isItd { get; set; }
        public string personalNum { get; set; }
        public string soato { get; set; }
        public class BudgetInfo
        {
            public bool success { get; set; }
            public string reason { get; set; }
            public DataInfo data { get; set; }
            public class DataInfo
            {
                public string tin { get; set; }
                public string account { get; set; }
                public int? isBudget { get; set; }

            }
        }
        public class VatCodeStatus
        {
            public bool success { get; set; }
            public string reason { get; set; }
            public DataInfo data { get; set; }
            public class DataInfo
            {
                public string vatRegCode { get; set; }
                public bool active { get; set; }

                public bool IsEmptyVatCode()
                {
                    return string.IsNullOrEmpty(vatRegCode);
                }

                public string GetMessage() => GetMessage("ru");

                public string GetMessage(string lang)
                {
                    string orgstatus = "";

                    if (active)
                        orgstatus = lang == "ru" ? "Плательщик НДС" : "QQS to'lovchisi";
                    else if (!active && string.IsNullOrEmpty(vatRegCode))
                        orgstatus = lang == "ru" ? "Неплательщик НДС" : "QQS to'lovchisi emas";
                    else if (!active && !string.IsNullOrEmpty(vatRegCode))
                        orgstatus = lang == "ru" ? "Сертификат плательщика НДС неактивен" : "QQS guvohnomasifaol emas";

                    return orgstatus;
                }
            }

        }
    }
}
