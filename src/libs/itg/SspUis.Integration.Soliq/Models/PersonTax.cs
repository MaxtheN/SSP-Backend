using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SspUis.Integration.Soliq.Models
{
    public class PersonTax
    {
        public int? ns10Code { get; set; }
        public int? ns11Code { get; set; }
        public string tin { get; set; }
        public string fullName { get; set; }
        public string passSeries { get; set; }
        public string passNumber { get; set; }
        public string passOrg { get; set; }
        public string passIssueDate { get; set; }
        public string address { get; set; }
        public string isItd { get; set; }
        public string personalNum { get; set; }
    }
}