using SspUis.DataLayer.Repositories;
using StatusGeneric;
using System.Threading.Tasks;
using WEBASE.Integration.EImzo;

namespace SspUis.BizLogicLayer.AccountServices
{
    public interface IAccountService : IStatusGeneric
    {
        AccountUserDto GetUserInfo();
        void ChangePassword(ChangePasswordDlDto dto);
        void ChangeLanguage(ChangeUserLanguageDlDto dto);
        LoginResultDto Login(LoginDto dto);
        Task<LoginResultDto> OneIdLogin(OneIdLoginDto dto);
        Task Logout();
        Task<LoginResultDto> LoginByEImzo(LoginByEImzoDto dto);
        Task<EImzoChallangeResultDto> GetChallenge();
        Task RecoverPassword(RecoverPasswordDto dto);
        void RecoveredPasswordConfirm(RestoredPasswordDlDto dto);
    }
}
