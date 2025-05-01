using System;
using GenericServices;
using Newtonsoft.Json;
using SspUis.DataLayer.EfClasses;
using WEBASE.OfficeTools.Models;

namespace SspUis.BizLogicLayer.ArbitrationCourtApplicationServices;

public class ArbitrationCourtApplicationSignDto : ILinkToEntity<ArbitrationCourtApplicationSigner>
{
    public long Id { get; set; }
    public int DepartmentId { get; set; }
    public string Department { get; set; }
    public int SignOrder { get; set; }
    public int PositionId { get; set; }
    public DateOnly DocOn { get; set; }
    public string DocNumber { get; set; }
    public string Position { get; set; }
    public string Employee { get; set; }
    public string SignedUserInfo { get; set; }
    public long? EmployeeManageId { get; set; }
    public int? ArbitrationJudgeId { get; set; }
    public string ArbitrationJudge { get; set; }
    public DateTime? SignedAt { get; set; }
    public DateTime CreatedAt { get; set; }
    public int StepId { get; set; }

    private QrCodeModel _qrSign;

    [System.Text.Json.Serialization.JsonIgnore]
    [JsonIgnore]
    public QrCodeModel QrSign
    {
        get
        {
            if (this._qrSign == null)
                _qrSign = new QrCodeModel()
                {
                    Dpi = 512,
                    Text = DocOn.ToString("dd.MM.yyyy") + ", " + DocNumber + ", "
                             + Position + ", " + Employee,
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
