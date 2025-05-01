using SspUis.DataLayer.Repositories.Kpi;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SspUis.BizLogicLayer.Kpi;

internal class UpdateStatusKpiPlanForEmployeeDto : UpdateStatusKpiPlanForEmployeeDlDto
{
    internal new int StatusId { get => base.StatusId; set => base.StatusId = value; }
}

