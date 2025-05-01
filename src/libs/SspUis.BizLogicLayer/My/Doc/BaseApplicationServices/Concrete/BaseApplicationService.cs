using Microsoft.EntityFrameworkCore;
using SspUis.BizLogicLayer.DocumentChangeLogServices;
using SspUis.Core;
using SspUis.DataLayer;
using SspUis.DataLayer.Repositories;
using SspUis.DataLayer.Repositories.Doc.BaseApplication;
using StatusGeneric;
using System;
using System.Linq;
using WEBASE;
using WEBASE.EF;
using WEBASE.Models;

namespace SspUis.BizLogicLayer;

public class BaseApplicationService<TEntity, TListDto, TDto, TCreateDlDto, TUpdateDlDto, TRepository, TSortFilterPageOptions>
        : StatusGenericHandler, IBaseApplicationService<long, TEntity, TListDto, TDto, TCreateDlDto, TUpdateDlDto, TSortFilterPageOptions>
        where TEntity : class, IHaveIdProp<long>, IBaseApplicationEntity
        where TListDto : class
        where TDto : class, IBaseApplication<ApplicationDto>, new()
        where TCreateDlDto : BaseApplicationDlDto<TCreateDlDto, TEntity>
        where TUpdateDlDto : BaseApplicationDlDto<TUpdateDlDto, TEntity>, IHaveIdProp<long>
        where TRepository : class, IBaseEntityRepository<long, TEntity, TCreateDlDto, TUpdateDlDto>
        where TSortFilterPageOptions : class, IPageOptions
{

    private readonly IDocumentChangeLogService _documentChangeLogService;
    //private readonly IPersonService _admPersonService;

    public BaseApplicationService(IUnitOfWork unitOfWork, IDocumentChangeLogService documentChangeLogService)
    {
        UnitOfWork = unitOfWork;
        Repository = unitOfWork.GetRepository<TRepository>();
        _documentChangeLogService = documentChangeLogService;
        //_admPersonService = admPersonService;
    }

    protected IUnitOfWork UnitOfWork { get; }
    protected TRepository Repository { get; }

    protected virtual IQueryable<TListDto> SortFilter(IQueryable<TListDto> query, TSortFilterPageOptions options)
    {
        return query;
    }

    public virtual PagedResult<TListDto> GetList(TSortFilterPageOptions options)
    {
        var result = SortFilter(Repository.ReadAsNoTracked<TListDto>(), options)
                            .AsPagedResult(options);
        return result;
    }

    public virtual TDto Get()
    {
        return new TDto();
    }

    public virtual TDto Get(long id)
    {
        var dto = Repository.ById<TDto>(id);
        CombineStatuses(Repository);
        return dto;
    }

    public virtual HaveId<long> Create(TCreateDlDto dto)
    {
        bool canCommit = UnitOfWork.CurrentTransaction == null;
        var transaction = UnitOfWork.CurrentTransaction ?? UnitOfWork.BeginTransaction();

        try
        {
            #region IHMA code
            //var serviceRecipient = UnitOfWork.PersonRepository.ByPinfl(dto.Application.ServiceRecipient.Pinfl);
            //if (serviceRecipient != null)
            //{
            //    dto.Application.ServiceRecipientId = serviceRecipient.Id;
            //}
            //else
            //{
            //    var personCreateResult = await _admPersonService.Create(new PersonCreateDto
            //    {
            //        BirthOn = dto.Application.ServiceRecipient.BirthOn,
            //        CitizenshipId = dto.Application.ServiceRecipient.CitizenshipId,
            //        DocExpireOn = dto.Application.ServiceRecipient.DocExpireOn,
            //        DocIssueOn = dto.Application.ServiceRecipient.DocIssueOn,
            //        DocIssueOrganization = dto.Application.ServiceRecipient.DocIssueOrganization,
            //        DocNumber = dto.Application.ServiceRecipient.DocNumber,
            //        DocSeria = dto.Application.ServiceRecipient.DocSeria,
            //        DocumentTypeId = dto.Application.ServiceRecipient.DocumentTypeId,
            //        FirstName = dto.Application.ServiceRecipient.FirstName,
            //        GenderId = dto.Application.ServiceRecipient.GenderId,
            //        LastName = dto.Application.ServiceRecipient.LastName,
            //        MiddleName = dto.Application.ServiceRecipient.MiddleName,
            //        NationalityId = dto.Application.ServiceRecipient.NationalityId,
            //        Pinfl = dto.Application.ServiceRecipient.Pinfl,
            //        StateId = dto.Application.ServiceRecipient.StateId
            //    });
            //    CombineStatuses(_admPersonService);

            //    if (!personCreateResult.IsSuccess)
            //    {
            //        AddError($"Шахс маълумоти яратилишида хатолик юз берди: {personCreateResult.ResponseAsString}");
            //        return null;
            //    }

            //    dto.Application.ServiceRecipientId = personCreateResult.Response.Id;
            //}

            //if (dto.Application.ApplicantPerson != null && !string.IsNullOrEmpty(dto.Application.ApplicantPerson.Pinfl))
            //{
            //    if (!dto.Application.ForSelf)
            //    {
            //        var applicantPerson = UnitOfWork.PersonRepository.ByPinfl(dto.Application.ApplicantPerson.Pinfl);
            //        if (applicantPerson != null)
            //        {
            //            dto.Application.ApplicantPersonId = applicantPerson.Id;
            //        }
            //        else
            //        {
            //            var personCreateResult = await _admPersonService.Create(new PersonCreateDto
            //            {
            //                BirthOn = dto.Application.ApplicantPerson.BirthOn,
            //                CitizenshipId = dto.Application.ApplicantPerson.CitizenshipId,
            //                DocExpireOn = dto.Application.ApplicantPerson.DocExpireOn,
            //                DocIssueOn = dto.Application.ApplicantPerson.DocIssueOn,
            //                DocIssueOrganization = dto.Application.ApplicantPerson.DocIssueOrganization,
            //                DocNumber = dto.Application.ApplicantPerson.DocNumber,
            //                DocSeria = dto.Application.ApplicantPerson.DocSeria,
            //                DocumentTypeId = dto.Application.ApplicantPerson.DocumentTypeId,
            //                FirstName = dto.Application.ApplicantPerson.FirstName,
            //                GenderId = dto.Application.ApplicantPerson.GenderId,
            //                LastName = dto.Application.ApplicantPerson.LastName,
            //                MiddleName = dto.Application.ApplicantPerson.MiddleName,
            //                NationalityId = dto.Application.ApplicantPerson.NationalityId,
            //                Pinfl = dto.Application.ApplicantPerson.Pinfl,
            //                StateId = dto.Application.ApplicantPerson.StateId
            //            });
            //            CombineStatuses(_admPersonService);

            //            if (!personCreateResult.IsSuccess)
            //            {
            //                AddError($"Шахс маълумоти яратилишида хатолик юз берди: {personCreateResult.ResponseAsString}");
            //                return null;
            //            }

            //            dto.Application.ApplicantPersonId = personCreateResult.Response.Id;
            //        }
            //    }
            //    else
            //    {
            //        dto.Application.ApplicantPersonId = null;
            //        dto.Application.ApplicantPhoneNumber = null;
            //        dto.Application.ApplicantPhoneNumberAlt = null;
            //        dto.Application.ApplicantPerson = null;
            //    }
            //}
            #endregion
            if (HasErrors)
                return null;

            var entity = Repository.Create(dto, entity => CreateValidation(dto, entity));
            CombineStatuses(Repository);
            if (IsValid)
            {
                UnitOfWork.Save();
                //CreateDocumentChangeLog(entity.Application.Id);
                if (IsValid)
                {
                    if (canCommit)
                        transaction.Commit();

                    return HaveId.Create(entity.Application.Id);
                }
            }
        }
        catch (Exception ex)
        {
            transaction.Rollback();
            AddError($"{ex.Message}: {ex.InnerException}");
        }
        return null;
    }

    public virtual void Update(TUpdateDlDto dto)
    {
        bool canCommit = UnitOfWork.CurrentTransaction == null;
        var transaction = UnitOfWork.CurrentTransaction ?? UnitOfWork.BeginTransaction();

        try
        {
            #region IHMA code
            //var serviceRecipient = UnitOfWork.PersonRepository.ByPinfl(dto.Application.ServiceRecipient.Pinfl);
            //if (serviceRecipient != null)
            //{
            //    dto.Application.ServiceRecipientId = serviceRecipient.Id;
            //}
            //else
            //{
            //    var personCreateResult = await _admPersonService.Create(new PersonCreateDto
            //    {
            //        BirthOn = dto.Application.ServiceRecipient.BirthOn,
            //        CitizenshipId = dto.Application.ServiceRecipient.CitizenshipId,
            //        DocExpireOn = dto.Application.ServiceRecipient.DocExpireOn,
            //        DocIssueOn = dto.Application.ServiceRecipient.DocIssueOn,
            //        DocIssueOrganization = dto.Application.ServiceRecipient.DocIssueOrganization,
            //        DocNumber = dto.Application.ServiceRecipient.DocNumber,
            //        DocSeria = dto.Application.ServiceRecipient.DocSeria,
            //        DocumentTypeId = dto.Application.ServiceRecipient.DocumentTypeId,
            //        FirstName = dto.Application.ServiceRecipient.FirstName,
            //        GenderId = dto.Application.ServiceRecipient.GenderId,
            //        LastName = dto.Application.ServiceRecipient.LastName,
            //        MiddleName = dto.Application.ServiceRecipient.MiddleName,
            //        NationalityId = dto.Application.ServiceRecipient.NationalityId,
            //        Pinfl = dto.Application.ServiceRecipient.Pinfl,
            //        StateId = dto.Application.ServiceRecipient.StateId
            //    });
            //    CombineStatuses(_admPersonService);

            //    if (!personCreateResult.IsSuccess)
            //    {
            //        AddError($"Шахс маълумоти яратилишида хатолик юз берди: {personCreateResult.ResponseAsString}");
            //        return;
            //    }

            //    dto.Application.ServiceRecipientId = personCreateResult.Response.Id;
            //}

            //if (dto.Application.ApplicantPerson != null && !string.IsNullOrEmpty(dto.Application.ApplicantPerson.Pinfl))
            //{
            //    if (!dto.Application.ForSelf)
            //    {
            //        var applicantPerson = UnitOfWork.PersonRepository.ByPinfl(dto.Application.ApplicantPerson.Pinfl);
            //        if (applicantPerson != null)
            //        {
            //            dto.Application.ApplicantPersonId = applicantPerson.Id;
            //        }
            //        else
            //        {
            //            var personCreateResult = await _admPersonService.Create(new PersonCreateDto
            //            {
            //                BirthOn = dto.Application.ApplicantPerson.BirthOn,
            //                CitizenshipId = dto.Application.ApplicantPerson.CitizenshipId,
            //                DocExpireOn = dto.Application.ApplicantPerson.DocExpireOn,
            //                DocIssueOn = dto.Application.ApplicantPerson.DocIssueOn,
            //                DocIssueOrganization = dto.Application.ApplicantPerson.DocIssueOrganization,
            //                DocNumber = dto.Application.ApplicantPerson.DocNumber,
            //                DocSeria = dto.Application.ApplicantPerson.DocSeria,
            //                DocumentTypeId = dto.Application.ApplicantPerson.DocumentTypeId,
            //                FirstName = dto.Application.ApplicantPerson.FirstName,
            //                GenderId = dto.Application.ApplicantPerson.GenderId,
            //                LastName = dto.Application.ApplicantPerson.LastName,
            //                MiddleName = dto.Application.ApplicantPerson.MiddleName,
            //                NationalityId = dto.Application.ApplicantPerson.NationalityId,
            //                Pinfl = dto.Application.ApplicantPerson.Pinfl,
            //                StateId = dto.Application.ApplicantPerson.StateId
            //            });
            //            CombineStatuses(_admPersonService);

            //            if (!personCreateResult.IsSuccess)
            //            {
            //                AddError($"Шахс маълумоти яратилишида хатолик юз берди: {personCreateResult.ResponseAsString}");
            //                return;
            //            }

            //            dto.Application.ApplicantPersonId = personCreateResult.Response.Id;
            //        }
            //    }
            //    else
            //    {
            //        dto.Application.ApplicantPersonId = null;
            //        dto.Application.ApplicantPhoneNumber = null;
            //        dto.Application.ApplicantPhoneNumberAlt = null;
            //        dto.Application.ApplicantPerson = null;
            //    }
            //}
            //if (HasErrors)
            //    return;
            #endregion
            var entity = Repository.Update(dto, entity => UpdateValidation(dto, entity));
            CombineStatuses(Repository);
            if (IsValid)
            {
                UnitOfWork.Save();
                //CreateDocumentChangeLog(entity.Application.Id);
                if (IsValid && canCommit)
                {
                    transaction.Commit();
                }
            }
        }
        catch (Exception ex)
        {
            transaction.Rollback();
            AddError($"{ex.Message}: {ex.InnerException}");
        }
    }

    public virtual void Delete(long id)
    {
        try
        {
            Repository.Delete(id);
            CombineStatuses(Repository);
            if (IsValid)
                UnitOfWork.Save();
        }
        catch (DbUpdateException)
        {
            AddError("Запись не может быть удален");
        }
    }

    public virtual void CreateValidation(TCreateDlDto dto, TEntity entity)
    {

    }

    public virtual void UpdateValidation(TUpdateDlDto dto, TEntity entity)
    {

    }


    //public HaveId<long> CreateDocumentChangeLog(long id, string? message = null)
    //{
    //    var moveDto = Repository.ById<TDto>(id, applyFilter: false);
    //    _documentChangeLogService.CreateApplication<TDto>(moveDto, OrganizationIdConst.SSP, message);
    //    CombineStatuses(_documentChangeLogService);

    //    if (HasErrors)
    //        return null;

    //    return HaveId.Create(id);
    //}
}