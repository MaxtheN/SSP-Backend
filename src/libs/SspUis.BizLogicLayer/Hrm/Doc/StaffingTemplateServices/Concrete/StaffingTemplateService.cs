using Microsoft.EntityFrameworkCore;
using SspUis.BizLogicLayer.ApplicationServices;
using SspUis.BizLogicLayer.DocumentChangeLogServices;
using SspUis.Core;
using SspUis.Core.Security;
using SspUis.DataLayer;
using SspUis.DataLayer.EfClasses;
using SspUis.DataLayer.EfClasses.Hrm;
using SspUis.DataLayer.Repositories.Hrm;
using SspUis.ServiceLayer.NumberServices;
using StatusGeneric;
using System;
using System.Collections.Generic;
using System.Linq;
using WEBASE;
using WEBASE.Models;

namespace SspUis.BizLayer.Hrm.StaffingTemplateServices
{
    public class StaffingTemplateService : StatusGenericHandler, IStaffingTemplateService
    {
        private readonly IStaffingTemplateRepository _repository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IAuthService _authService;
        private readonly IDocumentChangeLogService _documentChangeLogService;
        private readonly INumberService _numberService;

        public StaffingTemplateService(
            IUnitOfWork unitOfWork,
            IAuthService authService,
            IDocumentChangeLogService documentChangeLogService,
            INumberService numberService)
        {
            _repository = unitOfWork.StaffingTemplateRepository;
            _unitOfWork = unitOfWork;
            _authService = authService;
            _documentChangeLogService = documentChangeLogService;
            _numberService = numberService;
        }
        public PagedResult<StaffingTemplateListDto> GetList(StaffingTemplateListSortFilterDto dto)
        {
            if (dto.OrganizationSettlementAccountCode != null)
            {
                return _repository.CrudServices.ProjectFromEntityToDto<StaffingTemplate, StaffingTemplateListDto>(query => query.Where(a => a.OrganizationId == _authService.Organization.Id && a.StatusId == StatusIdConst.ACCEPTED))
                    .SortFilter(dto)
                        .AsPagedResult(dto);
            }

            return _repository.ReadAsNoTracked<StaffingTemplateListDto>()
                .Where(a => a.OrganizationId == _authService.Organization.Id)
                .SortFilter(dto)
                .AsPagedResult(dto);
        }
        public SelectList<long> AsSelectList()
        {
            return _repository.AllAsQueryable
                            .AsSelectList();
        }

        public StaffingTemplateDto Get()
        {
            return new StaffingTemplateDto
            {
                DocOn = DateTime.Today.AsDateOnly(),
                DocNumber = _numberService.GetNext(nameof(TableIdConst.HRM__DOC_STAFFING_TEMPLATE), 1).Item2
            };
        }

        public StaffingTemplateDto Get(long id)
        {
            var dto = _repository.ById<StaffingTemplateDto>(id);
            CombineStatuses(_repository);
            if (IsValid)
            {
                dto.CanModify = StatusIdConst.CanApplyHrmDocStatus(dto.StatusId, StatusIdConst.MODIFIED) && _authService.HasPermission(ModuleCode.StaffingTemplateEdit);
                dto.CanAccept = StatusIdConst.CanApplyHrmDocStatus(dto.StatusId, StatusIdConst.ACCEPTED) && _authService.HasPermission(ModuleCode.StaffingTemplateAccept);
                dto.CanCancel = StatusIdConst.CanApplyHrmDocStatus(dto.StatusId, StatusIdConst.NOT_ACCEPTED) && _authService.HasPermission(ModuleCode.StaffingTemplateCancel);
                dto.CanDelete = StatusIdConst.CanApplyHrmDocStatus(dto.StatusId, StatusIdConst.DELETED) && _authService.HasPermission(ModuleCode.StaffingTemplateDelete);

                InitializeTariffScaleCoef(dto.Tables);
            }
            return dto;
        }

        public IEnumerable<StaffingPostitionDto> GetPositionsAsSelectList(
            string? organizationSettlementAccountCode = null,
            int? staffingTemplateId = null)
        {
            var staffingTempTables
                = GetTableAsSelectList(
                        organizationSettlementAccountCode,
                        staffingTemplateId);

            if (staffingTempTables.Any())
                return staffingTempTables.Select(staffingTemplateDto => new StaffingPostitionDto()
                {
                    Position = staffingTemplateDto.Position,
                    PositionId = staffingTemplateDto.PositionId,
                    TariffScaleTypeId = staffingTemplateDto.TariffScaleTypeId,
                    TariffScaleType = staffingTemplateDto.TariffScaleType
                });
            var positions = _unitOfWork.Context.Set<DataLayer.EfClasses.Position>()
                .Include(position => position.TariffScaleType)
                .Include(position => position.StaffTypeBasicTariff)
                .IsActive();

            return positions.Select(position => new StaffingPostitionDto()
            {
                Position = position.FullName,
                PositionId = position.Id,
                StaffTypeBasicTariffId = position.StaffTypeBasicTariffId,
                StaffTypeBasicTariff = position.StaffTypeBasicTariff != null ? position.StaffTypeBasicTariff.FullName : "",
                TariffScaleTypeId = position.TariffScaleTypeId ?? 0,
                TariffScaleType = position.TariffScaleType.FullName
            });
        }
        private IEnumerable<StaffingTemplateTableDto> GetTableAsSelectList(
            string? organizationSettlementAccountCode = null,
            int? staffingTemplateId = null)
        {
            List<StaffingTemplateTableDto> result = null;

            if (organizationSettlementAccountCode != null)
            {
                result = _repository.CrudServices.ProjectFromEntityToDto<StaffingTemplateTable, StaffingTemplateTableDto>(
                    query => query.Where(a => a.Owner.StatusId == StatusIdConst.ACCEPTED
                        && a.Owner.OrganizationId == _authService.Organization.Id)
                    )
                    .ToList();
            }
            else
                result = _repository.CrudServices.ProjectFromEntityToDto<StaffingTemplateTable, StaffingTemplateTableDto>(
                    query => query.Where(a =>
                                            a.Owner.OrganizationId == _authService.Organization.Id &&
                                            (staffingTemplateId.HasValue ? a.OwnerId == staffingTemplateId : true) &&
                                            a.Owner.StatusId == StatusIdConst.ACCEPTED)
                                         )
                    .ToList();

            InitializeTariffScaleCoef(result);

            return result;
        }
        public HaveId<long> Create(CreateStaffingTemplateDlDto dto)
        {
            using (var transaction = _unitOfWork.BeginTransaction())
            {
                var entity = _repository.Create(dto, ent => Validation(dto, ent));

                CombineStatuses(_repository);
                if (HasErrors)
                    return null;
                _unitOfWork.Save();

                var res = CreateDocumentChangeLog(entity.Id);
                if (IsValid)
                {
                    transaction.Commit();
                }
                return res;
            }
            return null;
        }
        public void Update(UpdateStaffingTemplateDlDto dto)
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
        public void Accept(UpdateStatusStaffingTemplateDlDto statusDto)
        {
            var dto = new UpdateStatusStaffingTemplateDlDto { Id = statusDto.Id, StatusId = StatusIdConst.ACCEPTED };
            UpdateStatus(dto, ent =>
            {
                if (!StatusIdConst.CanApplyHrmDocStatus(ent.StatusId, dto.StatusId))
                    _repository.AddError("Нет доступа");
            });
        }
        public void Cancel(UpdateStatusStaffingTemplateDlDto statusDto)
        {
            var dto = new UpdateStatusStaffingTemplateDlDto { Id = statusDto.Id, StatusId = StatusIdConst.NOT_ACCEPTED };
            UpdateStatus(dto, ent =>
            {
                if (!StatusIdConst.CanApplyHrmDocStatus(ent.StatusId, dto.StatusId))
                    _repository.AddError("Нет доступа");

                bool existsStaffing = _unitOfWork.Context.Set<Staffing>()
                    .Any(a => a.StatusId != StatusIdConst.DELETED);
                if (existsStaffing)
                {
                    AddError("Bu namunaga shtatlar jadvali hujjati mavjud / На этот шаблон создан штатное расписание");
                }
            });
        }
        public void Delete(long id)
        {
            var dto = new UpdateStatusStaffingTemplateDlDto { Id = id, StatusId = StatusIdConst.DELETED };
            UpdateStatus(dto, ent =>
            {
                if (!StatusIdConst.CanApplyHrmDocStatus(ent.StatusId, dto.StatusId))
                    _repository.AddError("Нет доступа");
            });
        }
        private HaveId<long> UpdateStatus(UpdateStatusStaffingTemplateDlDto dto, Action<StaffingTemplate> validation)
        {
            using (var transaction = _unitOfWork.BeginTransaction())
            {
                try
                {
                    var entity = _repository.UpdateStatus(dto, validation);
                    CombineStatuses(_repository);
                    if (HasErrors)
                        return null;
                    _unitOfWork.Save();
                    var res = CreateDocumentChangeLog(entity.Id, dto.Message);
                    if (IsValid)
                        transaction.Commit();
                    return res;
                }
                catch (Exception ex)
                {
                    AddError(ex.Message);
                    transaction.Rollback();
                }
            }
            return null;
        }
        private void Validation<TDto>(StaffingTemplateDlDto<TDto> dto, StaffingTemplate entity)
          where TDto : StaffingTemplateDlDto<TDto>
        {
            var query = _repository.AllAsQueryable;

            if (entity != null)
            {
                query = query.Where(a => a.Id != entity.Id);

                if (!StatusIdConst.CanModifyStatus.Contains(entity.StatusId))
                    AddError("Таҳрирлаш мумкин эмас / Невозможно редактировать");
            }

            if (query.ByDocNumber(dto.DocNumber).Any())
                _repository.AddError($"Документ с этим кодом {dto.DocNumber} уже существует", nameof(dto.DocNumber));
        }
        private void InitializeTariffScaleCoef(IEnumerable<StaffingTemplateTableDto> tables)
        {
            foreach (var item in tables)
            {
                var tariffScaleCoefTable = _repository.Context.Set<TariffScaleCoefTable>().FirstOrDefault(a => a.TariffScaleTableId == item.TariffScaleTableId);

                if (tariffScaleCoefTable != null)
                    item.TariffScaleCoef = tariffScaleCoefTable.Coef;
            }
        }


        private HaveId<long> CreateDocumentChangeLog(long id, string message = null, string userIp = null, string userAgent = null)
        {
            var entityDto = _repository.ById<StaffingTemplateDto>(id, applyFilter: false);
            _documentChangeLogService.Create(
                dto: entityDto,
                tableId: TableIdConst.HRM__DOC_STAFFING_TEMPLATE,
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
