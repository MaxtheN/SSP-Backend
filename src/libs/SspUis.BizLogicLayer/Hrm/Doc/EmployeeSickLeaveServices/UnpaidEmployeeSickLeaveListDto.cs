using GenericServices;
using iText.Layout.Element;
using SspUis.DataLayer.EfClasses.Hrm;
using SspUis.DataLayer.Repositories.Hrm;
using System;
using System.Collections.Generic;
using WEBASE.Models;

namespace SspUis.BizLogicLayer.Hrm
{
    public class UnpaidEmployeeSickLeaveListDto : EmployeeSickLeaveTableDlDto, ILinkToEntity<EmployeeSickLeaveTable>
    {
        public string DepartmentName { get; set; } = null!;
        public string EmployeeName { get; set; } = null!;
        public long DocumentId { get; set; }
        public int DocumentStatusId { get; set; }
        public DateOnly DocOn { get; set; }
        public int CalculationKindId { get; set; }
    }
}
