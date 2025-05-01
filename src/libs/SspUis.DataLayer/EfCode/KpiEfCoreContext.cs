using Microsoft.EntityFrameworkCore;
using SspUis.DataLayer.EfClasses;
using SspUis.DataLayer.EfClasses.Kpi;
using WEBASE.EF;

namespace SspUis.DataLayer.EfCode;

public partial class EfCoreContext : BaseDbContext
{
	#region Info
	public virtual DbSet<Indicator> Indicators { get; set; }
	public virtual DbSet<IndicatorTable> IndicatorTables { get; set; }
	public virtual DbSet<IndicatorTranslate> IndicatorTranslates { get; set; }

    public virtual DbSet<IndicatorDistrict> IndicatorDistricts { get; set; }
    public virtual DbSet<IndicatorDistrictTable> IndicatorDistrictTables { get; set; }
    public virtual DbSet<IndicatorDistrictTranslate> IndicatorDistrictTranslates { get; set; }
    public virtual DbSet<IndicatorDepartment> IndicatorDepartments { get; set; }
    public virtual DbSet<IndicatorDepartmentTranslate> IndicatorDepartmentTranslates { get; set; }
    #endregion

    #region Doc
    public virtual DbSet<KpiGrating> KpiGratings { get; set; }
    public virtual DbSet<KpiGratingIndicator> KpiGratingIndicators { get; set; }
    public virtual DbSet<KpiGratingIndicatorTable> KpiGratingIndicatorTables { get; set; }

    public virtual DbSet<KpiPlanEmployeeIndicatorCreate> KpiPlanEmployeeIndicatorCreates { get; set; }
    public virtual DbSet<KpiPlanForEmployee> KpiPlanForEmployees { get; set; }
    public virtual DbSet<KpiPlanForEmployeeTable> KpiPlanForEmployeeTables { get; set; }

    public virtual DbSet<KpiRatingEmployee> KpiRatingEmployees { get; set; }
    public virtual DbSet<KpiRatingEmployeeTable> KpiRatingEmployeeTables { get; set; }
    public virtual DbSet<KpiRatingEmployeePoint> KpiRatingEmployeePoints { get; set; }
    #endregion


    #region UnitOfMeasure
    public virtual DbSet<UniteOfMeasure> UniteOfMeasures { get; set; }
	public virtual DbSet<UniteOfMeasureTranslate> UniteOfMeasureTranslates { get; set; }
	#endregion
}