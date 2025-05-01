using GenericServices;
using SspUis.BizLogicLayer.Doc.MonoApplicationServices;
using SspUis.DataLayer;
using SspUis.DataLayer.EfClasses;
using System;
using WEBASE.Models;

namespace SspUis.BizLogicLayer.MonoApplicationServices
{
    public class MonoApplicationListDto : MonoApplicationDocumentListDto<long>, ILinkToEntity<MonoApplication>, IHaveIdProp<long>
    {
        public ApplicationListDto Application { get; set; }
        public decimal? TotalAmount { get; set; }
        public int? CurrencyId { get; set; }
        public string Currency { get; set; }
        public long MfyId { get; set; }
        public string Mfy { get; set; }
        public decimal SpendForBuild { get; set; }
        public long MonoMfyId { get; set; }
        public string MonoMfy { get; set; }
        public int MonoRegionId { get; set; }
        public string MonoRegion { get; set; }
        public int MonoDistrictId { get; set; }
        public string MonoDistrict { get; set; }
        public string MonoAdress { get; set; }
        public string Status { get; set; }
        public int BuildingCount { get; set; }
        public decimal LearningArea { get; set; }
        public decimal TotalArea { get; set; }
        public int TableId { get; } = TableIdConst.DOC_MONO_APPLICATION;

    }
}
