using System;
using SspUis.DataLayer;
using WEBASE.OfficeTools.Attributes;

namespace SspUis.BizLogicLayer;

[PrintableModel("Qo'shimcha kelishuv", TableIdConst.ADDITIONAL_AGREEMENT)]
public class MemshipAdditionalAgreementDto : AdditionalAgreementDto
{
    public MemshipAdditionalAgreementDto()
    {
    }

    public string OrganizationAddress { get; set; }
    public string OrganizationAccountCode { get; set; }
    public string OrganizationBank { get; set; }
    public string OrganizationMFO { get; set; }
    public string OrganizationInn { get; set; }
    public string OrganizationOked { get; set; }
    public string OrganizationPhone { get; set; }
    public string OrganizationDirector { get; set; }
    public string ContractorAddress { get; set; }
    public string ContractorAccountCode { get; set; }
    public string ContractorBank { get; set; }
    public string ContractorMFO { get; set; }
    public string ContractorInn { get; set; }
    public string ContractorOked { get; set; }
    public string ContractorPhone { get; set; }
    //public string ContractorFaks { get; set; }
    public string ContractorDirector { get; set; }
    public string MemshipContractDocNumber { get; set; }
    public DateOnly MemshipContractDocOn { get; set; }
    public DateTime? MemshipContractSignedAt { get; set; }

    public override string ToString()
    {
        return base.ToString();
    }
}
