using GenericServices;
using SspUis.DataLayer.EfClasses;
using SspUis.DataLayer.EfClasses.Hrm;
using SspUis.DataLayer.Repositories.Hrm;
using System;
using System.Collections.Generic;
using WEBASE.Models;

namespace SspUis.BizLayer.Hrm.StaffingTemplateServices
{
    public class StaffingTemplateDto : UpdateStaffingTemplateDlDto, ILinkToEntity<StaffingTemplate>, IHaveIdProp<long>, IDocument
    {
        public int StatusId { get; set; }
        public string Status { get; set; }
        new public List<StaffingTemplateTableDto> Tables { get; set; } = new();


        #region Actions
        public bool CanModify { get; set; }
        public bool CanAccept { get; set; }
        public bool CanCancel { get; set; }
        public bool CanDelete { get; set; }
        #endregion
    }
}
