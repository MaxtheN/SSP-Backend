using System;
using GenericServices;
using Newtonsoft.Json;
using SspUis.DataLayer.EfClasses;
using SspUis.DataLayer.Repositories;
using WEBASE.OfficeTools.Models;

namespace SspUis.BizLogicLayer;

public class ArbitrationResultSignDto :
    ArbitrationResultSignDlDto,
    ILinkToEntity<ArbitrationResultSign>
{
    //public string? Status { get; set; }
    public string? SignedUserInfo { get; set; }
    public DateTime? SignedAt { get; set; }
    #region ArbitrationJudge
    public string? FirstName { get; set; }
    public string? LastName { get; set; }
    public string? MiddleName { get; set; }
    public string PositionName { get; set; }

    public string ShortName
    {
        get
        {
            return FirstName.Trim()[0].ToString().ToUpper() + "."
                + MiddleName.Trim()[0].ToString().ToUpper() + "." + LastName;
        }
    }
    #endregion
    //public ArbitrationJudgeDto ArbitrationJudge { get; set; }

    [JsonIgnore]
    [System.Text.Json.Serialization.JsonIgnore]
    private QrCodeModel _qrSign;

    [JsonIgnore]
    [System.Text.Json.Serialization.JsonIgnore]
    public QrCodeModel QrSign
    {
        get
        {
            if (this._qrSign == null)
                if (SignedAt != null)
                    _qrSign = new QrCodeModel()
                    {
                        Dpi = 256,
                        Text = FirstName
                        + "  " + LastName
                        + "  " + MiddleName
                        + "  " + PositionName
                        + "  " + SignedAt,
                        Width = 256,
                        Height = 256,
                    };
            return _qrSign;
        }
        set
        {
            _qrSign = value;
        }
    }
}
