using SspUis.DataLayer.Repositories.Hrm;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SspUis.BizLogicLayer.Hrm.StaffingServices
{
    public class UpdateStatusStaffingDto : UpdateStatusStaffingDlDto
    {
        public new int StatusId { get => base.StatusId; internal set => base.StatusId = value; }
    }
}
