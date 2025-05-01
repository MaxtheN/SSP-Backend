using SspUis.DataLayer.EfClasses.Kpi;
using SspUis.DataLayer.Repositories.Hrm;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WEBASE.EF;
using WEBASE.Models;

namespace SspUis.DataLayer.Repositories.Kpi;

public class UpdateKpiGratingDlDto : KpiGratingDlDto<UpdateKpiGratingDlDto>, IHaveIdProp<long>
{
    public long Id { get; set; }
   
}
