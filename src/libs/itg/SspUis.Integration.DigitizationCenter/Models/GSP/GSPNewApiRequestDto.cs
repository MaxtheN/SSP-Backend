using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SspUis.Integration.DigitizationCenter.Models.GSP
{
    public class GSPNewApiRequestDto
    {
        public int transaction_id { get; set; }
        public string is_consent { get; set; }
        public int langId { get; set; }
        public string document { get; set; }
        public string birth_date { get; set; }
        public string is_photo { get; set; }
    }
}
