using Microsoft.EntityFrameworkCore;
using SspUis.DataLayer.EfClasses;

namespace SspUis.DataLayer.EfCode
{
    public partial class EfCoreContext
    {
        #region DOCUMENT
        public virtual DbSet<ServicePrice> ServicePrice { get; set; }
        public virtual DbSet<ServicePriceGroup> ServicePriceGroup { get; set; }
        public virtual DbSet<ServicePriceTable> ServicePriceGroupTable { get; set; }

        public virtual DbSet<SrvApplicationYearlyPlan> SrvApplicationYearlyPlans { get; set; }
        public virtual DbSet<SrvApplicationYearlyPlanTable> SrvApplicationYearlyPlanTables { get; set; }

        public virtual DbSet<ServiceApplication> ServiceApplication { get; set; }
        public virtual DbSet<ServiceApplicationGroup> ServiceApplicationGroup { get; set; }
        public virtual DbSet<ServiceApplicationTable> ServiceApplicationGroupTable { get; set; }
        public virtual DbSet<ServiceApplicationTableFile> ServiceApplicationTableFile { get; set; }

        public virtual DbSet<ServiceContract> ServiceContract { get; set; }
        public virtual DbSet<ServiceContractSign> ServiceContractSign { get; set; }
        public virtual DbSet<ServiceContractGroup> ServiceContractGroup { get; set; }
        public virtual DbSet<ServiceContractTable> ServiceContractGroupTable { get; set; }

        public virtual DbSet<ServiceDeed> ServiceDeed { get; set; }
        public virtual DbSet<ServiceDeedSign> ServiceDeedSign { get; set; }
        public virtual DbSet<ServiceDeedGroup> ServiceDeedGroup { get; set; }
        public virtual DbSet<ServiceDeedTable> ServiceDeedTable { get; set; }

        public virtual DbSet<CompletedService> CompletedService { get; set; }

        public virtual DbSet<SrvYearlyPlan> SrvYearlyPlans { get; set; }
        public virtual DbSet<SrvYearlyPlanTableRegion> SrvYearlyPlanTableRegions { get; set; }
        public virtual DbSet<SrvYearlyPlanTableDistrict> SrvYearlyPlanTableDistricts { get; set; }
        public virtual DbSet<SrvYearlyPlanFile> SrvYearlyPlanFiles { get; set; }
        #endregion

        #region ENUM
        public virtual DbSet<ServicePriceType> ServicePriceType { get; set; }
        #endregion

        #region TRANSLATES
        public virtual DbSet<ServicePriceTypeTranslate> ServicePriceTypeTranslate { get; set; }
        #endregion
    }
}
