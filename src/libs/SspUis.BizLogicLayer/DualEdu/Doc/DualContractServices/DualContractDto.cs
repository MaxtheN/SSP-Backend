using GenericServices;
using Newtonsoft.Json;
using SspUis.DataLayer.EfClasses;
using SspUis.DataLayer.Repositories;
using WEBASE.OfficeTools.Models;

namespace SspUis.BizLogicLayer;

public class DualContractDto : UpdateDualContractDlDto, ILinkToEntity<DualContract>, IDocument
{
    public string Institute { get; set; }
    public string InstituteInn { get; set; }
    public string Speciality { get; set; }
    public string EduType { get; set; }
    public string EduSpeciality { get; set; }
    public string Status { get; set; }
    public int StatusId { get; set; }
    public string DualEducationType { get; set; }
    public string Contractor { get; set; }
    public string ContractorPhoneNumber { get; set; }
    public string RectorName { get; set; }
    public string ContractorOrganizationName { get; set; }
    public string ContractorAddress { get; set; }
    public string ContractorDirector { get; set; }
    public string OrganizationTel { get; set; } 
    public string StudentName { get; set; }
    public string OrganizationInn { get; set; }
    public string StudentAddress { get; set; }
    public string StudentNumber { get; set; }
    public string PassportInfo { get; set; }
    public string ContractorDirectorName { get; set; } 
    public string ContractorPostal { get; set; } 
    public string ContractorPhoneNumber2 { get; set; } 
    public string ContractorAccountCode { get; set; } 
    public string ContractorBankName { get; set; } 
    public string ContractorBankCode { get; set; } 
    public string ContractorInn { get; set; } 
    public string RectorNameForSign { get; set; } 
    public string ContractorDirectorNameForSign { get; set; } 
    public string StudentNameForSign { get; set; }
    [JsonIgnore]
    public long? CurrentDualContractSignId { get; set; }
    [JsonIgnore]
    public string CurrentDualContractSignPinfl { get; set; }
    public string QrSignValue { get; set; } = "++QrSign0++";
    public string QrSignValueA { get; set; } = "++QrSignA++";
    public string QrSignValueB { get; set; } = "++QrSignB++";
    public QrCodeModel QrSign0 { get; set; }
    public QrCodeModel QrSignA { get; set; }
    public QrCodeModel QrSignB { get; set; }


    #region
    public bool CanReject { get; set; }
    public bool CanSign { get; set; }
    #endregion
   
}