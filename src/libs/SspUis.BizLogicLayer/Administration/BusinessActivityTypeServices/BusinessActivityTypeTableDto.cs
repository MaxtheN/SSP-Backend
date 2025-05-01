using GenericServices;
using SspUis.DataLayer.EfClasses;
using SspUis.DataLayer.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SspUis.BizLogicLayer.BusinessActivityTypeServices
{
    public class BusinessActivityTypeTableDto : BusinessActivityTypeTableDlDto, ILinkToEntity<BusinessActivityTypeTable>
    {
        public string Currency { get; set; }
    }
}
