using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SspUis.Integration.Investitsiya.Models
{
    public class InvestitsiyaAgreementStateResponseDto
    {
        public long ResponseId { get; set; }
        public int ResultCode { get; set; }
        public string ResultNote { get; set; }
        public List<InvestitsiyaAgreementStates> Records { get; set; }
    }
    public class InvestitsiyaAgreementStates
    {

        public int Id { get; set; }
        public string Name { get; set; }
        public int IsDeleted { get; set; }
    }
}