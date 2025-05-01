using WEBASE.Attributes;
using WEBASE.EF;
using WEBASE.Models;
using SspUis.DataLayer.EfClasses.Claim;
using GenericServices;
using System.Text.Json.Serialization;

namespace SspUis.DataLayer.Repositories
{
    public class ClaimApplicationTableDlDto : EntityDto<ClaimApplicationTableDlDto, ClaimApplicationTable>,
        IHaveIdProp<long>,
        ILinkToEntity<ClaimApplicationTable>
    {
        public long Id { get; set; }
        [LocalizedRequired]
        [LocalizedStringLength(30)]
        public string OrderNumber { get; set; }
        [LocalizedRequired]
        [LocalizedRange(1, int.MaxValue)]
        public int ClaimResponsibleTypeId { get; set; }
        [LocalizedRequired]
        [LocalizedMinLength(9)]
        [LocalizedStringLength(14)]
        public string InnOrPinfl { get; set; }
        [LocalizedRequired]
        [LocalizedStringLength(500)]
        public string FullName { get; set; }
        [LocalizedRequired]
        [LocalizedStringLength(500)]
        public string Address { get; set; }
        [LocalizedRequired]
        [LocalizedMinLength(12)]
        [LocalizedStringLength(50)]
        public string PhoneNumber { get; set; }
        [JsonIgnore]
        public bool? IsRegistred { get; set; }
    }
}
