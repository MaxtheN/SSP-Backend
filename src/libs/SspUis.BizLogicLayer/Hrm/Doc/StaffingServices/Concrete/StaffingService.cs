using WEBASE;
using WEBASE.Models;
using Microsoft.EntityFrameworkCore;
using iText.Kernel.Pdf;
using iText.Kernel.Geom;
using iText.Html2pdf;
using System.Web;
using Aspose.Cells;
using WEBASE.Storage;
using Microsoft.AspNetCore.Http;
using OfficeOpenXml;
using WEBASE.i18n;
using Microsoft.AspNetCore.Mvc;
using WEBASE.Integration.MSPD.Sud;
using WEBASE.Utility;
using SspUis.DataLayer.EfClasses.Hrm;
using StatusGeneric;
using SspUis.DataLayer.Repositories.Hrm;
using SspUis.DataLayer;
using SspUis.Core.Security;
using SspUis.BizLogicLayer.DocumentChangeLogServices;
using SspUis.BizLogicLayer.Hrm.CalculationKindServices;
using SspUis.ServiceLayer.NumberServices;
using System;
using System.Collections.Generic;
using SspUis.BizLogicLayer.Hrm.StaffingIndicatorServices;
using System.Linq;
using SspUis.Core;
using SspUis.DataLayer.EfClasses;
using SspUis.BizLayer.Hrm.StaffingTemplateServices;
using WEBASE.Salary.Core.Model;
using WEBASE.Salary.Core;
using System.IO;

namespace SspUis.BizLogicLayer.Hrm.StaffingServices
{
    public class StaffingService : StatusGenericHandler, IStaffingService
    {
        private readonly IStaffingRepository _repository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IAuthService _authService;
        private readonly IDocumentChangeLogService _documentChangeLogService;
        private readonly ICalculationKindService _calculationKindService;
        private readonly INumberService _numberService;
        private readonly IStorageService _storageService;
        private readonly ICultureHelper _cultureHelper;

        public StaffingService(IUnitOfWork unitOfWork,
            IAuthService authService,
            IDocumentChangeLogService documentChangeLogService,
            ICalculationKindService calculationKindService,
            INumberService numberService,
            IStorageService storageService,
            ICultureHelper cultureHelper)
        {
            _repository = unitOfWork.StaffingRepository;
            _unitOfWork = unitOfWork;
            _authService = authService;
            _documentChangeLogService = documentChangeLogService;
            _calculationKindService = calculationKindService;
            _numberService = numberService;
            _storageService = storageService;
            _cultureHelper = cultureHelper;
        }
        public PagedResult<StaffingListDto> GetList(StaffingSortFilterPageOptions dto)
        {
            var result = _repository.ReadAsNoTracked<StaffingListDto>()
                                    .SortFilter(dto)
                                    .AsPagedResult(dto);
            return result;
        }
        public PagedResult<StaffingListDto> GetListForHeader(StaffingSortFilterPageOptions dto)
        {
            var result = _repository.ReadAsNoTracked<StaffingListDto>(applyFilter: false).Where(a => new int[]
            {
                        StatusIdConst.SENT,     StatusIdConst.REVOKED,
                        StatusIdConst.RECEIVED, StatusIdConst.REJECTED,
                        StatusIdConst.ACCEPTED
            }.Contains(a.StatusId))
                                    .SortFilter(dto)
                                    .AsPagedResult(dto);
            return result;
        }
        public PagedResult<StaffingListDto> GetListForOwnOrg(StaffingSortFilterPageOptions dto)
        {
            var result = _repository.ReadAsNoTracked<StaffingListDto>().Where(a => new int[]
            {
                        StatusIdConst.SENT,     StatusIdConst.REVOKED,
                        StatusIdConst.RECEIVED, StatusIdConst.REJECTED,
                        StatusIdConst.ACCEPTED
            }.Contains(a.StatusId))
                                    .SortFilter(dto)
                                    .AsPagedResult(dto);
            return result;
        }
        public StaffingDto Get()
        {
            var result = new StaffingDto()
            {
                DocOn = DateTime.Today.AsDateOnly(),
                ForMonths = 12,
                DocNumber = _numberService.GetNext(nameof(TableIdConst.HRM__DOC_STAFFING), organizationId: _authService.Organization.Id).Item2,
                TableId = TableIdConst.HRM__DOC_STAFFING,
            };

            return result;
        }
        public List<StaffingIndicatorValueDto> _FillIndicator(CreateStaffingDlDto dto)
        {
            var indicatorValues = new List<StaffingIndicatorValueDto>();
            var defaultStaffingIndicators = _repository.CrudServices
                        .ProjectFromEntityToDto<StaffingIndicator, StaffingIndicatorDto>(
                                query => query.Where(a => a.StateId == StateIdConst.ACTIVE && new string[] { StaffingIndicatorCodeConst._018 }.Contains(a.Code)))
                        .ToList();

            indicatorValues.AddRange(defaultStaffingIndicators.Select(a => new StaffingIndicatorValueDto
            {
                StaffingIndicatorId = a.Id,
                StaffingIndicatorName = a.ShortName,
                StaffingIndicatorCode = a.Code,
                Quantity = dto.Positions.Sum(x => x.Quantity),
                TotalSum = dto.Positions.Sum(x => x.Fot)
            }));

            var totalSum = indicatorValues.Sum(a => a.TotalSum);

            if (_authService.Organization.OrganizationalStructureId.HasValue)
            {
                var organizationalStructure = _repository.Context.Set<OrganizationalStructure>()
                    .FirstOrDefault(a => a.Id == _authService.Organization.OrganizationalStructureId);

                _repository.Context.Entry(organizationalStructure)
                    .Collection(a => a.StructureStaffingIndicator)
                    .Query()
                    .Include(a => a.StaffingIndicator).ThenInclude(a => a.Translates)
                    .Load();

                if (organizationalStructure != null && organizationalStructure.StructureStaffingIndicator.Any())
                {
                    indicatorValues.AddRange(organizationalStructure.StructureStaffingIndicator.Select(a => new StaffingIndicatorValueDto
                    {
                        StaffingIndicatorId = a.StaffingIndicatorId,
                        StaffingIndicatorCode = a.StaffingIndicator.Code,
                        StaffingIndicatorName = a.StaffingIndicator.Translates.AsQueryable().FirstOrDefault(StaffingIndicatorTranslate.GetExpr(TranslateColumn.full_name, _cultureHelper.CurrentCulture.Id))?.TranslateText ?? a.StaffingIndicator.FullName,
                        Quantity = 0,
                        TotalSum = totalSum * a.Percentage / 100,
                        CanEdit = true
                    }));
                }
            }

            return indicatorValues;
        }
        public IEnumerable<StaffingIndicatorValueDto> FillIndicator(CreateStaffingDlDto dto)
        {
            var indicatorValues = new List<FillStaffingIndicatorValueDto>();
            var indicators = new List<OrganizationalStructureStaffingIndicator>();

            if (_authService.Organization.OrganizationalStructureId.HasValue)
            {
                var organizationalStructure = _repository.Context.Set<OrganizationalStructure>()
                    .FirstOrDefault(a => a.Id == _authService.Organization.OrganizationalStructureId);

                _repository.Context.Entry(organizationalStructure)
                                   .Collection(a => a.StructureStaffingIndicator)
                                   .Query()
                                   .Include(a => a.StaffingIndicator).ThenInclude(a => a.Translates)
                                   .Include(a => a.Tables)
                                   .Load();

                if (organizationalStructure != null && organizationalStructure.StructureStaffingIndicator.Any())
                {
                    indicatorValues.AddRange(organizationalStructure.StructureStaffingIndicator.OrderBy(a => a.CalcOrderCode).Select(a => new FillStaffingIndicatorValueDto
                    {
                        StaffingIndicatorId = a.StaffingIndicatorId,
                        StaffingIndicatorCode = a.StaffingIndicator.Code,
                        StaffingIndicatorName = a.StaffingIndicator.Translates.AsQueryable().FirstOrDefault(StaffingIndicatorTranslate.GetExpr(TranslateColumn.full_name, _cultureHelper.CurrentCulture.Id))?.TranslateText ?? a.StaffingIndicator.FullName,
                        Quantity = 0,
                        IsTotal = a.IsTotal,
                        IsCalculationKindTotal = a.IsCalculationKindTotal,
                        Tables = a.Tables.Select(a => a.StaffingIndicatorId).ToList(),
                        Percentage = a.Percentage,
                        DisplayOrderCode = a.DisplayOrderCode,
                        CalcOrderCode = a.CalcOrderCode
                    }));
                }

            }

            if (indicatorValues.Any())
            {
                foreach (var indicator in indicatorValues)
                {
                    // 55 id li ko'rsatkichga => hamma ko'rsatkichlar hisoblangandan keyin 'Жами меҳнатга ҳақ тўлаш жамғармаси йиллик' ni 1/12 qismi olinadi
                    if (indicator.StaffingIndicatorId == StaffingIndicatorIdConst._55)
                        continue;

                    if (indicator.IsTotal)
                    {
                        indicator.TotalSum = dto.Positions.Sum(x => x.Fot) ?? 0;
                        indicator.IsCalCulated = true;
                    }
                    else if (indicator.IsCalculationKindTotal)
                    {
                        indicator.TotalSum = dto.Positions.SelectMany(a => a.CalcKinds).Sum(a => a.CalcSum) ?? 0;
                        indicator.IsCalCulated = true;
                    }

                    // TODO: here hard-code calculations of the indicator totals

                    if (indicator.Tables.Any())
                        CalculateIndicatorTotalSum(indicatorValues, indicator);
                }

                var monthlySalary = indicatorValues.FirstOrDefault(a => a.StaffingIndicatorId == StaffingIndicatorIdConst._55);
                if (monthlySalary != null)
                {
                    var yearlySalary = indicatorValues.FirstOrDefault(a => a.StaffingIndicatorId == StaffingIndicatorIdConst._46);
                    if (yearlySalary != null)
                    {
                        monthlySalary.TotalSum = decimal.Round((yearlySalary.TotalSum / 12) ?? 0, 2);
                        monthlySalary.IsCalCulated = true;
                    }
                }
            }

            return indicatorValues.Where(a => a.DisplayOrderCode > 0).OrderBy(a => a.DisplayOrderCode);

            static void CalculateIndicatorTotalSum(List<FillStaffingIndicatorValueDto> indicatorValues, FillStaffingIndicatorValueDto indicator)
            {
                if (!indicator.IsCalCulated)
                {
                    foreach (var staffingIndicatorId in indicator.Tables)
                    {
                        var sourceIndicator = indicatorValues.FirstOrDefault(a => a.StaffingIndicatorId == staffingIndicatorId);

                        if (sourceIndicator.Tables.Any())
                        {
                            CalculateIndicatorTotalSum(indicatorValues, sourceIndicator);
                        }

                        if (sourceIndicator != null)
                            indicator.TotalSum += sourceIndicator.TotalSum;
                    }

                    indicator.TotalSum = indicator.TotalSum * indicator.Percentage / 100;
                    indicator.IsCalCulated = true;
                }
            }
        }
        public List<StaffingIndicatorValueDto> OldFillIndicator(CreateStaffingDlDto dto)
        {
            var indicatorValues = new List<StaffingIndicatorValueDto>();
            var allStaffingIndicators = _repository.CrudServices.ProjectFromEntityToDto<StaffingIndicator, StaffingIndicatorDto>(query => query.Where(a => a.StateId == StateIdConst.ACTIVE));

            decimal oneMonthSalaryFundSum = dto.Positions.Sum(x => x.Fot) ?? 0;
            decimal sicknessSheetSum = oneMonthSalaryFundSum * 12 * 0.01m;
            decimal additionalPremiumPaymentsSum = oneMonthSalaryFundSum * 12 * 0.02m;

            decimal laborCompensationFundSum = oneMonthSalaryFundSum * 12 * 0.98m;
            decimal financialSupportByOneMonthSalarySum = oneMonthSalaryFundSum;
            decimal rewardByTwoMonthSalarySum = oneMonthSalaryFundSum * 2;
            decimal specialFinancialIncentiveFundSum = (laborCompensationFundSum + financialSupportByOneMonthSalarySum + rewardByTwoMonthSalarySum) * 0.15m;
            decimal totalSalaryFundSum = sicknessSheetSum +
                                         additionalPremiumPaymentsSum +
                                         laborCompensationFundSum +
                                         financialSupportByOneMonthSalarySum +
                                         rewardByTwoMonthSalarySum +
                                         specialFinancialIncentiveFundSum;

            var totalSalaryFund = allStaffingIndicators.FirstOrDefault(a => a.Code == StaffingIndicatorCodeConst._018);
            if (totalSalaryFund != null)
            {
                indicatorValues.Add(new StaffingIndicatorValueDto
                {
                    StaffingIndicatorId = totalSalaryFund.Id,
                    StaffingIndicatorCode = totalSalaryFund.Code,
                    StaffingIndicatorName = totalSalaryFund.FullName,
                    TotalSum = totalSalaryFundSum,
                    //CanEdit = false
                });
            }

            var laborCompensationFund = allStaffingIndicators.FirstOrDefault(a => a.Code == StaffingIndicatorCodeConst._019);
            if (laborCompensationFund != null)
            {
                indicatorValues.Add(new StaffingIndicatorValueDto
                {
                    StaffingIndicatorId = laborCompensationFund.Id,
                    StaffingIndicatorCode = laborCompensationFund.Code,
                    StaffingIndicatorName = laborCompensationFund.FullName,
                    TotalSum = laborCompensationFundSum,
                });
            }

            var financialSupportByOneMonthSalary = allStaffingIndicators.FirstOrDefault(a => a.Code == StaffingIndicatorCodeConst._020);
            if (financialSupportByOneMonthSalary != null)
            {
                indicatorValues.Add(new StaffingIndicatorValueDto
                {
                    StaffingIndicatorId = financialSupportByOneMonthSalary.Id,
                    StaffingIndicatorCode = financialSupportByOneMonthSalary.Code,
                    StaffingIndicatorName = financialSupportByOneMonthSalary.FullName,
                    TotalSum = financialSupportByOneMonthSalarySum,
                });
            }

            var rewardByTwoMonthSalary = allStaffingIndicators.FirstOrDefault(a => a.Code == StaffingIndicatorCodeConst._034);
            if (rewardByTwoMonthSalary != null)
            {
                indicatorValues.Add(new StaffingIndicatorValueDto
                {
                    StaffingIndicatorId = rewardByTwoMonthSalary.Id,
                    StaffingIndicatorCode = rewardByTwoMonthSalary.Code,
                    StaffingIndicatorName = rewardByTwoMonthSalary.FullName,
                    TotalSum = rewardByTwoMonthSalarySum,
                });
            }

            var specialFinancialIncentiveFund = allStaffingIndicators.FirstOrDefault(a => a.Code == StaffingIndicatorCodeConst._022);
            if (specialFinancialIncentiveFund != null)
            {
                indicatorValues.Add(new StaffingIndicatorValueDto
                {
                    StaffingIndicatorId = specialFinancialIncentiveFund.Id,
                    StaffingIndicatorCode = specialFinancialIncentiveFund.Code,
                    StaffingIndicatorName = specialFinancialIncentiveFund.FullName,
                    TotalSum = specialFinancialIncentiveFundSum,
                });
            }

            var additionalPremiumPayments = allStaffingIndicators.FirstOrDefault(a => a.Code == StaffingIndicatorCodeConst._023);
            if (additionalPremiumPayments != null)
            {
                indicatorValues.Add(new StaffingIndicatorValueDto
                {
                    StaffingIndicatorId = additionalPremiumPayments.Id,
                    StaffingIndicatorCode = additionalPremiumPayments.Code,
                    StaffingIndicatorName = additionalPremiumPayments.FullName,
                    TotalSum = additionalPremiumPaymentsSum,
                });
            }

            var _028 = allStaffingIndicators.FirstOrDefault(a => a.Code == StaffingIndicatorCodeConst._028);
            if (_028 != null)
            {
                indicatorValues.Add(new StaffingIndicatorValueDto
                {
                    StaffingIndicatorId = _028.Id,
                    StaffingIndicatorCode = _028.Code,
                    StaffingIndicatorName = _028.FullName,
                    TotalSum = 0
                });
            }

            var _029 = allStaffingIndicators.FirstOrDefault(a => a.Code == StaffingIndicatorCodeConst._029);
            if (_029 != null)
            {
                indicatorValues.Add(new StaffingIndicatorValueDto
                {
                    StaffingIndicatorId = _029.Id,
                    StaffingIndicatorCode = _029.Code,
                    StaffingIndicatorName = _029.FullName,
                    TotalSum = 0
                });
            }

            var _030 = allStaffingIndicators.FirstOrDefault(a => a.Code == StaffingIndicatorCodeConst._030);
            if (_029 != null)
            {
                indicatorValues.Add(new StaffingIndicatorValueDto
                {
                    StaffingIndicatorId = _030.Id,
                    StaffingIndicatorCode = _030.Code,
                    StaffingIndicatorName = _030.FullName,
                    TotalSum = 0
                });
            }

            var _031 = allStaffingIndicators.FirstOrDefault(a => a.Code == StaffingIndicatorCodeConst._031);
            if (_031 != null)
            {
                indicatorValues.Add(new StaffingIndicatorValueDto
                {
                    StaffingIndicatorId = _031.Id,
                    StaffingIndicatorCode = _031.Code,
                    StaffingIndicatorName = _031.FullName,
                    TotalSum = 0
                });
            }

            var _026 = allStaffingIndicators.FirstOrDefault(a => a.Code == StaffingIndicatorCodeConst._026);
            if (_026 != null)
            {
                indicatorValues.Add(new StaffingIndicatorValueDto
                {
                    StaffingIndicatorId = _026.Id,
                    StaffingIndicatorCode = _026.Code,
                    StaffingIndicatorName = _026.FullName,
                    TotalSum = 0
                });
            }

            var _032 = allStaffingIndicators.FirstOrDefault(a => a.Code == StaffingIndicatorCodeConst._032);
            if (_032 != null)
            {
                indicatorValues.Add(new StaffingIndicatorValueDto
                {
                    StaffingIndicatorId = _032.Id,
                    StaffingIndicatorCode = _032.Code,
                    StaffingIndicatorName = _032.FullName,
                    TotalSum = 0
                });
            }

            var _035 = allStaffingIndicators.FirstOrDefault(a => a.Code == StaffingIndicatorCodeConst._035);
            if (_035 != null)
            {
                indicatorValues.Add(new StaffingIndicatorValueDto
                {
                    StaffingIndicatorId = _035.Id,
                    StaffingIndicatorCode = _035.Code,
                    StaffingIndicatorName = _035.FullName,
                    TotalSum = 0
                });
            }

            var _033 = allStaffingIndicators.FirstOrDefault(a => a.Code == StaffingIndicatorCodeConst._033);
            if (_033 != null)
            {
                indicatorValues.Add(new StaffingIndicatorValueDto
                {
                    StaffingIndicatorId = _033.Id,
                    StaffingIndicatorCode = _033.Code,
                    StaffingIndicatorName = _033.FullName,
                    TotalSum = 0
                });
            }
            var sicknessSheet = allStaffingIndicators.FirstOrDefault(a => a.Code == StaffingIndicatorCodeConst._024);
            if (sicknessSheet != null)
            {
                indicatorValues.Add(new StaffingIndicatorValueDto
                {
                    StaffingIndicatorId = sicknessSheet.Id,
                    StaffingIndicatorCode = sicknessSheet.Code,
                    StaffingIndicatorName = sicknessSheet.FullName,
                    TotalSum = sicknessSheetSum,
                });
            }

            var oneMonthSalaryFund = allStaffingIndicators.FirstOrDefault(a => a.Code == StaffingIndicatorCodeConst._025);
            if (oneMonthSalaryFund != null)
            {
                indicatorValues.Add(new StaffingIndicatorValueDto
                {
                    StaffingIndicatorId = oneMonthSalaryFund.Id,
                    StaffingIndicatorCode = oneMonthSalaryFund.Code,
                    StaffingIndicatorName = oneMonthSalaryFund.FullName,
                    TotalSum = oneMonthSalaryFundSum,
                });
            }

            return indicatorValues;
        }
        public StaffingDto Get(long id)
        {
            var dto = _repository.ById<StaffingDto>(id, applyFilter: false);

            CombineStatuses(_repository);
            //dto.CanModify = StatusIdConst.CanApplyStatus(dto.StatusId, StatusIdConst.MODIFIED) && _authService.HasPermission(PermissionCode.StaffingEdit);
            //dto.CanAccept = StatusIdConst.CanApplyStatus(dto.StatusId, StatusIdConst.ACCEPTED) && _authService.HasPermission(PermissionCode.StaffingAccept);
            //dto.CanCancel = StatusIdConst.CanApplyStatus(dto.StatusId, StatusIdConst.NOT_ACCEPTED) && _authService.HasPermission(PermissionCode.StaffingCancel);
            //dto.CanDelete = StatusIdConst.CanApplyStatus(dto.StatusId, StatusIdConst.DELETED) && _authService.HasPermission(PermissionCode.StaffingDelete);
            if (IsValid)
            {
                foreach (var item in dto.Positions)
                {
                    var tariffScaleCoefTable = _repository.Context.Set<TariffScaleCoefTable>().FirstOrDefault(a => a.RankCode == item.RankCode);

                    if (tariffScaleCoefTable != null)
                        item.RankCoef = tariffScaleCoefTable.Coef;
                }
                dto.IndicatorValues.ForEach(indicator =>
                {
                    if (indicator.StaffingIndicatorId == 1
                      || indicator.StaffingIndicatorId == 2
                      || indicator.StaffingIndicatorId == 3
                      || indicator.StaffingIndicatorId == 4
                      || indicator.StaffingIndicatorId == 5
                      || indicator.StaffingIndicatorId == 6
                      || indicator.StaffingIndicatorId == 8
                      || indicator.StaffingIndicatorId == 16
                      ) indicator.CanEdit = false;
                    else indicator.CanEdit = true;
                });
            }
            dto.Positions = dto.Positions.OrderBy(a => a.DepartmentCode).ThenBy(a => a.PositionOrderCode).ToList();
            return dto;
        }
        public StaffingDto GetClone(long id)
        {
            var dto = _repository.ById<StaffingDto>(id);
            if (HasErrors)
                return null;
            dto.Id = 0;
            dto.DocOn = DateTime.Today.AsDateOnly();
            dto.ForMonths = 12;
            dto.DocNumber = _numberService.GetNext(nameof(TableIdConst.HRM__DOC_STAFFING), organizationId: _authService.Organization.Id).Item2;
            foreach (var item in dto.Positions)
            {
                item.Id = 0;
            }
            return dto;
        }
        public IEnumerable<StaffingPositionDto> GetAllStaffingPositions(DateTime? date = null, int? positionId = null, int? positionClassificationId = null, int? departmentId = null, int? organizationId = null)
        {
            if (!date.HasValue)
                date = DateTime.Today;

            var staffing = _repository.CrudServices.ProjectFromEntityToDto<StaffingPosition, StaffingPositionDto>
                (
                    query => query
                        .Where(a => new int[]
                        {
                        StatusIdConst.RECEIVED
                        }.Contains(a.Owner.StatusId) && (organizationId.HasValue
                                                     ? a.Owner.OrganizationId == organizationId.Value
                                                     : a.Owner.OrganizationId == _authService.Organization.Id) &&
                                    (date.HasValue ? a.Owner.StartOn <= DateOnly.FromDateTime(date.Value) : true) &&
                                    (positionId.HasValue && positionId != 0 ? a.PositionId == positionId : true) &&
                                    //(positionClassificationId.HasValue && positionClassificationId != 0 ? a.PositionClassificationId == positionClassificationId : true) &&
                                    (departmentId.HasValue ? a.DepartmentId == departmentId : true))
                );
            return staffing.Any() ? staffing : new List<StaffingPositionDto>();
        }
        public IEnumerable<StaffingDtoForChamber> GetPositionsForChamber(int? langId)
        {
            var staffingPositions = _unitOfWork.Context.Set<StaffingPosition>()
                .Include(a => a.Owner)
                .Include(d => d.Department)
                    .ThenInclude(dt => dt.Translates)
                .Include(p => p.Position)
                    .ThenInclude(pt => pt.Translates)
                .Where(a => a.Department.Code == 1 &&
                            a.Owner.OrganizationId == OrganizationIdConst.SSP &&
                            a.Owner.StatusId == StatusIdConst.RECEIVED)
                .AsEnumerable() // Switch to LINQ to Objects
                .Where(a => a.Owner.StartOn <= DateOnly.FromDateTime(DateTime.Now)) // Perform date comparison in memory
                .GroupBy(a => a.Department.Translates
                    .AsQueryable()
                    .FirstOrDefault(DepartmentTranslate.GetExpr(DataLayer.TranslateColumn.full_name, langId ?? 3))
                    ?.TranslateText ?? a.Department.FullName)
                .Select(g => new StaffingDtoForChamber
                {
                    Department = g.Key,
                    Positions = g.Select(a => a.Position.Translates
                        .AsQueryable()
                        .FirstOrDefault(PositionTranslate.GetExpr(DataLayer.TranslateColumn.full_name, langId ?? 3))
                        ?.TranslateText ?? a.Position.FullName)
                        .ToList()
                })
                .ToList();

            return staffingPositions;
        }

        public StaffingPostionForNow GetAllStaffingPositionsForQuantity(int positionId, int departmentId, int? organizationId)
        {
            var staffingPositions = _unitOfWork.Context.Set<StaffingPosition>()
               .FirstOrDefault(a => a.Owner.StatusId == StatusIdConst.RECEIVED && (organizationId.HasValue
                                                     ? a.Owner.OrganizationId == organizationId.Value
                                                     : a.Owner.OrganizationId == _authService.Organization.Id) && a.PositionId == positionId && a.DepartmentId == departmentId).Quantity;
            var quantity = new StaffingPostionForNow();

            quantity = new StaffingPostionForNow
            {
                QuantityForNow = staffingPositions - _unitOfWork.Context.Set<EmployeeManage>()
                                    .Where(b => b.OrganizationId == _authService.User.OrganizationId && b.EndOn == null && !b.IsDeleted && b.PositionId == positionId && b.DepartmentId == departmentId)
                                    .Sum(a => a.EmploymentRate.Value)
            };
            return quantity;
        }
        public IEnumerable<PositionDto> GetAllStaffingPositionClassifications(DateTime? date = null, int? positionClassificationId = null, int? departmentId = null)
        {
            if (!date.HasValue)
                date = DateTime.Today;

            var positionClassification = _unitOfWork.Context.Set<StaffingPosition>()
                .Where(a => a.Owner.StatusId == StatusIdConst.ACCEPTED &&
                    a.Owner.OrganizationId == _authService.Organization.Id &&
                    (date.HasValue ? a.Owner.StartOn <= DateOnly.FromDateTime(date.Value) : true) &&
                    (positionClassificationId.HasValue && positionClassificationId != 0 ? a.PositionClassificationId == positionClassificationId : true) && (departmentId.HasValue ? a.DepartmentId == departmentId : true))
                .Select(a => a.PositionClassificationId)
                .Distinct()
                .ToList();

            var positions = _repository.CrudServices.ProjectFromEntityToDto<Position, PositionDto>
            (
                query => query.Where(a => a.StateId == StateIdConst.ACTIVE && positionClassification.Contains(a.PositionClassificationId))
            );

            return positions.Any() ? positions : new List<PositionDto>();
        }
        public IEnumerable<PositionDto> GetAllStaffingPositions(DateTime? date = null, int? departmentId = null)
        {
            if (!date.HasValue)
                date = DateTime.Today;

            var positionIds = _unitOfWork.Context.Set<StaffingPosition>()
                .Where(a => a.Owner.StatusId == StatusIdConst.ACCEPTED &&
                    a.Owner.OrganizationId == _authService.Organization.Id &&
                    (date.HasValue ? a.Owner.StartOn <= DateOnly.FromDateTime(date.Value) : true) &&
                    (departmentId.HasValue ? a.DepartmentId == departmentId : true))
                .Select(a => a.PositionId)
                .Distinct()
                .ToList();

            var positions = _repository.CrudServices.ProjectFromEntityToDto<Position, PositionDto>
            (
                query => query.Where(a => a.StateId == StateIdConst.ACTIVE && positionIds.Contains(a.Id))
            );
            return positions.Any() ? positions : new List<PositionDto>();
        }
        public IEnumerable<StaffingPositionDto> FillStaffingPosition(int staffingTemplateId)
        {
            var data = _repository.CrudServices.ProjectFromEntityToDto<StaffingTemplateTable, StaffingTemplateTableDto>(query => query.Where(a => a.Owner.Id == staffingTemplateId));

            foreach (var item in data)
            {
                var tariffScaleCoefTable = _repository.Context.Set<TariffScaleCoefTable>().FirstOrDefault(a => a.TariffScaleTableId == item.TariffScaleTableId);

                if (tariffScaleCoefTable != null)
                    item.TariffScaleCoef = tariffScaleCoefTable.Coef;

                return data.Select(a => new StaffingPositionDto
                {
                    PositionId = a.PositionId,
                    PositionName = a.Position,
                    TariffScaleId = a.TariffScaleId,
                    TariffScaleName = a.TariffScale,
                    TariffScaleTypeId = a.TariffScaleTypeId,
                    TariffScaleTypeName = a.TariffScaleType,
                    RankCode = a.RankCode,
                    RankName = a.RankName,
                    RankCoef = a.TariffScaleCoef,
                    CorrCoef = 1, // static 1 nearly always
                    Quantity = a.Quantity ?? 0,
                });
            }

            return new List<StaffingPositionDto>();
        }
        public StaffingPositionDto GetStaffingPosition(GetStaffingPositionDto dto)
        {
            var staffingPosition = _repository.CrudServices
                                        .ReadManyNoTracked<StaffingPositionDto>()
                                        .FirstOrDefault(a => a.Id == dto.StaffingPositionId);

            if (staffingPosition != null)
            {
                var salaryByDayCalcKind = _repository.CrudServices
                                    .ReadManyNoTracked<CalculationKindDto>()
                                    .FirstOrDefault(a => a.Id == CalculationKindIdConst.SalaryByDay);

                if (!staffingPosition.CalcKinds.Any(a => a.CalculationKindId == CalculationKindIdConst.SalaryByDay))
                {
                    staffingPosition.CalcKinds.Insert(0, new StaffingCalcKindDto
                    {
                        CalculationKindId = CalculationKindIdConst.SalaryByDay,
                        CalculationKindName = salaryByDayCalcKind?.ShortName,
                        CalcCoef = 0,
                        CalcSum = staffingPosition.Salary / staffingPosition.Quantity * dto.EmploymentRate,
                        OrgSettlementAccountId = dto.OrgSettlementAccountId,
                        orgSettlementAccountCode = dto.OrgSettlementAccountCode
                    });
                }

                return staffingPosition;
            }

            AddError("По вашему запросу запись не найдено");
            CombineStatuses(_repository);

            return null;
        }
        public SelectList<long> AsSelectList()
        {
            return _repository.AllAsQueryable.AsSelectList();
        }
        private decimal GetMinimumValue(DateTime date, int? tariffScaleId, int? minimumValueTypeId)
        {
            var minimumValueQuery = _unitOfWork.Context.Set<FixedMinimumValue>().Include(a => a.MinimumValueType).ThenInclude(a => a.TariffScales)
                .Where(a => a.DateOn <= date.AsDateOnly());

            if (tariffScaleId.HasValue)
                minimumValueQuery = minimumValueQuery.Where(a => a.MinimumValueType.TariffScales.Select(a => a.Id).Any(a => a == tariffScaleId.Value));

            if (minimumValueTypeId.HasValue)
                minimumValueQuery = minimumValueQuery.Where(a => a.MinimumValueTypeId == minimumValueTypeId);


            return minimumValueQuery.Any() ? minimumValueQuery.OrderByDescending(a => a.DateOn)
                .FirstOrDefault()!.FixedValue : 0;
        }
        public StaffingPositionDto RecalcStaffingCalcKindTables(StaffingPositionDto dto)
        {
            #region Vidrashetlarni ro`yxatini olish
            List<CalculationKindCore> calculationKinds = _calculationKindService.GetAllCalculationKindCore();

            var organizationalStructureId = _authService.Organization.OrganizationalStructureId;
            if (organizationalStructureId.HasValue)
            {
                var organizationalStructure = _repository.Context
                                                    .Set<OrganizationalStructure>()
                                                    .FirstOrDefault(a => a.Id == organizationalStructureId);

                if (organizationalStructure != null)
                {
                    if (organizationalStructure != null)
                    {
                        _repository.Context.Entry(organizationalStructure).Collection(a => a.StructureCalculationKind).Load();
                        if (organizationalStructure!.StructureCalculationKind.Any())
                        {
                            var calculationKindIds = organizationalStructure.StructureCalculationKind.Select(a => a.CalculationKindId);

                            foreach (var item in calculationKinds.Where(a => calculationKindIds.Contains(a.ID)))
                            {
                                var calculationKind = organizationalStructure.StructureCalculationKind.FirstOrDefault(a => a.CalculationKindId == item.ID);

                                if (calculationKind != null && calculationKind.Percentage > 0)
                                    item.Percentage = calculationKind.Percentage;
                            }
                        }
                    }
                }
            }

            #endregion

            #region Eng kam ish haqini olish
            List<FixedMinimumValueCore> fixedMinimumValues = new List<FixedMinimumValueCore>();

            foreach (var fixedMinimumValue in _unitOfWork.Context.Set<FixedMinimumValue>().OrderByDescending(a => a.DateOn))
            {
                fixedMinimumValues.Add(new FixedMinimumValueCore()
                {
                    Date = fixedMinimumValue.DateOn.ToDateTime(TimeOnly.MinValue),
                    MinimumValueType = SalaryCalculationCore.ToMinimumValueTypeCore(fixedMinimumValue.MinimumValueTypeId),
                    FixedSum = fixedMinimumValue.FixedValue
                });
            }
            #endregion

            SalaryCalculationCore salaryCalculationCore = new SalaryCalculationCore(calculationKinds, fixedMinimumValues);

            #region Hisoblash hujjatini yaratish
            SalaryCoreDoc salaryCoreDoc = new SalaryCoreDoc()
            {
                PersonID = 0,
                Date = DateTime.Today,
            };
            var salaryCoreEnrolment = new SalaryCoreDoc.SalaryCoreEnrolment()
            {
                ID = 1,
                OwnerID = 1,
                EnrolmentID = 1,
                PositionID = dto.PositionId.HasValue ? dto.PositionId.Value : 0,
                Rows = new List<SalaryCoreDoc.SalaryCoreEnrolment.SalaryCoreRow>(),
                PlanDays = 1,
                PlanHours = 1,
                FactDays = 1,
                FactHours = 1,
                Rate = 1,
            };
            #endregion

            #region Oklad vidrashetini hisoblash hujjatiga qo'shish
            decimal coef = 0;
            decimal basictariffrate = 0;
            dto.FixedValue = GetMinimumValue(dto.OwnerDocDate, null, MinimumValueTypeIdConst.MPOT);

            var rankCoefData = _unitOfWork.Context.Set<TariffScaleCoefTable>()
                .Include(a => a.Owner)
                .Where(a => a.Owner.TariffScaleId == dto.TariffScaleId)
                .OrderByDescending(a => a.Owner.DateOn)
                .Join(
                    _unitOfWork.Context.Set<TariffScaleTable>()
                                       .Include(a => a.Owner)
                                       .Where(a => a.Owner.StateId == StateIdConst.ACTIVE && a.RankCode == dto.RankCode),
                        a => a.Owner.TariffScaleId,
                        b => b.OwnerId,
                        (a, b) => new
                        {
                            TariffCoefTableRankCode = a.RankCode,
                            TariffTableRankCode = b.RankCode,
                            Coefficient = a.Coef
                        }).FirstOrDefault(a => a.TariffCoefTableRankCode == a.TariffTableRankCode);

            //var rankCoefData = _unitOfWork.Context.Set<TariffScaleCoefTable>()
            //    .Include(a => a.Owner)
            //    .Where(a => a.Owner.StateId == StateIdConst.ACTIVE)
            //    .OrderByDescending(a => a.Owner.OnDate)
            //    .Join(
            //        _unitOfWork.Context.Set<TariffScaleTable>().Where(a => a.RankCode == dto.RankCode),
            //            a => a.Owner.TariffScaleId,
            //            b => b.OwnerId,
            //            (a, b) => new
            //            {
            //                TariffCoefTableRankCode = a.RankCode,
            //                TariffTableRankCode = b.RankCode,
            //                Coefficient = a.Coef
            //            }).FirstOrDefault(a => a.TariffCoefTableRankCode == a.TariffTableRankCode);



            dto.RankCoef = rankCoefData != null ? rankCoefData.Coefficient : 0;

            //var positinClass = _repository.Context.Set<PositionClassification>().FirstOrDefault(x => x.Id == dto.PositionClassificationId);
            //if (positinClass == null)
            //{
            //    AddError("PositionClassification topilmadi");
            //    return null;
            //}
            //else if (positinClass.ParentId == null)
            //{ 
            //    AddError("PositionClassification ning Position topilmadi");
            //    return null;
            //}

            //var position = _repository.Context.Set<Position>().FirstOrDefault(a => a.Id == (dto.PositionId.HasValue?dto.PositionId.Value:positinClass.ParentId));
            //if (position == null)
            //{
            //    AddError("Position topilmadi");
            //    return null;
            //}

            if (dto!.TariffScaleTypeId == TariffScaleTypeIdConst.BY_CATEGORY)
            {
                if (dto.CorrCoef > 0)
                    coef = (dto.RankCoef * dto.CorrCoef) ?? 0;
                else
                    coef = dto.RankCoef ?? 0;

                dto.Salary = WEBASE.Utility.NumberUtility.Round((coef * dto.FixedValue * dto.Quantity) ?? 0, 0);
            }
            else if (dto!.TariffScaleTypeId == TariffScaleTypeIdConst.BY_BASE_SALARY)
            {
                //basictariffrate = dto.;
                //if (dto.CorrCoef > 0)
                //    dto.Salary =
                //      dto.CorrCoef * basictariffrate ?? 1 * dto.Quantity;
                //else
                //    dto.Salary = basictariffrate * dto.Quantity;

                dto.Salary = WEBASE.Utility.NumberUtility.Round((dto.Salary * dto.Quantity), 0);
            }

            var calckindsalarybyday = _calculationKindService.GetByMethod((int)CalculationMethodCore.BySalaryByDay);
            salaryCoreEnrolment.Rows.Add(new SalaryCoreDoc.SalaryCoreEnrolment.SalaryCoreRow()
            {
                ID = 0,
                OwnerID = salaryCoreEnrolment.ID,
                InSum = dto.Salary,
                CalculationKindCore = salaryCalculationCore.CalculationKinds.FirstOrDefault(x => x.ID == calckindsalarybyday.Id)
            });
            #endregion

            #region Vidrashetlarni hisoblash hujjatiga qo'shish
            dto.CalcKinds.ForEach(item =>
            {
                item.CalcSum = 0;
            });

            if (dto.PositionCategoryId.HasValue && dto.PositionCategoryId != PositionCategoryIdConst.Assistant)
            {
                salaryCalculationCore.CalculationKinds.ForEach((calculationKind) =>
                {
                    var staffrow = dto.CalcKinds.FirstOrDefault(x => x.CalculationKindId == calculationKind.ID);
                    if (staffrow != null)
                    {
                        var row = new SalaryCoreDoc.SalaryCoreEnrolment.SalaryCoreRow()
                        {
                            ID = staffrow.Id,
                            OwnerID = salaryCoreEnrolment.ID,
                            InSum = 0,
                            Percentage = (calculationKind.Percentage.HasValue && calculationKind.Percentage > 0) ? calculationKind.Percentage.Value : staffrow.CalcCoef!.Value,
                            CalculationKindCore = salaryCalculationCore.CalculationKinds.FirstOrDefault(x => x.ID == staffrow.CalculationKindId)
                        };
                        salaryCoreEnrolment.Rows.Add(row);
                    }
                });
            }
            salaryCoreDoc.Enrolments.Add(salaryCoreEnrolment);
            #endregion

            #region Hisoblash jarayoni
            var calculatedsalarydoc = salaryCalculationCore.CalculateByPerson(salaryCoreDoc);
            dto.Fot = dto.Salary;
            //calculatedsalarydoc.Enrolments.ForEach((enrolment) =>
            //{
            //    enrolment.ClaimThemeCount.ForEach((item) =>
            //    {
            //        if (dto.CalcKinds.Any(x => x.CalculationKindId == item.CalculationKindCore.ID))
            //        {
            //            dto.CalcKinds.First(x => x.CalculationKindId == item.CalculationKindCore.ID).CalcCoef = item.Percentage;
            //            dto.CalcKinds.First(x => x.CalculationKindId == item.CalculationKindCore.ID).CalcSum = item.OutSum;
            //            dto.Fot += item.OutSum;
            //        }

            //    });
            //});
            dto.CalcKindsCalcSum = dto.CalcKinds.Sum(a => a.CalcSum);
            dto.TotalSum = dto.Fot; // TODO: need to clarify 'Total Sum'
            #endregion

            return dto;
        }
        public HaveId<long> Create(CreateStaffingDlDto dto, bool autoCommit = true)
        {

            var transaction = _unitOfWork.CurrentTransaction ?? _unitOfWork.BeginTransaction();

            try
            {
                //dto.IndicatorValues = FillIndicator(dto);
                var sum = dto.IndicatorValues.Where(x => x.StaffingIndicatorId == 16).FirstOrDefault()?.TotalSum;
                dto.DocSum = sum.HasValue ? sum.Value : 0;

                var entity = _repository.Create(dto, ent => Validation(dto, ent));

                CombineStatuses(_repository);
                if (HasErrors)
                    return null;
                _unitOfWork.Save();

                var res = CreateDocumentChangeLog(entity.Id);

                if (IsValid)
                {
                    if (autoCommit)
                        transaction.Commit();
                }
                else
                    return null;
                return res;
            }
            catch
            {
                transaction.Rollback();
                throw;
            }
        }
        public void Update(UpdateStaffingDlDto dto)
        {
            using (var transaction = _unitOfWork.BeginTransaction())
            {
                var entity = _repository.Update(dto, ent =>
                {
                    if (!StatusIdConst.CanApplyHrmDocStatus(ent.StatusId, StatusIdConst.MODIFIED))
                        _repository.AddError("Нет доступа");
                    else
                        Validation(dto, ent);
                });
                CombineStatuses(_repository);
                if (HasErrors)
                    return;

                _unitOfWork.Save();

                var res = CreateDocumentChangeLog(entity.Id);
                if (IsValid)
                    transaction.Commit();
            }
        }
        public void Reject(UpdateStatusStaffingDto dto)
        {
            using (var transaction = _unitOfWork.BeginTransaction())
            {
                //var position = _unitOfWork.Context.Set<StaffingPosition>()
                //    .Include(g => g.Owner)
                //    .FirstOrDefault(g => g.OwnerId == dto.Id);
                //var anyEmployee = _unitOfWork.Context.Set<AppointEmployeeTable>()
                //    .Include(a => a.Owner)
                //    .Where(a => new int[]
                //    {
                //        StatusIdConst.ACCEPTED, StatusIdConst.CREATED
                //    }.Contains(a.Owner.StatusId) && a.PositionId == position.PositionId && a.Owner.OrganizationId == position.Owner.OrganizationId).ToList();
                //if (anyEmployee != null)
                //{
                //    AddError($"Bu shtatga hodim biriktilgan {anyEmployee.FirstOrDefault().Owner.DocNumber}");
                //}
                //CombineStatuses(_repository);
                //if (HasErrors)
                //    return;
                _unitOfWork.Context.Set<Staffing>().Lock(dto.Id);
                dto.StatusId = StatusIdConst.REJECTED;
                _repository.UpdateStatus(dto, ent =>
                {
                    if (!StatusIdConst.CanApplyHrmDocStatus(ent.StatusId, dto.StatusId))
                        AddError("Имкони йўқ / Нет доступа");
                }, applyFilter: false);
                CombineStatuses(_repository);
                if (HasErrors)
                    return;
                _unitOfWork.Save();

                var res = CreateDocumentChangeLog(dto.Id, dto.Message);
                if (HasErrors)
                {
                    transaction.Rollback();
                    return;
                }
                _unitOfWork.Save();

                if (IsValid)
                    transaction.Commit();
            }
        }
        public void Revoke(UpdateStatusStaffingDto dto)
        {
            using (var transaction = _unitOfWork.BeginTransaction())
            {
                _unitOfWork.Context.Set<Staffing>().Lock(dto.Id);
                dto.StatusId = StatusIdConst.REVOKED;
                _repository.UpdateStatus(dto, ent =>
                {
                    if (!StatusIdConst.CanApplyHrmDocStatus(ent.StatusId, dto.StatusId))
                        AddError("Имкони йўқ / Нет доступа");
                }, applyFilter: false);
                CombineStatuses(_repository);
                if (HasErrors)
                    return;
                _unitOfWork.Save();

                var res = CreateDocumentChangeLog(dto.Id, dto.Message);
                if (HasErrors)
                {
                    transaction.Rollback();
                    return;
                }
                _unitOfWork.Save();

                if (IsValid)
                    transaction.Commit();
            }
        }
        public void Receieved(UpdateStatusStaffingDto dto)
        {
            using (var transaction = _unitOfWork.BeginTransaction())
            {
                _unitOfWork.Context.Set<Staffing>().Lock(dto.Id);
                dto.StatusId = StatusIdConst.RECEIVED;
                _repository.UpdateStatus(dto, ent =>
                {
                    if (!StatusIdConst.CanApplyHrmDocStatus(ent.StatusId, dto.StatusId))
                        AddError("Имкони йўқ / Нет доступа");
                }, applyFilter: false);
                CombineStatuses(_repository);
                if (HasErrors)
                    return;
                _unitOfWork.Save();

                var res = CreateDocumentChangeLog(dto.Id, dto.Message);
                if (HasErrors)
                {
                    transaction.Rollback();
                    return;
                }
                _unitOfWork.Save();

                if (IsValid)
                    transaction.Commit();
            }
        }
        public void Delete(long id)
        {
            var dto = new UpdateStatusStaffingDlDto { Id = id, StatusId = StatusIdConst.DELETED };
            UpdateStatus(dto, ent =>
            {
                if (!StatusIdConst.CanApplyHrmDocStatus(ent.StatusId, dto.StatusId))
                    _repository.AddError("Нет доступа");
            });
        }
        public void Send(long id)
        {
            var res = UpdateStatus(
                  new() { Id = id, StatusId = StatusIdConst.SENT }
                  , ent =>
                  {
                      if (!StatusIdConst.CanApplyHrmDocStatus(ent.StatusId, StatusIdConst.SENT))
                          AddError("Нет доступа");
                  });
        }
        public void SendToArchive(long id)
        {
            var res = UpdateStatus(
                  new() { Id = id, StatusId = StatusIdConst.ARCHIVED }
                  , ent =>
                  {
                      if (!StatusIdConst.CanApplyHrmDocStatus(ent.StatusId, StatusIdConst.ARCHIVED))
                          AddError("Нет доступа");
                  });
        }
        public void RecallFromArchive(long id)
        {
            var res = UpdateStatus(
                  new() { Id = id, StatusId = StatusIdConst.RECEIVED }
                  , ent =>
                  {
                      if (!StatusIdConst.CanApplyHrmDocStatus(ent.StatusId, StatusIdConst.RECEIVED))
                          AddError("Нет доступа");
                  });
        }
        private HaveId<long> UpdateStatus(UpdateStatusStaffingDlDto dto, Action<Staffing> validation)
        {
            using (var transaction = _unitOfWork.BeginTransaction())
            {
                var entity = _repository.UpdateStatus(dto, validation, applyFilter: false);
                CombineStatuses(_repository);
                if (HasErrors)
                    return null;
                _unitOfWork.Save();
                var res = CreateDocumentChangeLog(entity.Id, dto.Message);
                if (IsValid)
                    transaction.Commit();
                return res;
            }
            return null;
        }
        public void Accept(UpdateStatusStaffingDto dto)
        {
            using (var transaction = _unitOfWork.BeginTransaction())
            {
                var anyStaffing = _unitOfWork.Context.Set<Staffing>()
                    .Include(a => a.Positions)
                    .FirstOrDefault(a => a.Id == dto.Id);
                if (anyStaffing.Positions == null || anyStaffing.Positions.Count == 0)
                {
                    AddError($"Bu shtatda lavozimlar to'ldirilmagan {dto.Id}");
                }
                _unitOfWork.Context.Set<Staffing>().Lock(dto.Id);
                dto.StatusId = StatusIdConst.ACCEPTED;
                _repository.UpdateStatus(dto, ent =>
                {
                    if (!StatusIdConst.CanApplyHrmDocStatus(ent.StatusId, dto.StatusId))
                        AddError("Имкони йўқ / Нет доступа");
                }, applyFilter: false);
                CombineStatuses(_repository);
                if (HasErrors)
                    return;
                _unitOfWork.Save();

                var res = CreateDocumentChangeLog(dto.Id, dto.Message);
                if (HasErrors)
                {
                    transaction.Rollback();
                    return;
                }
                _unitOfWork.Save();

                if (IsValid)
                    transaction.Commit();
            }
        }
        public void Cancel(UpdateStatusStaffingDto dto)
        {
            using (var transaction = _unitOfWork.BeginTransaction())
            {
                _repository.AllAsQueryable.Lock(dto.Id);
                dto.StatusId = StatusIdConst.CANCELED;
                _repository.UpdateStatus(dto, ent =>
                {
                    if (!StatusIdConst.CanApplyHrmDocStatus(ent.StatusId, dto.StatusId))
                        AddError("Имкони йўқ / Нет доступа");
                }, applyFilter: false);
                CombineStatuses(_repository);
                if (HasErrors)
                    return;
                _unitOfWork.Save();

                var res = CreateDocumentChangeLog(dto.Id, dto.Message);
                if (HasErrors)
                {
                    transaction.Rollback();
                    return;
                }
                _unitOfWork.Save();

                if (IsValid)
                    transaction.Commit();
            }
        }
        public int ParseId(string str, int row, int column)
        {
            if (int.TryParse(str.Split("--", StringSplitOptions.RemoveEmptyEntries).FirstOrDefault(), out int intstaffingTypeId))
                return intstaffingTypeId;
            AddError($"Мана бу Ид {intstaffingTypeId} хато киритилган!, катор = {row}, устун = {column}");
            return 0;
        }
        public decimal ParseDecimal(string decimal1, int row, int column)
        {
            if (decimal.TryParse(decimal1.Replace(",", "."), out decimal coef))
                return coef;
            AddError($"Мана бу коеф {coef} хато! катор = {row}, устун = {column}");
            return 0m;
        }
        private void Validation<TDto>(StaffingDlDto<TDto> dto, Staffing entity)
             where TDto : StaffingDlDto<TDto>
        {
            var query = _repository.AllAsQueryable;

            if (entity != null)
            {
                query = query.Where(a => a.Id != entity.Id);

                if (!StatusIdConst.CanModifyForHrmStatus.Contains(entity.StatusId))
                    AddError("Таҳрирлаш мумкин эмас / Невозможно редактировать");
            }

            //if (query.ByDocNumber(dto.DocNumber).Any())
            //AddError($"Документ с этим кодом {dto.DocNumber} уже существует", nameof(dto.DocNumber));
        }
        private HaveId<long> CreateDocumentChangeLog(long id, string message = null, string userIp = null, string userAgent = null)
        {
            var entityDto = _repository.ById<StaffingDto>(id, applyFilter: false);
            _documentChangeLogService.Create(
                dto: entityDto,
                tableId: TableIdConst.HRM__DOC_STAFFING,
                organizationId: null,
                statusId: entityDto.StatusId,
                message: message,
                userIp: userIp,
                userAgent: userAgent);
            CombineStatuses(_documentChangeLogService);

            if (HasErrors)
                return null;

            return HaveId.Create(id);
        }
    }
}
