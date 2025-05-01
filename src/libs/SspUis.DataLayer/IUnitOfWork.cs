using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.EntityFrameworkCore.Storage;
using SspUis.DataLayer.EfClasses;
using SspUis.DataLayer.EfCode;
using SspUis.DataLayer.Repositories;
using SspUis.DataLayer.Repositories.Appeal;
using SspUis.DataLayer.Repositories.Claim;
using SspUis.DataLayer.Repositories.Corruption;
using SspUis.DataLayer.Repositories.Hrm;
using SspUis.DataLayer.Repositories.Kpi;
using SspUis.DataLayer.Repositories.Memship;

namespace SspUis.DataLayer
{
    public interface IUnitOfWork
    {
        EfCoreContext Context { get; }
        //SspUis.DataLayer.EfCode.EfCoreContext EdocContext { get; }
        IDbContextTransaction CurrentTransaction { get; }
        TRepository GetRepository<TRepository>();

        #region Repositories
        IMemshipNewContractorRepository MemshipNewContractorRepository { get; }
        IContractorRatingRepository ContractorRatingRepository { get; }
        IKpiGratingRepository KpiGratingRepository { get; }
        IKpiPlanForEmployeeRepository KpiPlanForEmployeeRepository { get; }
        IDocumentChangeLogRepository DocumentChangeLogRepository { get; }
        ICountryRepository CountryRepository { get; }
        IPersonRepository PersonRepository { get; }
        IAccountRepository AccountRepository { get; }
        IBankRepository BankRepository { get; }
        ICitizenshipRepository CitizenshipRepository { get; }
        IDistrictRepository DistrictRepository { get; }
        IMfyRepository MfyRepository { get; }
        IEmployeeRepository EmployeeRepository { get; }
        INationalityRepository NationalityRepository { get; }
        IOkedRepository OkedRepository { get; }
        ILandingPageDatumRepository LandingPageDatumRepository { get; }
        IOrganizationRepository OrganizationRepository { get; }
        IRegionRepository RegionRepository { get; }
        IRoleRepository RoleRepository { get; }
        IUserRepository UserRepository { get; }
        IDocumentHistoryRepository DocumentHistoryRepository { get; }
        IContractorRepository ContractorRepository { get; }
        IVideoLessonRepository VideoLessonRepository { get; }
        IOrganizationLegalFormRepository OrganizationLegalFormRepository { get; }
        IVideoCategoryRepository VideoCategoryRepository { get; }
        IBusinessmanAccountRepository MyAccountRepository { get; }
        INewsRepository NewsRepository { get; }
        ITagRepository TagRepository { get; }
        INewsTagRepository NewsTagRepository { get; }
        ISignHistoryRepository SignHistoryRepository { get; }
        IPositionRepository PositionRepository { get; }
        IApplicationRepository ApplicationRepository { get; }
        IPrtnContractRepository PrtnContractRepository { get; }
        IPrtnContractTypeRepository PrtnContractTypeRepository { get; }
        IPrtnCertificateRepository PrtnCertificateRepository { get; }
        INotBudgetContractorRepository NotBudgetContractorRepository { get; }
        IPrtnRejectReasonRepository PrtnRejectReasonRepository { get; }
        IDepartmentRepository DepartmentRepository { get; }
        IRelativeDegreeRepository RelativeDegreeRepository { get; }
        IIdentityDocumentRepository IdentityDocumentRepository { get; }
        IWorkScheduleRepository WorkScheduleRepository { get; }
        IAppointEmployeeRepository AppointEmployeeRepository { get; }
        ITimesheetRepository TimesheetRepository { get; }
        IApiRequestLogRepository ApiRequestLogRepository { get; }
        IEmployeeLeaveOrderRepository EmployeeLeaveOrderRepository { get; }
        ITempCalcKindRepository TempCalcKindRepository { get; }
        IRecallLeaveRepository RecallLeaveRepository { get; }
        IWorkDayOffRepository WorkDayOffRepository { get; }
        IEmployeeSickLeaveRepository EmployeeSickLeaveRepository { get; }
        IOrderToSendBusinessTripRepository OrderToSendBusinessTripRepository { get; }
        IPlannedCalculationRepository PlannedCalculationRepository { get; }
        ITaxBenefitTypeRepository TaxBenefitTypeRepository { get; }
        ICalculationKindRepository CalculationKindRepository { get; }
        IEmployeeManageRepository EmployeeManageRepository { get; }
        ITaxBenefitRepository TaxBenefitRepository { get; }
        IMassPlannedCalculationRepository MassPlannedCalculationRepository { get; }
        IItemOfExpenseRepository ItemOfExpenseRepository { get; }
        IProposalRepository ProposalRepository { get; }
        IPositionCategoryRepository PositionCategoryRepository { get; }
        IMonoApplicationRepository MonoApplicationRepository { get; }
        IStaffingTemplateRepository StaffingTemplateRepository { get; }
        IStaffingRepository StaffingRepository { get; }
        IStaffingIndicatorRepository StaffingIndicatorRepository { get; }
        IPositionTypeRepository PositionTypeRepository { get; }
        IEmployeeSendTrainRepository EmployeeSendTrainRepository { get; }
        ITariffScaleRepository TariffScaleRepository { get; }
        IStaffTypeBasicTariffRepository StaffTypeBasicTariffRepository { get; }
        IOrganizationalStructureRepository OrganizationalStructureRepository { get; }
        ITariffScaleCoefRepository TariffScaleCoefRepository { get; }
        ISourceCodeRepository SourceCodeRepository { get; }
        ILevelCodeRepository LevelCodeRepository { get; }
        ISettlementAccountSourceRepository SettlementAccountSourceRepository { get; }
        IFixedMinimumValueRepository FixedMinimumValueRepository { get; }
        IContractorActivityTypeRepository ContractorActivityTypeRepository { get; }
        INeedChamberServiceRepository NeedChamberServiceRepository { get; }
        IMemshipContractRepository MemshipContractRepository { get; }
        IContractorActivityGroupRepository ContractorActivityGroupRepository { get; }
        IQualificationCategoryRepository QualificationCategoryRepository { get; }
        IMemshipCertificateRepository MemshipCertificateRepository { get; }
        IPositionClassificationRepository PositionClassificationRepository { get; }
        IClaimThemeRepository ClaimThemeRepository { get; }
        IContractorContactRepository ContractorContactRepository { get; }
        IContractorSettlementAccountRepository ContractorSettlementAccountRepository { get; }
        IClaimOrganizationRepository ClaimOrganizationRepository { get; }
        IClaimOrganizationTypeRepository ClaimOrganizationTypeRepository { get; }
        IMediationRepository MediationRepository { get; }
        IClaimApplicationRepository ClaimApplicationRepository { get; }
        IMediationPlanRepository MediationPlanRepository { get; }
        IContractorSurveyRepository ContractorSurveyRepository { get; }
        IQuestionnaireRepository QuestionnaireRepository { get; }
        IQuestionGroupRepository QuestionGroupRepository { get; }
        IQuestionRepository QuestionRepository { get; }
        IAnswerRepository AnswerRepository { get; }
        ICurrencyRepository CurrencyRepository { get; }
        IJoinAntiCorruptionResultRepository JoinAntiCorruptionResultRepository { get; }
        IApplicationForCourtRepository ApplicationForCourtRepository { get; }
        IContractorUnionActivityTypeRepository ContractorUnionActivityTypeRepository { get; }
        IBusinessActivityTypeRepository BusinessActivityTypeRepository { get; }
        ICustomJobRepository CustomJobRepository { get; }
        IPrtnCreditDemandRepository PrtnCreditDemandRepository { get; }
        IDualEducationTypeRepository DualEducationTypeRepository { get; }
        IInstituteRepository InstituteRepository { get; }
        IInstituteBillingRepository InstituteBillingRepository { get; }
        ISpecialtyRepository SpecialtyRepository { get; }
        ISpecialtyBillingRepository SpecialtyBillingRepository { get; }
        ISendSmsConfigRepository SendSmsConfigRepository { get; }
        IMemshipPaymentOrderRepository MemshipPaymentOrderRepository { get; }
        IContractorCategoryCriterionRepository ContractorCategoryCriterionRepository { get; }
        IJoinAntiCorruptionCertificateRepository JoinAntiCorruptionCertificateRepository { get; }
        IMemshipApplicationRepository MemshipApplicationRepository { get; }
        IMemshipYearlyPlanRepository MemshipYearlyPlanRepository { get; }
        ISignCriterionRepository SignCriterionRepository { get; }
        IAcademicDegreeRepository AcademicDegreeRepository { get; }
        IDegreeTitleRepository DegreeTitleRepository { get; }
        IElectionMemberRepository ElectionMemberRepository { get; }
        ILanguageProficiencyRepository LanguageProficiencyRepository { get; }
        IScientificDegreeRepository ScientificDegreeRepository { get; }
        IStateAwardRepository StateAwardRepository { get; }
        IPartisanshipRepository PartisanshipRepository { get; }
        IMilitaryRankRepository MilitaryRankRepository { get; }
        IAdditionalAgreementRepository AdditionalAgreementRepository { get; }
        IServiceApplicationRepository ServiceApplicationRepository { get; }
        IServicePriceRepository ServicePriceRepository { get; }
        IServiceContractRepository ServiceContractRepository { get; }
        IEducationItemRepository EducationItemRepository { get; }
        IArbitrationJudgeRepository ArbitrationJudgeRepository { get; }
        IChastisementRepository ChastisementRepository { get; }
        ICandidatesConfirmationRepository CandidatesConfirmationRepository { get; }
        ISubsidyRequestRepository SubsidyRequestRepository { get; }
        IAppealApplicationRepository AppealApplicationRepository { get; }
        IAppealDescriptionRepository AppealDescriptionRepository { get; }
        IAppealTypeArriveRepository AppealTypeArriveRepository { get; }
        ISrvYearlyPlanRepository SrvYearlyPlanRepository { get; }
        ISrvApplicationYearlyPlanRepository SrvApplicationYearlyPlanRepository { get; }
        IArbitrationCourtApplicationRepository ArbitrationCourtApplicationRepository { get; }
	    IArbitrationDiscussionRepository ArbitrationDiscussionRepository { get; }
        IServiceDeedRepository ServiceDeedRepository { get; }
        ICallCenterAppealRepository CallCenterAppealRepository { get; }
        IArbitrationDelayRepository ArbitrationDelayRepository { get; }
        IIndicatorRepository IndicatorRepository { get; }
        IKpiRatingEmployeeRepository KpiRatingEmployeeRepository { get; }
        IEmployeeSendStudyRepository EmployeeSendStudyRepository { get; }
        IIndicatorDepartmentRepository IndicatorDepartmentRepository { get; }
        IEmployeeMissedDayRepository EmployeeMissedDayRepository { get; }
        IDualApplicationRepository DualApplicationRepository { get; }
        IExternalDocFromEdocRepository ExternalDocFromEdocRepository { get; }
        IPersonLogRepository PersonLogRepository { get; }
        IExecutionApplicationRepository ExecutionApplicationRepository { get; }
        ICourtntegrationRepository CourtntegrationRepository { get; }
		#endregion

		IDbContextTransaction BeginTransaction();
        void Save();
        void Commit();
        void Rollback();
        //IDbContextTransaction EdocBeginTransaction();
        //void EdocSave();
        //void EdocCommit();
        //void EdocRollback();
    }
}
