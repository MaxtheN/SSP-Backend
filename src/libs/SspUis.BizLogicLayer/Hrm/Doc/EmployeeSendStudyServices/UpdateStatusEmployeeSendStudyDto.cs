using GenericServices;
using SspUis.Core;
using SspUis.DataLayer.Repositories.Hrm;
using WEBASE.Attributes;

namespace SspUis.BizLogicLayer.Hrm
{
    public class UpdateStatusEmployeeSendStudyDto : UpdateStatusEmployeeSendStudyDlDto
    {
        internal new int StatusId { get => base.StatusId; set => base.StatusId = value; }
        [LocalizedRequired]
        public string SignedData { get; set; }
    }
    public class SignStatusEmployeeSendStudyDto : UpdateStatusEmployeeSendStudyDto
    {
        public SignStatusEmployeeSendStudyDto()
        {
            base.StatusId = StatusIdConst.SIGNING;
        }
    }
}
