using System;
using System.Collections.Generic;
using GenericServices;
using SspUis.DataLayer.EfClasses.Hrm;
using WEBASE.Models;

namespace SspUis.BizLogicLayer.Hrm
{
    public class EmployeeMissedDayListDto : DocumentListDto<long>, ILinkToEntity<EmployeeMissedDay>, IHaveIdProp<long>
    {
        public DateOnly DocDate { get; set; }
        public string Status { get; set; }
        public string Details { get; set; }
        public string Department { get; set; }
        public IEnumerable<string> EmployeeFullNames { get; set; }
        public int[] EmployeesId { get; set; }

        #region Actions
        public bool CanDelete { get; set; }
        public bool CanEdit { get; set; }
        public bool CanAccept { get; set; }
        public bool CanCancel { get; set; }
        #endregion
    }
}
