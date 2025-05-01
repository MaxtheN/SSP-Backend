using Microsoft.EntityFrameworkCore;
using SspUis.DataLayer.EfClasses;
using SspUis.DataLayer.EfClasses.Claim;
using SspUis.DataLayer.EfClasses.Exapidata.BankCredit;
using SspUis.DataLayer.EfClasses.Exapidata.Boj;
using SspUis.DataLayer.EfClasses.Exapidata.Finance;
using SspUis.DataLayer.EfClasses.Exapidata.Fund;
using SspUis.DataLayer.EfClasses.Exapidata.Investment;
using SspUis.DataLayer.EfClasses.Exapidata.Tax;
using WEBASE.EF;

namespace SspUis.DataLayer.EfCode;

public partial class EfCoreContext : BaseDbContext
{
    #region DOC
    public virtual DbSet<MediationPlan> MediationPlans { get; set; }
    public virtual DbSet<Mediation> Mediations { get; set; }
    public virtual DbSet<MediationFile> MediationFiles { get; set; }
    public virtual DbSet<ApplicationForCourt> ApplicationForCourts { get; set; }
    public virtual DbSet<ApplicationForCourtFile> ApplicationForCourtFiles { get; set; }
    #endregion

    #region ENUM
    public virtual DbSet<ClaimApplicationType> ClaimApplicationTypes { get; set; }
    public virtual DbSet<ClaimResponsibleType> ClaimResponsibleTypes { get; set; }
    public virtual DbSet<MediationType> MediationTypes { get; set; }
    public virtual DbSet<MediationResult> MediationResults { get; set; }
    public virtual DbSet<ClaimNeedCourt> ClaimNeedCourts { get; set; }
    #endregion

    #region INFO
    public virtual DbSet<ClaimTheme> ClaimThemes { get; set; }
    public virtual DbSet<ClaimOrganizationType> ClaimOrganizationTypes { get; set; }
    public virtual DbSet<ClaimOrganization> ClaimOrganizations { get; set; }
    public virtual DbSet<ArbitrationJudge> ArbitrationJudges { get; set; }
    #endregion

    #region Exapidata

    public virtual DbSet<EmployeeCount> EmployeeCounts { get; set; }
    public virtual DbSet<FinBenefit> FinBenefits { get; set; }
    public virtual DbSet<EfClasses.Exapidata.Tax.Debt> Debts{ get; set; }
    public virtual DbSet<GTDByInn> GTDByInns { get; set; }
    public virtual DbSet<GTDByInnGood> GTDByInnGoods { get; set; }
    public virtual DbSet<FundTadbirkor> FundTadbirkors { get; set; }
    public virtual DbSet<FundTadbirkorCredit> FundTadbirkorCredits { get; set; }
    public virtual DbSet<ImtiyozData> ImtiyozDatas { get; set; }
    public virtual DbSet<FarmerRefund> FarmerRefunds { get; set; }
    public virtual DbSet<BankCreditApplication> BankCreditApplications { get; set; }
    public virtual DbSet<BankCreditApplicationTable> BankCreditApplicationTables { get; set; }
    public virtual DbSet<ContractorAylanma> ContractorAylanmas { get; set; }
    public virtual DbSet<InvestmentByInn> InvestmentByInns { get; set; }
    public virtual DbSet<FinPaymentData> FinPaymentDatas { get; set; }
    public virtual DbSet<QqsAylanma> QqsAylanmas { get; set; }
    public virtual DbSet<AosAylanma> AosAylanmas { get; set; }
    #endregion

    #region Translates
    public virtual DbSet<ClaimApplicationTypeTranslate> ClaimApplicationTypeTranslates { get; set; }
    public virtual DbSet<ClaimResponsibleTypeTranslate> ClaimResponsibleTypeTranslates { get; set; }
    public virtual DbSet<MediationTypeTranslate> MediationTypeTranslates { get; set; }
    public virtual DbSet<MediationResultTranslate> MediationResultTranslates { get; set; }
    public virtual DbSet<ClaimNeedCourtTranslate> ClaimNeedCourtTranslates { get; set; }
    public virtual DbSet<ClaimThemeTranslate> ClaimThemesTranslates { get; set; }
    public virtual DbSet<ClaimOrganizationTypeTranslate> ClaimOrganizationTypeTranslates { get; set; }
    public virtual DbSet<ClaimOrganizationTranslate> ClaimOrganizationTranslates { get; set; }
    #endregion
    
    #region SudIntegration
    public virtual DbSet<Courtntegration> Courtntegrations { get; set; }
    #endregion

}