using System.Collections.Generic;
using GenericServices;
using SspUis.DataLayer;
using SspUis.DataLayer.EfClasses;
using SspUis.DataLayer.EfClasses.DualEdu;
using SspUis.DataLayer.Repositories;

namespace SspUis.BizLogicLayer.DualApplicationServices
{
    public class DualApplicationDto : UpdateDualApplicationDlDto, ILinkToEntity<DualApplication>,
        IBaseApplication<ApplicationDto>
    {
        public new long Id { get => base.Id; set => base.Id = value; }
        public new ApplicationDto Application { get; set; }
        public int TableId { get; } = TableIdConst.DUALEDU__DOC_DUAL_APPLICATION;
        //public int StatusId { get; set; }
        //public string Status { get; set; }
        //public string Contractor { get; set; }
        //public long ContractorId { get; set; }
        //public string ContractorDirector { get; set; }
        public string ContractorInn { get; set; }
        //public string ContractorAddress { get; set; }
        //public string? ContractorForm { get; set; }
        //public string ApplicationType { get; set; }
        //public new int ApplicationTypeId { get; set; }
        //public int RegionId { get; set; }
        //public int DistrictId { get; set; }
        //public string Region { get; set; }
        //public string District { get; set; }
        public string DualEducationType { get; set; }

        public new List<DualApplicationTableDto> Tables { get; set; } = new();

        #region Actions
        public bool CanAccept { get; set; }
        public bool CanReject { get; set; }
        public bool CanEdit { get; set; }
        public bool CanRevoke { get; set; }
        public bool CanSend { get; set; }
        public bool CanCancel { get; set; }
        public bool CanDelete { get; set; }

        #endregion
    }
}
