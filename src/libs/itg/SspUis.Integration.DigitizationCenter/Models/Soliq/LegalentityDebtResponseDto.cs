using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SspUis.Integration.DigitizationCenter.Soliq
{
    public class LegalentityDebtResponseResultDto
    {
        public bool success { get; set; }

        public object reason { get; set; }

        public List<LegalentityDebtResponseDto> data { get; set; }
    }
    public class LegalentityDebtResponseDto
    {
        public int ns10Code { get; set; }

        public string ns10Name { get; set; }

        public int ns11Code { get; set; }

        public string ns11Name { get; set; }

        public string tin { get; set; }

        public object objectCode { get; set; }

        public object objectName { get; set; }

        public string currentDate { get; set; }

        public double nedoimka { get; set; }

        public double penya { get; set; }

        public double pereplata { get; set; }
    }
   
}
