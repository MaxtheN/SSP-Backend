using GenericServices;
using SspUis.DataLayer.EfClasses;
using SspUis.DataLayer.Repositories;
using System;
using System.Collections.Generic;

namespace SspUis.BizLogicLayer.MemshipApplicationServices
{
    public class MemshipApplicationDto : UpdateMemshipApplicationDlDto, ILinkToEntity<MemshipApplication>,
        IBaseApplication<SspUis.BizLogicLayer.ApplicationDto>
    {
        public new long Id { get => base.Id; set => base.Id = value; }
        public new SspUis.BizLogicLayer.ApplicationDto Application { get; set; }
        public string ContractorActivityType { get; set; }
        public string ContractorCategory { get; set; }
        public string Oked { get; set; }
        public long? MemshipContractId { get; set; } = null;
        public int MemshipContractTypeId { get; set; }
        public decimal? NowYearlyEarnings { get; set; }
        public string Organization { get; set; }
        public DateOnly RegistrationDate { get; set; }
        public string? OwnerName { get; set; }
        public string ChoosedRegion { get; set; }
        public string ChoosedDistrict { get; set; }
        public new List<MemshipApplicationFileDto> Files { get; set; }

        #region Actions
        public bool CanAccept { get; set; }
        //public bool CanReject { get; set; }
        public bool CanEdit { get; set; }
        public bool CanRevoke { get; set; }
        public bool CanSend { get; set; }
        public bool CanCancel { get; set; }
        public bool CanPaidCancel { get; set; }
        public bool CanDelete { get; set; }
        public bool CanSelectOrganization { get; set; } = false;

        #endregion
    }
}
