
namespace SspUis.Integration.Investitsiya.Models
{
    public class InvestitsiyaRequestDto
    {
        public DateOnly? DocDateFrom { get; set; }
        public DateOnly? DocDateTo { get; set;}
        public string ContractorUzInn { get; set; }
        //public string? CntrType { get ; set; } 
        //public string? CntrStatus { get; set; }

    }
}