using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SspUis.Integration.DigitizationCenter
{
    public class GSPNewApiResponseDto
    {
        public string result { get; set; }
        public List<GSPNewApiData> data { get; set; }
        public string comments { get; set; }
    }
    public class GSPNewApiData
    {
        public int transaction_id { get; set; }
        public string current_pinpp { get; set; }
        public List<string> pinpps { get; set; }
        public string current_document { get; set; }
        public List<Document> documents { get; set; }
        public string surnamelat { get; set; }
        public string namelat { get; set; }
        public string patronymlat { get; set; }
        public string surnamecyr { get; set; }
        public string namecyr { get; set; }
        public string patronymcyr { get; set; }
        public string engsurname { get; set; }
        public string engname { get; set; }
        public string birth_date { get; set; }
        public string birthplace { get; set; }
        public string birthcountry { get; set; }
        public int birthcountryid { get; set; }
        public int livestatus { get; set; }
        public string nationality { get; set; }
        public int nationalityid { get; set; }
        public string citizenship { get; set; }
        public int citizenshipid { get; set; }
        public int sex { get; set; }
        public string photo { get; set; }
    }

    public class Document
    {
        public string document { get; set; }
        public string type { get; set; }
        public string docgiveplace { get; set; }
        public int docgiveplaceid { get; set; }
        public string datebegin { get; set; }
        public string dateend { get; set; }
        public int status { get; set; }
    }
}
