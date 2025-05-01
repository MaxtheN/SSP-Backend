using SspUis.DataLayer.Repositories;

namespace SspUis.BizLogicLayer.JoinAntiCorruptionApplicationServices
{
    public class JoinAntiCorruptionApplicationEmployeeDto : JoinAntiCorruptionApplicationEmployeeDlDto
    {
        public string Person { get; set; }
        public string Position { get; set; }
    }
}
