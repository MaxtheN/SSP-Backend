using Microsoft.AspNetCore.Mvc;
using SspUis.BizLogicLayer.AccountServices;
using SspUis.BizLogicLayer.ContractorServices;
using SspUis.DataLayer.Repositories;
using SspUis.Integration.Soliq.Models;
using StatusGeneric;
using System.Collections.Generic;
using System.Threading.Tasks;
using WEBASE.Integration.EImzo;

namespace SspUis.BizLogicLayer.BusinessmanAccountServices
{
    public interface IBusinessmanAccountService : IStatusGeneric
    {
        void ChangeLanguage(ChangeBusinessmanUserLanguageDlDto dto);
        void ChangePassword(BusinessmanChangePasswordDlDto dto);
        BusinessmanAccountUserDto GetUserInfo();
        bool IsUserRegistered(IsBusinessmanUserRegisteredDto dto);
        bool IsValidSMSCode(BusinessmanUserSmsCodeDto dto);
        Task<BusinessmanLoginResultDto> Login(BusinessmanLoginDto dto);
        Task<BusinessmanLoginResultDto> LoginByEImzo(LoginByEImzoBusinessmanDto dto);

        Task<EImzoChallangeResultDto> GetChallenge();
        IntegrationLoginResultDto IntegrationLogin(IntegrationLoginDto dto);
        Task<BusinessmanLoginResultDto> OneIdLogin(OneIdLoginDto dto);
        Task Logout();
        Task<BusinessmanLoginResultDto> Registrate(RegistrateBusinessmanDto dto);
        Task RestorePassword(RestorePasswordDto dto);
        void RestorePasswordConfirm(RestorePasswordDlDto dto);
        Task SendSMSCode(BusinessmanUserVerifyCodeDto dto);
        Task<BusinessmanLoginResultDto> SignInTwoFactor([FromBody] BusinessmanUserSmsCodeDto dto);
        Task<List<ByDirectorTinFactura>> GetContractorsList();
        Task<BusinessmanAccountUserDto> SelectContractor(string inn, string pinfl = null);
        ContractorInfoDto GetContractorInfo();
        ContractorDocumentInfo GetContractorDocumentInfo();
        Task SyncWithTax();
        Task<SoliqContractorByTinDto> GetFromTax(string inn);
        BusinessmanAccountUserDto UpdateUserInfo(UpdateBusinessmanAccountUserDto dto);
        Task SendSMSCode(IsBusinessmanUserRegisteredDto dto);
        Task ChangePhoneNumber(BusinessmanUserSmsCodeDto dto);
        List<ContractorSettlementAccountDto> GetContractorSettlementAccountList();
        Task<string> GetHash();
        Task IsOffer(OfferDto dto);
        ContractorMemshipStateDto GetContractorMemshipState();
        List<ContractorInfoListDto> GetContractorListByUserId(int businessmanUserId);
        BusinessmanLoginResultDto SetContractorToAuth(SetContractorDto dto);
        Task AddNewContractor(ToAddOrganizationDto dto);
        void DeactivateAssociation(DeactivateAssociationDto dto);
        Task<SoliqContractorDebtByPinflDto> GetFromTaxByPinfl(string pinfl);
    }
}
