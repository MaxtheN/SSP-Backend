using AutoMapper;
using SspUis.DataLayer.EfClasses;
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



public class KpiGratingIndicatorDlDto: EntityDto<KpiGratingIndicatorDlDto, KpiGratingIndicator>, IHaveIdProp<long>
{
    public long Id { get; set; }
    public int IndicatorId { get; set; }
    public List<KpiGratingIndicatorTableDlDto> Tables { get; set; }
    protected override Action<IMappingExpression<KpiGratingIndicatorDlDto, KpiGratingIndicator>> AlterMapping =>
       cfg => cfg.ForMember(x => x.Tables, c => c.Ignore());
}


