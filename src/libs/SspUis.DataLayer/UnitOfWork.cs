using EasyCaching.Core.Diagnostics;
using GenericServices;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.Extensions.DependencyInjection;
using SspUis.DataLayer.EfCode;
using SspUis.DataLayer.Repositories;
using SspUis.DataLayer.Repositories.Appeal;
using SspUis.DataLayer.Repositories.Claim;
using SspUis.DataLayer.Repositories.Corruption;
using SspUis.DataLayer.Repositories.Hrm;
using SspUis.DataLayer.Repositories.Kpi;
using SspUis.DataLayer.Repositories.Memship;
using System;

namespace SspUis.DataLayer
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly IServiceProvider _serviceProvider;
        public UnitOfWork(ICrudServices crudServices, IServiceProvider serviceProvider)
        {
            Context = (EfCoreContext)crudServices.Context;
            //EdocContext = (SspUis.DataLayer.EfCode.EfCoreContext)crudServices.Context;
            CrudServices = crudServices;
            _serviceProvider = serviceProvider;
        }
        public ICrudServices CrudServices { get; }
        public EfCoreContext Context { get; }
        //public SspUis.DataLayer.EfCode.EfCoreContext EdocContext { get; }

        public IDbContextTransaction CurrentTransaction { get => Context.Database.CurrentTransaction; }
        public TRepository GetRepository<TRepository>() => _serviceProvider.GetRequiredService<TRepository>();

        #region Repositories
        public IMemshipNewContractorRepository MemshipNewContractorRepository { get => GetRepository<IMemshipNewContractorRepository>(); }
        public IContractorRatingRepository ContractorRatingRepository { get => GetRepository<IContractorRatingRepository>(); }
        public IKpiGratingRepository KpiGratingRepository { get => GetRepository<IKpiGratingRepository>(); }
        public IDocumentChangeLogRepository DocumentChangeLogRepository { get => GetRepository<IDocumentChangeLogRepository>(); }
        public ICountryRepository CountryRepository { get => GetRepository<ICountryRepository>(); }
        public IPersonRepository PersonRepository { get => GetRepository<IPersonRepository>(); }
        public IAccountRepository AccountRepository { get => GetRepository<IAccountRepository>(); }
        public IBankRepository BankRepository { get => GetRepository<IBankRepository>(); }
        public ICitizenshipRepository CitizenshipRepository { get => GetRepository<ICitizenshipRepository>(); }
        public IDistrictRepository DistrictRepository { get => GetRepository<IDistrictRepository>(); }
        public IMfyRepository MfyRepository { get => GetRepository<IMfyRepository>(); }
        public IEmployeeRepository EmployeeRepository { get => GetRepository<IEmployeeRepository>(); }
        public INationalityRepository NationalityRepository { get => GetRepository<INationalityRepository>(); }
        public IOkedRepository OkedRepository { get => GetRepository<IOkedRepository>(); }
        public ILandingPageDatumRepository LandingPageDatumRepository { get => GetRepository<ILandingPageDatumRepository>(); }
        public IOrganizationRepository OrganizationRepository { get => GetRepository<IOrganizationRepository>(); }
        public IRegionRepository RegionRepository { get => GetRepository<IRegionRepository>(); }
        public IRoleRepository RoleRepository { get => GetRepository<IRoleRepository>(); }
        public IUserRepository UserRepository { get => GetRepository<IUserRepository>(); }
        public IDocumentHistoryRepository DocumentHistoryRepository { get => GetRepository<IDocumentHistoryRepository>(); }
        public IContractorRepository ContractorRepository { get => GetRepository<IContractorRepository>(); }
        public IVideoLessonRepository VideoLessonRepository { get => GetRepository<IVideoLessonRepository>(); }
        public IOrganizationLegalFormRepository OrganizationLegalFormRepository { get => GetRepository<IOrganizationLegalFormRepository>(); }
        public IVideoCategoryRepository VideoCategoryRepository { get => GetRepository<IVideoCategoryRepository>(); }
        public IBusinessmanAccountRepository MyAccountRepository { get => GetRepository<IBusinessmanAccountRepository>(); }
        public INewsRepository NewsRepository { get => GetRepository<INewsRepository>(); }
        public ITagRepository TagRepository { get => GetRepository<ITagRepository>(); }
        public INewsTagRepository NewsTagRepository { get => GetRepository<INewsTagRepository>(); }
        public ISignHistoryRepository SignHistoryRepository { get => GetRepository<ISignHistoryRepository>(); }
        public IPositionRepository PositionRepository { get => GetRepository<IPositionRepository>(); }
        public IPrtnContractTypeRepository PrtnContractTypeRepository { get => GetRepository<IPrtnContractTypeRepository>(); }
        public IPrtnContractRepository PrtnContractRepository { get => GetRepository<IPrtnContractRepository>(); }
        public IApplicationRepository ApplicationRepository { get => GetRepository<IApplicationRepository>(); }
        public IPrtnCertificateRepository PrtnCertificateRepository { get => GetRepository<IPrtnCertificateRepository>(); }
        public INotBudgetContractorRepository NotBudgetContractorRepository { get => GetRepository<INotBudgetContractorRepository>(); }
        public IPrtnRejectReasonRepository PrtnRejectReasonRepository { get => GetRepository<IPrtnRejectReasonRepository>(); }
        public IDepartmentRepository DepartmentRepository { get => GetRepository<IDepartmentRepository>(); }
        public IRelativeDegreeRepository RelativeDegreeRepository { get => GetRepository<IRelativeDegreeRepository>(); }
        public IIdentityDocumentRepository IdentityDocumentRepository { get => GetRepository<IIdentityDocumentRepository>(); }
        public IWorkScheduleRepository WorkScheduleRepository { get => GetRepository<IWorkScheduleRepository>(); }
        public IAppointEmployeeRepository AppointEmployeeRepository { get => GetRepository<IAppointEmployeeRepository>(); }
        public ITimesheetRepository TimesheetRepository { get => GetRepository<ITimesheetRepository>(); }
        public IApiRequestLogRepository ApiRequestLogRepository { get => GetRepository<IApiRequestLogRepository>(); }
        public IEmployeeLeaveOrderRepository EmployeeLeaveOrderRepository { get => GetRepository<IEmployeeLeaveOrderRepository>(); }
        public ITempCalcKindRepository TempCalcKindRepository { get => GetRepository<ITempCalcKindRepository>(); }
        public IRecallLeaveRepository RecallLeaveRepository { get => GetRepository<IRecallLeaveRepository>(); }
        public IWorkDayOffRepository WorkDayOffRepository { get => GetRepository<IWorkDayOffRepository>(); }
        public IEmployeeSickLeaveRepository EmployeeSickLeaveRepository { get => GetRepository<IEmployeeSickLeaveRepository>(); }
        public IOrderToSendBusinessTripRepository OrderToSendBusinessTripRepository { get => GetRepository<IOrderToSendBusinessTripRepository>(); }
        public IPlannedCalculationRepository PlannedCalculationRepository { get => GetRepository<IPlannedCalculationRepository>(); }
        public ITaxBenefitTypeRepository TaxBenefitTypeRepository { get => GetRepository<ITaxBenefitTypeRepository>(); }
        public ICalculationKindRepository CalculationKindRepository { get => GetRepository<ICalculationKindRepository>(); }
        public IEmployeeManageRepository EmployeeManageRepository { get => GetRepository<IEmployeeManageRepository>(); }
        public ITaxBenefitRepository TaxBenefitRepository { get => GetRepository<ITaxBenefitRepository>(); }
        public IMassPlannedCalculationRepository MassPlannedCalculationRepository { get => GetRepository<IMassPlannedCalculationRepository>(); }
        public IItemOfExpenseRepository ItemOfExpenseRepository { get => GetRepository<IItemOfExpenseRepository>(); }
        public IProposalRepository ProposalRepository { get => GetRepository<IProposalRepository>(); }
        public IPositionCategoryRepository PositionCategoryRepository { get => GetRepository<IPositionCategoryRepository>(); }
        public IPositionTypeRepository PositionTypeRepository { get => GetRepository<IPositionTypeRepository>(); }
        public IEmployeeSendTrainRepository EmployeeSendTrainRepository { get => GetRepository<IEmployeeSendTrainRepository>(); }
        public IStaffingTemplateRepository StaffingTemplateRepository { get => GetRepository<IStaffingTemplateRepository>(); }
        public IStaffingRepository StaffingRepository { get => GetRepository<IStaffingRepository>(); }
        public IStaffingIndicatorRepository StaffingIndicatorRepository { get => GetRepository<IStaffingIndicatorRepository>(); }
        public ITariffScaleRepository TariffScaleRepository { get => GetRepository<ITariffScaleRepository>(); }
        public IStaffTypeBasicTariffRepository StaffTypeBasicTariffRepository { get => GetRepository<IStaffTypeBasicTariffRepository>(); }
        public IOrganizationalStructureRepository OrganizationalStructureRepository { get => GetRepository<IOrganizationalStructureRepository>(); }
        public ITariffScaleCoefRepository TariffScaleCoefRepository { get => GetRepository<ITariffScaleCoefRepository>(); }
        public ISourceCodeRepository SourceCodeRepository { get => GetRepository<ISourceCodeRepository>(); }
        public ILevelCodeRepository LevelCodeRepository { get => GetRepository<ILevelCodeRepository>(); }
        public ISettlementAccountSourceRepository SettlementAccountSourceRepository { get => GetRepository<ISettlementAccountSourceRepository>(); }
        public IFixedMinimumValueRepository FixedMinimumValueRepository { get => GetRepository<IFixedMinimumValueRepository>(); }
        public IContractorActivityTypeRepository ContractorActivityTypeRepository { get => GetRepository<IContractorActivityTypeRepository>(); }
        public INeedChamberServiceRepository NeedChamberServiceRepository { get => GetRepository<INeedChamberServiceRepository>(); }
        public IMemshipContractRepository MemshipContractRepository { get => GetRepository<IMemshipContractRepository>(); }
        public IContractorActivityGroupRepository ContractorActivityGroupRepository { get => GetRepository<IContractorActivityGroupRepository>(); }
        public IQualificationCategoryRepository QualificationCategoryRepository { get => GetRepository<IQualificationCategoryRepository>(); }
        public IMemshipCertificateRepository MemshipCertificateRepository { get => GetRepository<IMemshipCertificateRepository>(); }
        public IPositionClassificationRepository PositionClassificationRepository { get => GetRepository<IPositionClassificationRepository>(); }
        public IClaimThemeRepository ClaimThemeRepository { get => GetRepository<IClaimThemeRepository>(); }
        public IClaimOrganizationRepository ClaimOrganizationRepository { get => GetRepository<IClaimOrganizationRepository>(); }
        public IClaimOrganizationTypeRepository ClaimOrganizationTypeRepository { get => GetRepository<IClaimOrganizationTypeRepository>(); }
        public IMediationRepository MediationRepository { get => GetRepository<IMediationRepository>(); }
        public IClaimApplicationRepository ClaimApplicationRepository { get => GetRepository<IClaimApplicationRepository>(); }
        public IMediationPlanRepository MediationPlanRepository { get => GetRepository<IMediationPlanRepository>(); }
        public IContractorSurveyRepository ContractorSurveyRepository { get => GetRepository<IContractorSurveyRepository>(); }
        public IQuestionnaireRepository QuestionnaireRepository { get => GetRepository<IQuestionnaireRepository>(); }
        public IQuestionGroupRepository QuestionGroupRepository { get => GetRepository<IQuestionGroupRepository>(); }
        public IQuestionRepository QuestionRepository { get => GetRepository<IQuestionRepository>(); }
        public IAnswerRepository AnswerRepository { get => GetRepository<IAnswerRepository>(); }
        public ICurrencyRepository CurrencyRepository { get => GetRepository<ICurrencyRepository>(); }
        public IJoinAntiCorruptionResultRepository JoinAntiCorruptionResultRepository { get => GetRepository<IJoinAntiCorruptionResultRepository>(); }
        public IApplicationForCourtRepository ApplicationForCourtRepository { get => GetRepository<IApplicationForCourtRepository>(); }
        public IContractorUnionActivityTypeRepository ContractorUnionActivityTypeRepository { get => GetRepository<IContractorUnionActivityTypeRepository>(); }
        public IBusinessActivityTypeRepository BusinessActivityTypeRepository { get => GetRepository<IBusinessActivityTypeRepository>(); }
        public ICustomJobRepository CustomJobRepository { get => GetRepository<ICustomJobRepository>(); }
        public IPrtnCreditDemandRepository PrtnCreditDemandRepository { get => GetRepository<IPrtnCreditDemandRepository>(); }
        public IDualEducationTypeRepository DualEducationTypeRepository { get => GetRepository<IDualEducationTypeRepository>(); }
        public IInstituteRepository InstituteRepository { get => GetRepository<IInstituteRepository>(); }
        public IInstituteBillingRepository InstituteBillingRepository { get => GetRepository<IInstituteBillingRepository>(); }
        public ISpecialtyRepository SpecialtyRepository { get => GetRepository<ISpecialtyRepository>(); }
        public ISpecialtyBillingRepository SpecialtyBillingRepository { get => GetRepository<ISpecialtyBillingRepository>(); }
        public IMemshipPaymentOrderRepository MemshipPaymentOrderRepository { get => GetRepository<IMemshipPaymentOrderRepository>(); }
        public IContractorCategoryCriterionRepository ContractorCategoryCriterionRepository { get => GetRepository<IContractorCategoryCriterionRepository>(); }
        public IJoinAntiCorruptionCertificateRepository JoinAntiCorruptionCertificateRepository { get => GetRepository<IJoinAntiCorruptionCertificateRepository>(); }
        public ISendSmsConfigRepository SendSmsConfigRepository { get => GetRepository<ISendSmsConfigRepository>(); }
        public IMemshipApplicationRepository MemshipApplicationRepository { get => GetRepository<IMemshipApplicationRepository>(); }
        public IMemshipYearlyPlanRepository MemshipYearlyPlanRepository { get => GetRepository<IMemshipYearlyPlanRepository>(); }
        public ISignCriterionRepository SignCriterionRepository { get => GetRepository<ISignCriterionRepository>(); }
        public IAcademicDegreeRepository AcademicDegreeRepository { get => GetRepository<IAcademicDegreeRepository>(); }
        public IDegreeTitleRepository DegreeTitleRepository { get => GetRepository<IDegreeTitleRepository>(); }
        public IElectionMemberRepository ElectionMemberRepository { get => GetRepository<IElectionMemberRepository>(); }
        public ILanguageProficiencyRepository LanguageProficiencyRepository { get => GetRepository<ILanguageProficiencyRepository>(); }
        public IScientificDegreeRepository ScientificDegreeRepository { get => GetRepository<IScientificDegreeRepository>(); }
        public IStateAwardRepository StateAwardRepository { get => GetRepository<IStateAwardRepository>(); }
        public IPartisanshipRepository PartisanshipRepository { get => GetRepository<IPartisanshipRepository>(); }
        public IMilitaryRankRepository MilitaryRankRepository { get => GetRepository<IMilitaryRankRepository>(); }
        public IAdditionalAgreementRepository AdditionalAgreementRepository { get => GetRepository<IAdditionalAgreementRepository>(); }
        public IServiceApplicationRepository ServiceApplicationRepository { get => GetRepository<IServiceApplicationRepository>(); }
        public IServicePriceRepository ServicePriceRepository { get => GetRepository<IServicePriceRepository>(); }
        public IServiceContractRepository ServiceContractRepository { get => GetRepository<IServiceContractRepository>(); }
        public IEducationItemRepository EducationItemRepository { get => GetRepository<IEducationItemRepository>(); }
        public IArbitrationJudgeRepository ArbitrationJudgeRepository { get => GetRepository<IArbitrationJudgeRepository>(); }
        public IChastisementRepository ChastisementRepository { get => GetRepository<IChastisementRepository>(); }
        public ICandidatesConfirmationRepository CandidatesConfirmationRepository { get => GetRepository<ICandidatesConfirmationRepository>(); }
        public ISubsidyRequestRepository SubsidyRequestRepository { get => GetRepository<ISubsidyRequestRepository>(); }
        public IAppealApplicationRepository AppealApplicationRepository { get => GetRepository<IAppealApplicationRepository>(); }
        public IAppealDescriptionRepository AppealDescriptionRepository { get => GetRepository<IAppealDescriptionRepository>(); }
        public IAppealTypeArriveRepository AppealTypeArriveRepository { get => GetRepository<IAppealTypeArriveRepository>(); }
        public ISrvYearlyPlanRepository SrvYearlyPlanRepository { get => GetRepository<ISrvYearlyPlanRepository>(); }
        public ISrvApplicationYearlyPlanRepository SrvApplicationYearlyPlanRepository { get => GetRepository<ISrvApplicationYearlyPlanRepository>(); }
        public IArbitrationCourtApplicationRepository ArbitrationCourtApplicationRepository { get => GetRepository<IArbitrationCourtApplicationRepository>(); }
        public IArbitrationDiscussionRepository ArbitrationDiscussionRepository { get => GetRepository<IArbitrationDiscussionRepository>(); }
        public IServiceDeedRepository ServiceDeedRepository { get => GetRepository<IServiceDeedRepository>(); }
        public ICallCenterAppealRepository CallCenterAppealRepository { get => GetRepository<ICallCenterAppealRepository>(); }
        public IArbitrationDelayRepository ArbitrationDelayRepository { get => GetRepository<IArbitrationDelayRepository>(); }
        public IIndicatorRepository IndicatorRepository { get => GetRepository<IIndicatorRepository>(); }

        public IKpiPlanForEmployeeRepository KpiPlanForEmployeeRepository { get => GetRepository<IKpiPlanForEmployeeRepository>(); }
        public IKpiRatingEmployeeRepository KpiRatingEmployeeRepository { get => GetRepository<IKpiRatingEmployeeRepository>(); }
        public IEmployeeSendStudyRepository EmployeeSendStudyRepository { get => GetRepository<IEmployeeSendStudyRepository>(); }
        public IIndicatorDepartmentRepository IndicatorDepartmentRepository { get => GetRepository<IIndicatorDepartmentRepository>(); }
        public IEmployeeMissedDayRepository EmployeeMissedDayRepository { get => GetRepository<IEmployeeMissedDayRepository>(); }
        public IDualApplicationRepository DualApplicationRepository { get => GetRepository<IDualApplicationRepository>(); }
        public IExternalDocFromEdocRepository ExternalDocFromEdocRepository { get => GetRepository<IExternalDocFromEdocRepository>(); }
        public IPersonLogRepository PersonLogRepository { get => GetRepository<IPersonLogRepository>(); }
        public IExecutionApplicationRepository ExecutionApplicationRepository { get => GetRepository<IExecutionApplicationRepository>(); }

        public ICourtntegrationRepository CourtntegrationRepository { get => GetRepository<ICourtntegrationRepository>(); }

        public IContractorContactRepository ContractorContactRepository { get => GetRepository<IContractorContactRepository>(); }

        public IMonoApplicationRepository MonoApplicationRepository { get => GetRepository<IMonoApplicationRepository>(); }

        public IContractorSettlementAccountRepository ContractorSettlementAccountRepository { get => GetRepository<IContractorSettlementAccountRepository>(); }


        #endregion

        public IDbContextTransaction BeginTransaction()
        {
            return Context.Database.BeginTransaction();
        }
        public void Save()
        {
            Context.SaveChanges();
        }
        public void Commit()
        {
            Save();
            if (Context.Database.CurrentTransaction != null)
                Context.Database.CurrentTransaction.Commit();
        }
        public void Rollback()
        {
            if (Context.Database.CurrentTransaction != null)
                Context.Database.CurrentTransaction.Rollback();
        }

        //public IDbContextTransaction EdocBeginTransaction()
        //{
        //    return EdocContext.Database.BeginTransaction();
        //}
        //public void EdocSave()
        //{
        //    EdocContext.SaveChanges();
        //}
        //public void EdocCommit()
        //{
        //    EdocSave();
        //    if (EdocContext.Database.CurrentTransaction != null)
        //        EdocContext.Database.CurrentTransaction.Commit();
        //}
        //public void EdocRollback()
        //{
        //    if (EdocContext.Database.CurrentTransaction != null)
        //        EdocContext.Database.CurrentTransaction.Rollback();
        //}
    }
}
