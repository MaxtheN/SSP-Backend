using GenericServices;
using SspUis.DataLayer;
using SspUis.DataLayer.EfClasses;
using SspUis.DataLayer.EfClasses.DualEdu;
using System;
using WEBASE.Attributes;

namespace SspUis.BizLogicLayer.DualApplicationServices
{
    public class DualApplicationListDto : DocumentListDto<long>, ILinkToEntity<DualApplication>
    {
        public Guid Id2 { get; set; }
        public int ApplicationTypeId { internal get; set; }
        public string DocNumber { get; set; }
        public string DocOn { get; set; }
        public string Status { get; set; }
        public long ContractorId { get; set; }
        public string Contractor { get; set; }
        public string ContractorInn { get; set; }
        public string ContractorPhoneNumber { get; set; }
        public int RegionId { get; set; }
        public string Region { get; set; }
        public int DistrictId { get; set; }
        public string District { get; set; }
        public string OkedCode { get; set; }
        public string Oked { get; set; }
        public int TableId { get; } = TableIdConst.DUALEDU__DOC_DUAL_APPLICATION;
        public long ApplicationId { get; set; }
		//public string Details { get; set; }
		//public string Institute { get; internal set; }
		//public string Specialty { get; internal set; }
		//public string PositionClassification { get; internal set; }
		//public int EmptyPositionsCount { get;  set; }


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
