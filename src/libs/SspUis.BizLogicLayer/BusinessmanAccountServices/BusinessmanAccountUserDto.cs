using GenericServices;
using SspUis.BizLogicLayer.ContractorServices;
using SspUis.DataLayer.EfClasses;
using WEBASE.Models;

namespace SspUis.BizLogicLayer.BusinessmanAccountServices
{
    public class BusinessmanAccountUserDto : ILinkToEntity<BusinessmanUser>, IHaveStateId
    {
        public int Id { get; set; }
        public string UserName { get; set; }
        public string FullName { get; set; }
        public string ShortName { get; set; }
        public string Inn { get; set; }
        public string Pinfl { get; set; }
        public int? LanguageId { get; set; }
        public int StateId { get; set; }
        public long? ContractorId { get; set; }
        public ContractorDto Contractor { get; set; }
        public string Email { get; set; }
        public string Language { get; set; }
    }
}
