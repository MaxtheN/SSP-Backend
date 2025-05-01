using GenericServices;
using SspUis.DataLayer.EfClasses;
using SspUis.DataLayer.Repositories;

namespace SspUis.BizLogicLayer
{
    public class ServicePriceGroupTableDto : ServicePriceTableDlDto,
        ILinkToEntity<ServicePriceTable>
    {
        public bool IsConcrete { get; set; }
        public string NeedChamberService { get; set; }
    }
}