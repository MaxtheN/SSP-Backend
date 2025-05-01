using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GenericServices;
using SspUis.DataLayer.EfClasses;
using WEBASE.Attributes;

namespace SspUis.BizLogicLayer.Administration.ContractorContactService
{
    public class ContractorContactListDto : ILinkToEntity<ContractorContact>
    {
        public long Id { get; set; }
        public string Contact { get; set; }
        public int ContactTypeId { get; set; }
        public string ContactType { get; set; }
        public long OwnerId { get; set; }
    }
}
