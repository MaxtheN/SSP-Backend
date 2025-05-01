using GenericServices;
using SspUis.DataLayer.EfClasses.Claim;

namespace SspUis.BizLogicLayer.Claim;

public class MediationClaimApplicationTableDto : ILinkToEntity<ClaimApplicationTable>
{
    public long Id { get; set; }
    public string OrderNumber { get; set; }
    public string ClaimResponsibleType { get; set; }
    public int? ClaimResponsibleTypeId { get; set; }
    public string InnOrPinfl { get; set; }
    public string FullName { get; set; }
    public string Address { get; set; }
    public string PhoneNumber { get; set; }
    public bool? IsRegistred { get; set; }
}