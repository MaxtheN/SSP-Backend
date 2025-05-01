using WEBASE.Integration.EImzo;

namespace SspUis.BizLogicLayer.BusinessmanAccountServices
{
    public class BusinessmanLoginDto
    {
        public string PhoneNumber { get; set; }
        public string Password { get; set; }
        public string PasswordConfirm { get; set; }
        public string AppKeyHash { get; set; }
    }

    public class IsBusinessmanUserRegisteredDto
    {
        public string PhoneNumber { get; set; }
    }

    public class BusinessmanUserVerifyCodeDto : IsBusinessmanUserRegisteredDto
    {
        public string Password { get; set; }
        public string PasswordConfirm { get; set; }
    }

    public class BusinessmanUserSmsCodeDto : OfferDto
    {
        public string SmsCode { get; set; }
    }

    public class RestorePasswordDto
    {
        public string UserName { get; set; }
        public string? AppKeyHash { get; set; }
    }
    public class LoginByEImzoBusinessmanDto : EImzoAuthDto
    {
        public int? LanguageId { get; set; }
    }
}
