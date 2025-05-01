using GenericServices;
using SspUis.Core;
using SspUis.DataLayer.Repositories.Hrm;
using WEBASE.Attributes;

namespace SspUis.BizLogicLayer.Hrm
{
    public class UpdateStatusTempCalcKindDto : UpdateStatusTempCalcKindDlDto
    {
        internal new int StatusId { get => base.StatusId; set => base.StatusId = value; }
        [LocalizedRequired]
        public string SignedData { get; set; }
    }
    public class SignStatusTempCalcKindDto : UpdateStatusTempCalcKindDto
    {
        public SignStatusTempCalcKindDto()
        {
            base.StatusId = StatusIdConst.SIGNING;
        }
    }
}
