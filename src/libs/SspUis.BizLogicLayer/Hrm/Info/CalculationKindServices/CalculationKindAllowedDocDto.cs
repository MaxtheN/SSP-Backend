using AutoMapper;
using GenericServices;
using GenericServices.Configuration;
using SspUis.DataLayer.EfClasses.Hrm;
using SspUis.DataLayer.Repositories;

namespace SspUis.BizLogicLayer.Hrm.CalculationKindServices
{
    public class CalculationKindAllowedDocDto : CalculationKindAllowedDocDlDto, ILinkToEntity<CalculationKindAllowedDoc>
    {
        public string? Table { get; set; } = null!;
        public string? State { get; set; } = null!;
    }
}
