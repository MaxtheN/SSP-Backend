using Microsoft.EntityFrameworkCore;

using SspUis.DataLayer.EfClasses;
using SspUis.DataLayer.EfClasses.Hrm;
using WEBASE.EF;

namespace SspUis.DataLayer.EfCode;

public partial class EfCoreContext : BaseDbContext
{
    #region HL
    public virtual DbSet<Department> Departments { get; set; }
    public virtual DbSet<Employee> Employees { get; set; }
    public virtual DbSet<EmployeePlaceOfWork> EmployeePlaceOfWorks { get; set; }
    public virtual DbSet<EmployeeRelative> EmployeeRelatives { get; set; }
    public virtual DbSet<EmployeeHigherEdu> EmployeeHigherEdus { get; set; }
    public virtual DbSet<EmployeeAcademicDegree> EmployeeAcademicDegrees { get; set; }
    public virtual DbSet<EmployeeDegreeTitle> EmployeeDegreeTitles { get; set; }
    public virtual DbSet<EmployeeElectionMember> EmployeeElectionMembers { get; set; }
    public virtual DbSet<EmployeeLanguageProficiency> EmployeeLanguageProficiencys { get; set; }
    public virtual DbSet<EmployeeScientificDegree> EmployeeScientificDegrees { get; set; }
    public virtual DbSet<EmployeeStateAward> EmployeeStateAwards { get; set; }
    public virtual DbSet<EmployeePartisanship> EmployeePartisanships { get; set; }
    public virtual DbSet<EmployeeMilitaryRank> EmployeeMilitaryRanks { get; set; }
    #endregion

    #region DOC
    public virtual DbSet<HrmSignerVtView> HrmSignerVtViews { get; set; }
    public virtual DbSet<HrmEmpVtView> HrmEmpVtViews { get; set; }
    public virtual DbSet<AppointEmployee> AppointEmployees { get; set; }
    public virtual DbSet<StateAssetApplication> StateAssetApplications { get; set; }
    public virtual DbSet<AppointEmployeeTable> AppointEmployeeTables { get; set; }
    public virtual DbSet<AppointEmployeeSigner> AppointEmployeeSigner { get; set; }
    public virtual DbSet<EmployeeLeaveOrder> EmployeeLeaveOrders { get; set; }
    public virtual DbSet<EmployeeLeaveOrderTable> EmployeeLeaveOrderTables { get; set; }
    public virtual DbSet<EmployeeSendTrain> EmployeeSendTrains { get; set; }
    public virtual DbSet<EmployeeSendTrainTable> EmployeeSendTrainTables { get; set; }
    public virtual DbSet<RecallLeave> RecallLeaves { get; set; }
    public virtual DbSet<RecallLeaveTable> RecallLeaveTables { get; set; }
    public virtual DbSet<TempCalcKind> TempCalcKinds { get; set; }
    public virtual DbSet<TempCalcKindTable> TempCalcKindTables { get; set; }
    public virtual DbSet<Staffing> Staffings { get; set; }
    public virtual DbSet<StaffingCalcKind> StaffingCalcKinds { get; set; }
    public virtual DbSet<StaffingPosition> StaffingPositions { get; set; }
    public virtual DbSet<StaffingIndicatorValue> StaffingIndicatorValues { get; set; }
    public virtual DbSet<Timesheet> Timesheets { get; set; }
    public virtual DbSet<TimesheetTable> TimesheetTables { get; set; }
    public virtual DbSet<TimesheetTableDay> TimesheetTableDays { get; set; }
    public virtual DbSet<WorkDayOff> WorkDayOffs { get; set; }
    public virtual DbSet<WorkDayOffTable> WorkDayOffTables { get; set; }
    public virtual DbSet<EmployeeSickLeave> EmployeeSickLeaves { get; set; }
    public virtual DbSet<EmployeeSickLeaveTable> EmployeeSickLeaveTables { get; set; }
    public virtual DbSet<OrderToSendBusinessTrip> OrderToSendBusinessTrips { get; set; }
    public virtual DbSet<OrderToSendBusinessTripTable> OrderToSendBusinessTripTables { get; set; }
    public virtual DbSet<PlannedCalculation> PlannedCalculations { get; set; }
    public virtual DbSet<PlannedCalculationTable> PlannedCalculationTables { get; set; }
    public virtual DbSet<TaxBenefit> TaxBenefits { get; set; }
    public virtual DbSet<Chastisement> Chastisements { get; set; }
    public virtual DbSet<ChastisementTable> ChastisementTables { get; set; }
    public virtual DbSet<ChastisementSigner> ChastisementSigners { get; set; }
    public virtual DbSet<MassPlannedCalculation> MassPlannedCalculations { get; set; }
    public virtual DbSet<CandidatesConfirmation> CandidatesConfirmations { get; set; }
    public virtual DbSet<CandidatesConfirmationTable> CandidatesConfirmationTables { get; set; }
    public virtual DbSet<EmployeeSendStudy> EmployeeSendStudys { get; set; }
    public virtual DbSet<EmployeeSendStudyTable> EmployeeSendStudyTables { get; set; }
    public virtual DbSet<EmployeeSendStudySigner> EmployeeSendStudySigners { get; set; }
    public virtual DbSet<EmployeeMissedDay> EmployeeMissedDays { get; set; }
    public virtual DbSet<EmployeeMissedDayTable> EmployeeMissedDayTables { get; set; }
    #endregion

    #region ENUM
    public virtual DbSet<TimesheetType> TimesheetTypes { get; set; }
    public virtual DbSet<WorkScheduleKind> WorkScheduleKinds { get; set; }
    public virtual DbSet<RoundingType> RoundingTypes { get; set; }
    public virtual DbSet<MinimumValueType> MinimumValueTypes { get; set; }
    public virtual DbSet<MinimumValueTypeTranslate> MinimumValueTypeTranslates { get; set; }
    public virtual DbSet<CalculateByTimeType> CalculateByTimeTypes { get; set; }
    public virtual DbSet<CalculateByTimeTypeTranslate> CalculateByTimeTypeTranslates { get; set; }
    public virtual DbSet<CalculationMethod> CalculationMethods { get; set; }
    public virtual DbSet<CalculationMethodTranslate> CalculationMethodTranslates { get; set; }
    public virtual DbSet<CalculationType> CalculationTypes { get; set; }
    public virtual DbSet<CalculationTypeTranslate> CalculationTypeTranslates { get; set; }
    public virtual DbSet<TariffScaleType> TariffScaleTypes { get; set; }
    public virtual DbSet<PositionPeriod> PositionPeriods { get; set; }
    public virtual DbSet<PositionPeriodTranslate> PositionPeriodTranslates { get; set; }
    public virtual DbSet<EmployeeTurnstileLogType> EmployeeTurnstileLogTypes { get; set; }
    public virtual DbSet<EmployeeSickLeaveType> EmployeeSickLeaveTypes { get; set; }
    public virtual DbSet<EmployeeSickLeaveTypeTranslate> EmployeeSickLeaveTypeTranslates { get; set; }
    public virtual DbSet<MissedDaysType> MissedDaysTypes { get; set; }
    public virtual DbSet<MissedDaysTypeTranslate> MissedDaysTypeTranslates { get; set; }
    #endregion

    #region INFO
    public virtual DbSet<CallCenter> CallCenters { get; set; }
    public virtual DbSet<TaxBenefitType> TaxBenefitTypes { get; set; }
    public virtual DbSet<TaxBenefitTypeTranslate> TaxBenefitTypeTranslates { get; set; }
    public virtual DbSet<WorkSchedule> WorkSchedules { get; set; }
    public virtual DbSet<WorkScheduleDayHour> WorkScheduleDayHours { get; set; }
    public virtual DbSet<WorkScheduleWorkHour> WorkScheduleWorkHours { get; set; }
    public virtual DbSet<CalculationKind> CalculationKinds { get; set; }
    public virtual DbSet<ItemOfExpense> ItemOfExpenses { get; set; }
    public virtual DbSet<PositionCategory> PositionCategories { get; set; }
    public virtual DbSet<PositionClassification> PositionClassifications { get; set; }
    public virtual DbSet<PositionType> PositionTypes { get; set; }
    public virtual DbSet<TariffScale> TariffScales { get; set; }
    public virtual DbSet<TariffScaleTable> TariffScaleTables { get; set; }
    public virtual DbSet<StaffTypeBasicTariff> StaffTypeBasicTariffs { get; set; }
    public virtual DbSet<TariffScaleCoef> TariffScaleCoefs { get; set; }
    public virtual DbSet<TariffScaleCoefTable> TariffScaleCoefTables { get; set; }
    public virtual DbSet<SourceCode> SourceCodes { get; set; }
    public virtual DbSet<LevelCode> LevelCodes { get; set; }
    public virtual DbSet<FixedMinimumValue> FixedMinimumValues { get; set; }
    public virtual DbSet<QualificationCategory> QualificationCategorys { get; set; }
    public virtual DbSet<StaffingIndicator> StaffingIndicators { get; set; }
    public virtual DbSet<AcademicDegree> AcademicDegrees { get; set; }
    public virtual DbSet<DegreeTitle> DegreeTitles { get; set; }
    public virtual DbSet<ElectionMember> ElectionMembers { get; set; }
    public virtual DbSet<LanguageProficiency> LanguageProficiencys { get; set; }
    public virtual DbSet<ScientificDegree> ScientificDegrees { get; set; }
    public virtual DbSet<StateAward> StateAwards { get; set; }
    public virtual DbSet<Partisanship> Partisanships { get; set; }
    public virtual DbSet<MilitaryRank> MilitaryRanks { get; set; }
    public virtual DbSet<EmployeeTurnstileLog> EmployeeTurnstileLogs { get; set; }
    #endregion

    #region Translates
    public virtual DbSet<WorkScheduleKindTranslate> WorkScheduleKindTranslates { get; set; }
    public virtual DbSet<WorkScheduleTranslate> WorkScheduleTranslates { get; set; }
    public virtual DbSet<TimesheetTypeTranslate> TimesheetTypeTranslate { get; set; }
    public virtual DbSet<RoundingTypeTranslate> RoundingTypeTranslate { get; set; }
    public virtual DbSet<CalculationKindTranslate> CalculationKindTranslate { get; set; }
    public virtual DbSet<ItemOfExpenseTranslate> ItemOfExpenseTranslate { get; set; }
    public virtual DbSet<PositionCategoryTranslate> PositionCategoryTranslates { get; set; }
    public virtual DbSet<PositionClassificationTranslate> PositionClassificationTranslates { get; set; }
    public virtual DbSet<PositionTypeTranslate> PositionTypeTranslates { get; set; }
    public virtual DbSet<TariffScaleTypeTranslate> TariffScaleTypeTranslates { get; set; }
    public virtual DbSet<TariffScaleTranslate> TariffScaleTranslates { get; set; }
    public virtual DbSet<StaffTypeBasicTariffTranslate> StaffTypeBasicTariffTranslates { get; set; }
    public virtual DbSet<TariffScaleCoefTranslate> TariffScaleCoefTranslates { get; set; }
    public virtual DbSet<SourceCodeTranslate> SourceCodeTranslates { get; set; }
    public virtual DbSet<LevelCodeTranslate> LevelCodeTranslates { get; set; }
    public virtual DbSet<QualificationCategoryTranslate> QualificationCategoryTranslates { get; set; }
    public virtual DbSet<StaffingIndicatorTranslate> StaffingIndicatorTranslates { get; set; }
    public virtual DbSet<AcademicDegreeTranslate> AcademicDegreeTranslates { get; set; }
    public virtual DbSet<DegreeTitleTranslate> DegreeTitleTranslates { get; set; }
    public virtual DbSet<ElectionMemberTranslate> ElectionMemberTranslates { get; set; }
    public virtual DbSet<LanguageProficiencyTranslate> LanguageProficiencyTranslates { get; set; }
    public virtual DbSet<ScientificDegreeTranslate> ScientificDegreeTranslates { get; set; }
    public virtual DbSet<StateAwardsTranslate> StateAwardsTranslates { get; set; }
    public virtual DbSet<PartisanshipTranslate> PartisanshipTranslates { get; set; }
    public virtual DbSet<MilitaryRankTranslate> MilitaryRankTranslates { get; set; }
    public virtual DbSet<EmployeeTurnstileLogTypeTranslate> EmployeeTurnstileLogTypeTranslates { get; set; }
    #endregion

    #region SYS
    public virtual DbSet<EmployeeManage> EmployeeManages { get; set; }
    #endregion
}
