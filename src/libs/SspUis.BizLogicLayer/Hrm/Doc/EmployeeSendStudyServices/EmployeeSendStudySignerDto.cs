using GenericServices;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Newtonsoft.Json;
using System.Threading.Tasks;
using WEBASE.Attributes;
using WEBASE.Models;
using SspUis.DataLayer.Repositories;
using SspUis.DataLayer.EfClasses.Hrm;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using WEBASE.OfficeTools.Models;

namespace SspUis.BizLogicLayer.Hrm
{
    public class EmployeeSendStudySignerDto : ILinkToEntity<EmployeeSendStudySigner>
    {
        public long Id { get; set; }
        public int DepartmentId { get; set; }
        public string Department { get; set; }
        public int SignOrder { get; set; }
        public  string DocNumber { get; set; }
        public  DateOnly? DocDate { get; set; }
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
                        Text = "Buyruq Sanasi:" + DocDate + ", /nBuyruq Raqam" + DocNumber + ", " +
							"/nImzolovchi" + Employee + ",/nBo'limi: " + Department + "," +
							"/nLavozimi: " + ForQrCodePosition + ",/nImzolangan vaqt " + SignedAt,
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
