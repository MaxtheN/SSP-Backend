using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GenericServices;
using SspUis.DataLayer.EfClasses.Hrm;
using SspUis.DataLayer.Repositories;

namespace SspUis.BizLogicLayer.Hrm.CalculationKindServices
{
    public class CalculationKindStructureDto : CalculationKindStructureDlDto, ILinkToEntity<CalculationKindStructure>
    {
        public object OrganizationalStructure { get; internal set; }
    }

}
