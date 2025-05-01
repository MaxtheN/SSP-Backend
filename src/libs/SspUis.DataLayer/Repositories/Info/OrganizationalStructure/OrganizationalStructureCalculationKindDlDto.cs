using SspUis.DataLayer.EfClasses;
using WEBASE.Attributes;
using WEBASE.EF;
using WEBASE.Models;

namespace SspUis.DataLayer.Repositories.Info.OrganizationalStructure
{
    public class OrganizationalStructureCalculationKindDlDto : EntityDto<OrganizationalStructureCalculationKindDlDto, OrganizationalStructureCalculationKind>, IHaveIdProp<int>
    {
        public int Id { get; set; }
        [LocalizedRequired]
        [LocalizedRange(1, int.MaxValue)]
        public int CalculationKindId { get; set; }
        [LocalizedRequired]
        public decimal Percentage { get; set; }
    }
}
