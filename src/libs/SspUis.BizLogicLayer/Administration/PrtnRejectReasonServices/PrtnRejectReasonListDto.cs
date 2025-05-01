using GenericServices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WEBASE.Models;
using SspUis.DataLayer.EfClasses;
using System.ComponentModel.DataAnnotations.Schema;

namespace SspUis.BizLogicLayer.PrtnRejectReasonServices
{
    public class PrtnRejectReasonListDto : ILinkToEntity<PrtnRejectReason>, IHaveIdProp<int>
    {
        public int Id { get; set; }
        public string ShortName { get; set; }
        public string FullName { get; set; }
        public int StateId { get; set; }
        public string State { get; set; }
        public int? PrtnContractTypeId { get; set; }
        public string PrtnContractType { get; set; }
        public int? PrtnContractTypeTableId { get; set; }
        public string PrtnContractTypeTable { get; set; }
    }
}
