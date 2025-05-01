using GenericServices;
using SspUis.Core;
using SspUis.DataLayer.Repositories.Hrm;
using WEBASE.Attributes;

namespace SspUis.BizLogicLayer.Hrm
{
    public class UpdateStatusEmployeeSendTrainDto : UpdateStatusEmployeeSendTrainDlDto
    {
        internal new int StatusId { get => base.StatusId; set => base.StatusId = value; }
        [LocalizedRequired]
        public string SignedData { get; set; }
    }
    public class SignStatusEmployeeSendTrainDto : UpdateStatusEmployeeSendTrainDto
    {
        public SignStatusEmployeeSendTrainDto()
        {
            base.StatusId = StatusIdConst.SIGNING;
        }
    }
}
