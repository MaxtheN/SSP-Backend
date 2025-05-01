using System.Collections.Generic;

namespace SspUis.DataLayer.Repositories
{
    public interface IClaimApplicationDlDto
    {
        public long ApplicationId { get; set; }
        public long MemshipContractId { get; set; }
        public long? MemshipCertificateId { get; set; }
        public long? PrevApplicationId { get; set; }
        public int ClaimApplicationTypeId { get; set; }
        public int ClaimThemeId { get; set; }
        public string Details { get; set; }
        public string ContractorInn { get; set; }
        public decimal? MainDebt { get; set; }
        public decimal? CalculedPenalty { get; set; }
        public decimal? Penalty { get; set; }
        public decimal? Percent { get; set; }
        public int CurrencyId { get; set; }
        public List<ClaimApplicationTableDlDto> Tables { get; set; }
        public List<ClaimApplicationFileDlDto> Files { get; set; }
    }
}
