using System;
using System.Collections.Generic;
using GenericServices;
using Newtonsoft.Json;
using SspUis.BizLogicLayer.PersonServices;
using SspUis.DataLayer;
using SspUis.DataLayer.EfClasses;
using SspUis.DataLayer.EfClasses.Hrm;
using SspUis.DataLayer.Repositories.Hrm;
using WEBASE.Models;
using WEBASE.OfficeTools.Attributes;
using WEBASE.OfficeTools.Models;

namespace SspUis.BizLogicLayer.Hrm
{
    public class EmployeeMissedDayDto : UpdateEmployeeMissedDayDlDto, ILinkToEntity<EmployeeMissedDay>, IDocument
    {
        public string Department { get; set; }
        public string Position { get; set; }
        public int StatusId { get; set; }
        public int OrganizationId { get; set; }
        public string Status { get; set; }
        public List<EmployeeMissedDayTableDto>? Tables { get; set; } = new();
    }
}
