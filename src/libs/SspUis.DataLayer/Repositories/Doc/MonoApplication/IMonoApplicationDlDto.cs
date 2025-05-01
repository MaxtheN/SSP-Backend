using System.Collections.Generic;

namespace SspUis.DataLayer.Repositories
{
    public interface IMonoApplicationDlDto
    {
        public long ApplicationId { get; set; }
        public long MfyId { get; set; }
        public decimal TotalAmount { get; set; }
        public int CurrencyId { get; set; }
        public decimal SpendForBuild { get; set; }
        public long MonoMfyId { get; set; }
        public int MonoRegionId { get; set; }
        public int MonoDistrictId { get; set; }
        public string MonoAdress { get; set; }
        public int BuildingCount { get; set; }
        public decimal LearningArea { get; set; }
        public decimal TotalArea { get; set; }
        public List<MonoApplicationStudentTableDlDto> StudentTables { get; set; }
        public List<MonoApplicationItemTableDlDto> ItemTables { get; set; }
        public List<MonoApplicationFileDlDto> Files { get; set; }
    }
}
