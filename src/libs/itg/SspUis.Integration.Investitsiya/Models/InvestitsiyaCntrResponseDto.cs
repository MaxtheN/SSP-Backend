using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SspUis.Integration.Investitsiya.Models
{
    public class InvestitsiyaCntrResponseDto
    {
        public long ResponseId { get; set; }
        public int ResultCode { get; set; }
        public string ResultNote { get; set; }
        public List<InvestitsiyaCntrTypeRecord> Records { get; set; }
    }
    public class InvestitsiyaCntrTypeRecord
    {
        public string Id { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
        public int IsDeleted { get; set; }
    }
}