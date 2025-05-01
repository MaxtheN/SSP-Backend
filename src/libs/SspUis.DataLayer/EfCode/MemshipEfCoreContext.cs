using Microsoft.EntityFrameworkCore;
using SspUis.DataLayer.EfClasses;
using SspUis.DataLayer.EfClasses.Memship;
using System.Linq;

namespace SspUis.DataLayer.EfCode;

public partial class EfCoreContext
{
    #region ENUM
    public virtual DbSet<ContractorCategory> ContractorCategories { get; set; }
    public virtual DbSet<MemshipContractType> MemshipContractTypes { get; set; }
    public virtual DbSet<Rating> Ratings { get; set; }
    public virtual DbSet<ContractorType> ContractorTypes { get; set; }
    #endregion

    #region INFO
    public virtual DbSet<ContractorActivityGroup> ContractorActivityGroups { get; set; }
    public virtual DbSet<ContractorActivityType> ContractorActivityTypes { get; set; }
    public virtual DbSet<NeedChamberService> NeedChamberServices { get; set; }
    public virtual DbSet<ContractorRating> ContractorRatings { get; set; }

    #endregion

    #region TRANSLATES
    public virtual DbSet<ContractorCategoryTranslate> ContractorCategoryTranslates { get; set; }
    public virtual DbSet<MemshipContractTypeTranslate> MemshipContractTypeTranslates { get; set; }
    public virtual DbSet<ContractorActivityGroupTranslate> ContractorActivityGroupTranslates { get; set; }
    public virtual DbSet<ContractorActivityTypeTranslate> ContractorActivityTypeTranslates { get; set; }
    public virtual DbSet<NeedChamberServiceTranslate> NeedChamberServiceTranslates { get; set; }
    public virtual DbSet<ContractorRating> ContractorRatingTranslates { get; set; }
    #endregion

    #region DOC
    public virtual DbSet<MemshipContract> MemshipContracts { get; set; }
    public virtual DbSet<MemshipApplication> MemshipApplications { get; set; }
    public virtual DbSet<MemshipContractSign> MemshipContractSigns { get; set; }
    public virtual DbSet<MemshipCertificate> MemshipCertificates { get; set; }
    public virtual DbSet<MemshipPaymentOrder> MemshipPaymentOrders { get; set; }
    public virtual DbSet<ContractorCategoryCriterion> ContractorCategoryCriterions { get; set; }
    public virtual DbSet<MemshipYearlyPlan> MemshipYearlyPlans { get; set; }
    public virtual DbSet<MemshipYearlyPlanTable> MemshipYearlyPlanTables { get; set; }
    public virtual DbSet<MemshipYearlyPlanFile> MemshipYearlyPlanFiles { get; set; }
    public virtual DbSet<Debt> MemshipDebts { get; set; }
    public virtual DbSet<DebtTable> MemshipDebtTables { get; set; }
    public virtual DbSet<MemshipNewContractor> MemshipNewContractors { get; set; }
    public virtual DbSet<MemshipNewContractorsFile> MemshipNewContractorsFiles { get; set; }
    public virtual DbSet<MemshipNewContractorsTable> MemshipNewContractorsTables { get; set; }
    #endregion

    #region Functions
    [DbFunction("get_memship_main_report", Schema = "memship")]
    public IQueryable<MemshipReportFuncDto>
          GetMemshipMainReport(
              int? pr_region_id,
              int? pr_district_id,
              bool pr_by_region,
              bool pr_by_district,
              int? pr_contractor_category_id,
              int? pr_language_id
           ) =>
          FromExpression(() => GetMemshipMainReport(pr_region_id, pr_district_id, pr_by_region, pr_by_district, pr_contractor_category_id, pr_language_id));
    #endregion
}
