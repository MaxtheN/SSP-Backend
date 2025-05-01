using Microsoft.EntityFrameworkCore;
using SspUis.DataLayer;
using SspUis.DataLayer.EfClasses;
using SspUis.DataLayer.Repositories;
using SspUis.Integration.Edoc.Models;
using StatusGeneric;
using System;
using System.Linq;
using WEBASE.Models;

namespace SspUis.BizLogicLayer
{
    public class ExternalDocFromEdocService : StatusGenericHandler, IExternalDocFromEdocService
    {

        private readonly IExternalDocFromEdocRepository _repository;

        private readonly IUnitOfWork _unitOfWork;
        public ExternalDocFromEdocService(IUnitOfWork unitOfWork, IExternalDocFromEdocRepository repository)
        {
            _repository = repository;
            _unitOfWork = unitOfWork;
        }

        public HaveId<int> Create(CreateExternalDocumentFromEdocDlDto dto)
        {
            try
            {
                dto.AppealApplicationId = dto.AppealApplicationId == 0 ? null : dto.AppealApplicationId;
                dto.CallCenterAppealId = dto.CallCenterAppealId == 0 ? null : dto.CallCenterAppealId;

                var template = _unitOfWork.Context.Set<ExternalDocumentFromEdoc>().FirstOrDefault(x =>
                  dto.AppealApplicationId.HasValue && dto.AppealApplicationId != 0
                    ? x.AppealAplicationtId == dto.AppealApplicationId
                    : dto.CallCenterAppealId.HasValue && dto.CallCenterAppealId != 0
                        ? x.CallCenterAppealId == dto.CallCenterAppealId
                        : false);
                if (template != null)
                {
                    UpdateExternalDocumentFromEdocDlDto val = new()
                    {
                        Id = template.Id,
                        RegNumber = dto.RegNumber,
                        AppealApplicationId = dto.AppealApplicationId,
                        CallCenterAppealId = dto.CallCenterAppealId,
                        TermExecution = dto.TermExecution,
                        Assignment = dto.Assignment,
                        OrganizationId = dto.OrganizationId,
                        ProcessId = dto.ProcessId,
                        RegDate = dto.RegDate
                    };
                    Update(val);
                }
                else
                {
                    //ExternalDocumentFromEdoc entity = _repository.Create(dto, ent => Validation(dto, ent));
                    ExternalDocumentFromEdoc entity = new()
                    {
                        TermExecution = dto.TermExecution,
                        RegNumber = dto.RegNumber,
                        RegDate = dto.RegDate,
                        Assignment = dto.Assignment,
                        OrganizationId = dto.OrganizationId,
                        ProcessId = dto.ProcessId,
                        CallCenterAppealId = dto.CallCenterAppealId,
                        AppealAplicationtId = dto.AppealApplicationId
                    };

                    _unitOfWork.Context.Set<ExternalDocumentFromEdoc>()
                        .Add(entity);
                    _unitOfWork.Save();
                    template = entity;
                }
                CombineStatuses(_repository);
                if (IsValid)
                {
                    _unitOfWork.Save();
                    return HaveId.Create(template.Id);
                }
            }
            catch (Exception ex)
            {
                AddError(ex.Message + " Inner: " + ex.InnerException);
                AddError("AppealApplicationId" + dto.AppealApplicationId + ",  CallCenterAppealId:" + dto.CallCenterAppealId);
            }
            return null;
        }

        public void Delete(int id)
        {
            try
            {
                _repository.Delete(id);
                CombineStatuses(_repository);
                if (IsValid)
                    _unitOfWork.Save();
            }
            catch (DbUpdateException)
            {
                AddError("������ �� ����� ���� ������");
            }
        }
        public void UpdateForAppeal(UpdateExternalDocumentFromEdocDlDto dto)
        {
            var entity = _unitOfWork.Context.Set<ExternalDocumentFromEdoc>().FirstOrDefault(x => x.AppealAplicationtId == dto.AppealApplicationId);
            if (entity != null)
                Update(dto);
        }
        public void Update(UpdateExternalDocumentFromEdocDlDto dto)
        {
            try
            {
                dto.AppealApplicationId = dto.AppealApplicationId == 0 ? null : dto.AppealApplicationId;
                dto.CallCenterAppealId = dto.CallCenterAppealId == 0 ? null : dto.CallCenterAppealId;

                var entity = _unitOfWork.Context.Set<ExternalDocumentFromEdoc>().FirstOrDefault(x =>
                    dto.AppealApplicationId.HasValue && dto.AppealApplicationId != 0
                    ? x.AppealAplicationtId == dto.AppealApplicationId
                    : dto.CallCenterAppealId.HasValue && dto.CallCenterAppealId != 0
                        ? x.CallCenterAppealId == dto.CallCenterAppealId
                        : false);

                UpdateExternalDocumentFromEdocDlDto temp = null;
                if (entity != null)
                {
                    UpdateExternalDocumentFromEdocDlDto val = new()
                    {
                        Id = entity.Id,
                        RegNumber = dto.RegNumber,
                        TermExecution = dto.TermExecution,
                        Assignment = dto.Assignment,
                        OrganizationId = dto.OrganizationId,
                        ProcessId = dto.ProcessId,
                        RegDate = dto.RegDate,
                        CallCenterAppealId = dto.CallCenterAppealId,
                        AppealApplicationId = dto.AppealApplicationId,
                        OutgoingDocCreatedData = dto.OutgoingDocCreatedData,
                    };
                    temp = val;
                    _repository.Update(temp, ent => Validation(temp, ent));
                }
                else
                {
                    Create(new()
                    {
                        RegDate = dto.RegDate,
                        RegNumber = dto.RegNumber,
                        TermExecution = dto.TermExecution,
                        Assignment = dto.Assignment,
                        OrganizationId = dto.OrganizationId,
                        ProcessId = dto.ProcessId,
                        CallCenterAppealId = dto.CallCenterAppealId,
                        AppealApplicationId = dto.AppealApplicationId,
                        OutgoingDocCreatedData= dto.OutgoingDocCreatedData,
                    });
                    AddError(" Not Found from ERP");
                }
                CombineStatuses(_repository);
                if (IsValid)
                    _unitOfWork.Save();
            }
            catch (Exception ex)
            {
                AddError(ex.Message + " Inner: " + ex.InnerException);
                AddError("AppealApplicationId" + dto.AppealApplicationId + ",  CallCenterAppealId:" + dto.CallCenterAppealId);
            }
        }

        private void Validation<TDto>(ExternalDocumentFromEdocDlDto<TDto> dto, ExternalDocumentFromEdoc entity)
        where TDto : ExternalDocumentFromEdocDlDto<TDto>
        {
            var query = _repository.AllAsQueryable;

            //if (entity != null)
            //    query = query.Where(a => a.Id != entity.Id);

            //var exsisit = query.Any(ent => ent.AppealAplicationtId == dto.AppealApplicationId);
            //if (exsisit)
            //    AddError($"Запись  ({dto?.AppealApplicationId}) уже существует.",
            //            nameof(dto.AppealApplicationId));
        }

        public ExternalIncomingDocumentDto GetByAppealId(long appealApplicationId)
        {
            var entity = _unitOfWork.Context.Set<ExternalDocumentFromEdoc>()
                .FirstOrDefault(x => x.AppealAplicationtId == appealApplicationId);

            if (entity == null)
                return null;

            return new()
            {
                Id = default,
                TermExecution = entity.TermExecution,
                Assignment = entity.Assignment,
                Organization = entity.Organization?.FullName,
                RegNumber = entity.RegNumber,
                RegDate = entity.RegDate,
                ProcessId = entity.ProcessId ?? 0
            };
        }
        public ExternalIncomingDocumentDto GetByCallCenterAppealId(long callCenterAppealId)
        {
            var entity = _unitOfWork.Context.Set<ExternalDocumentFromEdoc>()
                .FirstOrDefault(x => x.CallCenterAppealId == callCenterAppealId);

            if (entity == null)
                return null;

            return new()
            {
                Id = default,
                TermExecution = entity.TermExecution,
                Assignment = entity.Assignment,
                Organization = entity.Organization?.FullName,
                RegNumber = entity.RegNumber,
                RegDate = entity.RegDate,
                ProcessId = entity.ProcessId ?? 0
            };
        }
    }
}
