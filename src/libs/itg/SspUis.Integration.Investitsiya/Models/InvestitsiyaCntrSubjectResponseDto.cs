using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static WEBASE.Storage.StaticFileConst;

namespace SspUis.Integration.Investitsiya.Models
{
    public class InvestitsiyaCntrSubjectResponseDto
    {
        public long ResponseId { get; set; }
        public int ResultCode { get; set; }
        public string ResultNote { get; set; }
        public List<CntrSubjectDto> Records { get; set; }
    }
    public class CntrSubjectDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int IsDeleted { get; set; }
    }
}