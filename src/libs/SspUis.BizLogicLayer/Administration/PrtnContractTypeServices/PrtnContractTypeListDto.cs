using GenericServices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WEBASE.Models;
using SspUis.DataLayer.EfClasses;
using System.ComponentModel.DataAnnotations.Schema;

namespace SspUis.BizLogicLayer.PrtnContractTypeServices
{
    public class PrtnContractTypeListDto : ILinkToEntity<PrtnContractType>, IHaveIdProp<int>
    {
        public int Id { get; set; }
        public string ShortName { get; set; }
        public string FullName { get; set; }
        public int EmployeeRangeFrom { get; set; }
        public int? EmployeeRangeTo { get; set; }
        public int StateId { get; set; }
        public string State { get; set; }
        public int CertificatePeriodInYears { get; set; }
    }
}
