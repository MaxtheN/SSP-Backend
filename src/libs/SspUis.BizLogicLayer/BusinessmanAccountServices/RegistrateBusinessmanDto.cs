using SspUis.DataLayer.Repositories;

namespace SspUis.BizLogicLayer.BusinessmanAccountServices
{
    public class RegistrateBusinessmanDto : RegistrateBusinessmanUserDlDto
    {
        public string SignedData { get; set; }
        public string State { get; set; }
    }
}
