using GenericServices;
using SspUis.DataLayer.EfClasses.DualEdu;
using SspUis.DataLayer.Repositories;

namespace SspUis.BizLogicLayer.DualApplicationServices
{
    public class DualApplicationTableDto : DualApplicationTableDlDto, ILinkToEntity<DualApplicationTable>
    {
        public string Institute { get; internal set; }
        public string Specialty { get; internal set; }
        public string PositionClassification { get; internal set; }
    }
}
