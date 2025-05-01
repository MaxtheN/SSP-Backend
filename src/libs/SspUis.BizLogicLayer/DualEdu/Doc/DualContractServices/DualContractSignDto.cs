using GenericServices;
using Newtonsoft.Json;
using SspUis.DataLayer.EfClasses;
using System;
using WEBASE.OfficeTools.Models;

namespace SspUis.BizLogicLayer.DualEdu;

public class DualContractSignDto : ILinkToEntity<DualContractSign>
{
    public long Id { get; set; }
    public long OwnerId { get; set; }
    public DateOnly DocDate { get; set; }
    public string DocNumber { get; set; }
    public Guid SignFile { get; set; }
    public Guid DataFile { get; set; }
    public string SignedUserInfo { get; set; }
    public bool IsSigned { get; set; }
    public DateTime? SignedAt { get; set; }
    public DateTime CreatedAt { get; set; }
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
                _qrSign = new QrCodeModel()
                {
                    Dpi = 512,
                    Text = "Buyruq Sanasi:" + DocDate + ", \n Buyruq Raqam" + DocNumber + ", ",
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