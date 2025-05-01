using GenericServices;
using SspUis.DataLayer.EfClasses;
using SspUis.DataLayer.Repositories;

namespace SspUis.BizLogicLayer
{
    public class SrvContractTableDto 
        : ServiceContractTableDlDto, ILinkToEntity<ServiceContractTable>
    {
        public int CompletedWorksCount { get; set; }
        public string NeedChamberService { get; set; }
        public int ServicePriceTypeId { get; set; }
        public string ServicePriceType { get; set; }

        public decimal? BeginCoef { get; set; }
        public decimal? EndCoef { get; set; }
        public decimal? ConcreteCoef { get; set; }
        public bool? IsConcrete { get; set; }
    }
}