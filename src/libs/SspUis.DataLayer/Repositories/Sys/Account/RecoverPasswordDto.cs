using SspUis.DataLayer.EfClasses;
using System.ComponentModel.DataAnnotations;
using WEBASE.Attributes;
using WEBASE.EF;

namespace SspUis.DataLayer.Repositories;
public class RecoverPasswordDto
{
    [LocalizedRequired]
    public string Username { get; set; }
    public string? AppKeyHash { get; set; }
}

public class RestoredPasswordDlDto : EntityDto<RestoredPasswordDlDto, User>
{
    [LocalizedRequired]
    public string Username { get; set; }

    [LocalizedRequired]
    public string SmsCode { get; set; }

    [DataType(DataType.Password)]
    [LocalizedRequired]
    public string NewPassword { get; set; }

    [DataType(DataType.Password)]
    [LocalizedRequired]
    public string ConfirmedNewPassword { get; set; }

    public override void UpdateEntity(User entity)
    {
        entity.SetPassword(NewPassword, true);
    }
}