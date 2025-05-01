using SspUis.DataLayer.EfClasses;
using Microsoft.EntityFrameworkCore;
using WEBASE.EF;
using SspUis.DataLayer.EfClasses.Public.Sys;
using SspUis.DataLayer.EfClasses.Public.Hl;

namespace SspUis.DataLayer.EfCode
{
    public partial class EfCoreContext : BaseDbContext
    {
        public EfCoreContext(DbContextOptions options)
            : base(options)
        {
            Config.AutoSetProperties.DateOfCreated.PropertyName = nameof(User.CreatedAt);
            Config.AutoSetProperties.DateOfModified.PropertyName = nameof(User.ModifiedAt);
        }
        public virtual DbSet<SoliqImtiyoz> SoliqImtiyozs { get; set; }
        public virtual DbSet<Language> Languages { get; set; }
        public virtual DbSet<MfyApplicationLog> MfyApplicationLogs { get; set; }
        public virtual DbSet<Mfy> Mfies { get; set; }
        public virtual DbSet<State> States { get; set; }
        public virtual DbSet<Country> Countries { get; set; }
        public virtual DbSet<Table> Tables { get; set; }
        public virtual DbSet<Gender> Genders { get; set; }
        public virtual DbSet<NotificationType> NotificationTypes { get; set; }
        public virtual DbSet<Status> Statuses { get; set; }
        public virtual DbSet<MonoApplicationBandlikStatus> MonoApplicationIntegrationStatuses { get; set; }
        public virtual DbSet<Person> People { get; set; }
        public virtual DbSet<PersonLog> PersonLogs { get; set; }
        public virtual DbSet<Bank> Banks { get; set; }
        public virtual DbSet<Citizenship> Citizenships { get; set; }
        public virtual DbSet<District> Districts { get; set; }
        public virtual DbSet<Nationality> Nationalities { get; set; }
        public virtual DbSet<Oked> Okeds { get; set; }
        public virtual DbSet<Organization> Organizations { get; set; }
        public virtual DbSet<Region> Regions { get; set; }
        public virtual DbSet<Module> Modules { get; set; }
        public virtual DbSet<ModuleGroup> ModuleGroups { get; set; }
        public virtual DbSet<ModuleSubGroup> ModuleSubGroups { get; set; }
        public virtual DbSet<Role> Roles { get; set; }
        public virtual DbSet<RoleModule> RoleModules { get; set; }
        public virtual DbSet<User> Users { get; set; }
        public virtual DbSet<UserDeviceLog> UserDeviceLogs { get; set; }
        public virtual DbSet<UserLog> UserLogs { get; set; }
        public virtual DbSet<UserRole> UserRoles { get; set; }
        public virtual DbSet<Position> Positions { get; set; }
        public virtual DbSet<SignHistory> SignHistories { get; set; }
        public virtual DbSet<DocumentHistory> DocumentHistories { get; set; }
        public virtual DbSet<DocumentDirection> DocumentDirections { get; set; }
        public virtual DbSet<DocumentJobHistory> DocumentJobHistories { get; set; }
        public virtual DbSet<JobHistory> JobHistories { get; set; }
        public virtual DbSet<VideoLesson> VideoLessons { get; set; }
        public virtual DbSet<OrganizationLegalForm> OrganizationLegalForms { get; set; }
        public virtual DbSet<LandingPageDatum> LandingPageDatums { get; set; }
        public virtual DbSet<NumberTemplate> NumberTemplates { get; set; }
        public virtual DbSet<Number> Numbers { get; set; }
        public virtual DbSet<Accessibility> Accessibility { get; set; }
        public virtual DbSet<AccessServiceHistory> AccessServiceHistories { get; set; }
        public virtual DbSet<AccessModule> AccessModules { get; set; }
        public virtual DbSet<RelativeDegree> RelativeDegrees { get; set; }
        public virtual DbSet<IdentityDocument> IdentityDocuments { get; set; }
        public virtual DbSet<ApiRequestLog> ApiRequestLogs { get; set; }
        public virtual DbSet<AppError> AppErrors { get; set; }
        public virtual DbSet<CompanyType> CompanyTypes { get; set; }
        public virtual DbSet<SettlementAccountSource> SettlementAccountSources { get; set; }
        public virtual DbSet<Currency> Currencys { get; set; }
        public virtual DbSet<BusinessSectorCategory> BusinessSectorCategories { get; set; }
        public virtual DbSet<BusinessActivityType> BusinessActivityTypes { get; set; }
        public virtual DbSet<CustomJob> CustomJobs { get; set; }
        public virtual DbSet<CustomJobAction> CustomJobActions { get; set; }
        public virtual DbSet<MeetingType> MeetingTypes { get; set; }
        public virtual DbSet<BankCode> BankCodes { get; set; }
        public virtual DbSet<DocumentChat> DocumentChats { get; set; }
        public virtual DbSet<Offer> Offers { get; set; }
        public virtual DbSet<ContractorOffers> ContractorOffers { get; set; }
        public virtual DbSet<DocumentTempleteFile> DucumentTempleteFiles { get; set; }
        public virtual DbSet<ApplicationModelCode> ApplicationModelCodes { get; set; }
        public virtual DbSet<RestrictionOfSendingApplication> RestrictionOfSendingApplications { get; set; }
        public virtual DbSet<SignCriterion> SignCriterions { get; set; }
        public virtual DbSet<RegionPrefix> RegionPrefixes { get; set; }
        public virtual DbSet<DistrictPrefix> DistrictPrefixes { get; set; }
        public virtual DbSet<LanguageDegree> LanguageDegrees { get; set; }
        public virtual DbSet<AdditionalAgreement> AdditionalAgreements { get; set; }
        public virtual DbSet<EducationItem> EducationItems { get; set; }
        public virtual DbSet<CallCenterAppeal> CallCenterAppeals { get; set; }

        #region NOTIFY agar maqul bo'lmasa o'zgartiriladi
        public virtual DbSet<SendSmsConfig> SendSmsConfigs { get; set; }
        public virtual DbSet<SendSmsLog> SendSmsLogs { get; set; }
        #endregion

        #region Files
        public virtual DbSet<CallCenterAppealFile> CallCenterAppealFiles { get; set; }
        #endregion

        #region Translates

        public virtual DbSet<StateTranslate> StateTranslates { get; set; }
        public virtual DbSet<CountryTranslate> CountryTranslates { get; set; }
        public virtual DbSet<GenderTranslate> GenderTranslates { get; set; }
        public virtual DbSet<NotificationTypeTranslate> NotificationTypeTranslates { get; set; }
        public virtual DbSet<StatusTranslate> StatusTranslates { get; set; }
        public virtual DbSet<BankTranslate> BankTranslates { get; set; }
        public virtual DbSet<CitizenshipTranslate> CitizenshipTranslates { get; set; }
        public virtual DbSet<DistrictTranslate> DistrictTranslates { get; set; }
        public virtual DbSet<NationalityTranslate> NationalityTranslates { get; set; }
        public virtual DbSet<OkedTranslate> OkedTranslates { get; set; }
        public virtual DbSet<OrganizationTranslate> OrganizationTranslates { get; set; }
        public virtual DbSet<RegionTranslate> RegionTranslates { get; set; }
        public virtual DbSet<ModuleGroupTranslate> ModuleGroupTranslates { get; set; }
        public virtual DbSet<ModuleSubGroupTranslate> ModuleSubGroupTranslates { get; set; }
        public virtual DbSet<ModuleTranslate> ModuleTranslates { get; set; }
        public virtual DbSet<RoleTranslate> RoleTranslates { get; set; }
        public virtual DbSet<PositionTranslate> PositionTranslates { get; set; }
        public virtual DbSet<NewsTranslate> NewsTranslatess { get; set; }
        public virtual DbSet<DocumentChangeLog> DocumentChangeLogs { get; set; }
        public virtual DbSet<RelativeDegreeTranslate> RelativeDegreeTranslates { get; set; }
        public virtual DbSet<IdentityDocumentTranslate> IdentityDocumentTranslates { get; set; }
        public virtual DbSet<SettlementAccountSourceTranslate> SettlementAccountSourceTranslates { get; set; }
        public virtual DbSet<CurrencyTranslate> CurrencyTranslates { get; set; }
        public virtual DbSet<MeetingTypeTranslate> MeetingTypeTranslates { get; set; }
        public virtual DbSet<BankCodeTranslate> BankCodeTranslates { get; set; }
        public virtual DbSet<LanguageDegreeTranslate> LanguageDegreeTranslates { get; set; }
        public virtual DbSet<EducationItemTranslate> EducationItemTranslates { get; set; }
        #endregion

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            MyOnModelCreating(modelBuilder);
        }
    }
}
