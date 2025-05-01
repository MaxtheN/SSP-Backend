using GenericServices;
using SspUis.DataLayer;
using SspUis.DataLayer.EfClasses;
using SspUis.DataLayer.EfClasses.Hrm;
using SspUis.DataLayer.Repositories.Hrm;
using System.Collections.Generic;

namespace SspUis.BizLogicLayer.Hrm.StaffingServices
{
    public class StaffingDtoForChamber
    {
        public string Department { get; set; } 
        public List<string> Positions { get; set; } 
    }
}
