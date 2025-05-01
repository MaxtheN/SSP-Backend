using GenericServices;
using SspUis.BizLogicLayer.ApplicationServices;
using SspUis.DataLayer;
using SspUis.DataLayer.EfClasses;
using System;
using WEBASE.Attributes;

namespace SspUis.BizLogicLayer.MemshipApplicationServices
{
    public class MemshipApplicationListDto : DocumentListDto<long>, ILinkToEntity<MemshipApplication>
    {
        public ApplicationListDto Application { get; set; }
        public string OkedCode { get; set; }
        public string Oked { get; set; }
        public int TableId { get; } = TableIdConst.MEMSHIP__DOC_MEMSHIP_APPLICATION;
        public bool IsRead { get; set; }
        public int ContractorActivityTypeId { get; set; }
        public string ContractorActivityType { get; set; }
        public int ContractorCategoryId { get; set; }
        public int? OpfId { get; set; }
        public string? Opf { get; set; }
        public string ContractorCategory { get; set; }
        public int EmployeesCount { get; set; }
        public decimal? YearlyEarnings { get; set; }
        public decimal? YearlyTaxes { get; set; }
        public decimal? YearlyExport { get; set; }
        public decimal? YearlyImport { get; set; }
        public decimal? YearlyManufacture { get; set; }
        public bool ChooseLocation { get; set; }
        public int? ChoosedRegionId { get; set; }
        public int? ChoosedDistrictId { get; set; }
        public string ChoosedRegion { get; set; }
        public string ChoosedDistrict { get; set; }

        #region Actions
        public bool CanAccept { get; set; }
        //public bool CanReject { get; set; }
        public bool CanEdit { get; set; }
        public bool CanRevoke { get; set; }
        public bool CanSend { get; set; }
        public bool CanCancel { get; set; }
        public bool CanDelete { get; set; }

        #endregion
    }
}
