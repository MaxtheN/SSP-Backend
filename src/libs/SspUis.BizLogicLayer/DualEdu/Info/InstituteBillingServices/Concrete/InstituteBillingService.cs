using Microsoft.EntityFrameworkCore;
using SspUis.Core.Security;
using SspUis.DataLayer;
using SspUis.DataLayer.EfClasses.DualEdu;
using SspUis.DataLayer.Repositories;
using StatusGeneric;
using System;
using System.Linq;
using WEBASE;
using WEBASE.Models;

namespace SspUis.BizLogicLayer;

public class InstituteBillingService : StatusGenericHandler, IInstituteBillingService
{
    private readonly IInstituteBillingRepository _repository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IAuthService _authService;

    public InstituteBillingService(IUnitOfWork unitOfWork, IAuthService authService)
    {
        _repository = unitOfWork.InstituteBillingRepository;
        _unitOfWork = unitOfWork;
        _authService = authService;
    }
    public SelectList<int> AsSelectList()
    {
        return _repository.AllAsQueryable.AsSelectList();
    }

    public HaveId<int> Create(CreateInstituteBillingDlDto dto)
    {
        var entity = _repository.Create(dto, ent => Validation(dto, ent));
        CombineStatuses(_repository);
        if (IsValid)
        {
            _unitOfWork.Save();
            return HaveId.Create(entity.Id);
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
            AddError("Запись не может быть удален");
        }
    }

    public InstituteBillingDto Get()
    {
        return new InstituteBillingDto();
    }

    public InstituteBillingDto Get(int id)
    {
        var dto = _repository.ById<InstituteBillingDto>(id);
        CombineStatuses(_repository);
        return dto;
    }

    public PagedResult<InstituteBillingListDto> GetList(SortFilterPageOptions dto)
    {
        var result = _repository.ReadAsNoTracked<InstituteBillingListDto>()
                                .SortFilter(dto)
                                .AsPagedResult(dto);
        return result;
    }

    public void Update(UpdateInstituteBillingDlDto dto)
    {
        _repository.Update(dto, ent => Validation(dto, ent));
        CombineStatuses(_repository);
        if(IsValid)
            _unitOfWork.Save();
    }

    private void Validation<TDto>(InstituteBillingDlDto<TDto> dto, InstituteBilling entity)
        where TDto : InstituteBillingDlDto<TDto>
    {
        var query = _repository.AllAsQueryable;

        if(entity != null)
            query = query.Where(a => a.Id != entity.Id);
    }
}