using SspUis.DataLayer.Repositories;
using SspUis.DataLayer.Repositories.Kpi;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SspUis.BizLogicLayer;

public class UpdateStatusKpiGratingDto : UpdateStatusKpiGratingDlDto
{
    internal new int StatusId { get => base.StatusId; set => base.StatusId = value; }
}
