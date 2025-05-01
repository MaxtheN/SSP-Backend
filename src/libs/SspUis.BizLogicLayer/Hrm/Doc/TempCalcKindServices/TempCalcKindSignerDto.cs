using System;
using GenericServices;
using Newtonsoft.Json;
using SspUis.DataLayer.EfClasses.Hrm;
using WEBASE.OfficeTools.Models;

namespace SspUis.BizLogicLayer.Hrm
{
    public class TempCalcKindSignerDto : ILinkToEntity<TempCalcKindSigner>
    {
        public long Id { get; set; }
        public int DepartmentId { get; set; }
        public string Department { get; set; }
        public int SignOrder { get; set; }
		public string DocNumber { get; set; }
		public DateOnly? DocDate { get; set; }
		public int PositionId { get; set; }
        public string Position { get; set; }
		public string ForQrCodePosition { get; set; }
		public string Employee { get; set; }
        public string SignedUserInfo { get; set; }
        public long EmployeeManageId { get; set; }
        public bool IsHr { get; set; }
        public bool IsDirector { get; set; }
        public bool IsSigned { get; set; }
        public string QrSignValue { get; set; } = "++QrSign++";
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
						Text = "Buyruq Sanasi:" + DocDate + ", \n Buyruq Raqam" + DocNumber + ", " +
							"\n Imzolovchi" + Employee + ",\n Bo'limi: " + Department + "," +
							"\n Lavozimi: " + ForQrCodePosition + ",\n Imzolangan vaqt " + SignedAt ?? "",
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
}
