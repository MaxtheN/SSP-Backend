using Microsoft.EntityFrameworkCore;
using SspUis.DataLayer;
using SspUis.DataLayer.EfClasses.DualEdu;
using SspUis.DataLayer.Repositories;
using StatusGeneric;
using System;
using System.Linq;
using WEBASE;
using WEBASE.Models;

namespace SspUis.BizLogicLayer;

public class SpecialtyBillingService : StatusGenericHandler, ISpecialtyBillingService
{
    private readonly ISpecialtyBillingRepository _repository;
    private readonly IUnitOfWork _unitOfWork;

    public SpecialtyBillingService(IUnitOfWork unitOfWork)
    {
        _repository = unitOfWork.SpecialtyBillingRepository;
        _unitOfWork = unitOfWork;
    }
    public SelectList<int> AsSelectList(int? instituteId = null)
    {
        return _repository.AllAsQueryable
            .AsSelectList(instituteId);
    }

    public HaveId<int> Create(CreateSpecialtyBillingDlDto dto)
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

    public SpecialtyBillingDto Get()
    {
        return new SpecialtyBillingDto();
    }

    public SpecialtyBillingDto Get(int id)
    {
        var dto = _repository.ById<SpecialtyBillingDto>(id);
        CombineStatuses(_repository);
        return dto;
    }

    public PagedResult<SpecialtyBillingListDto> GetList(SortFilterPageOptions dto)
    {
        var result = _repository.ReadAsNoTracked<SpecialtyBillingListDto>().SortFilter(dto).AsPagedResult(dto);
        return result;
    }

    public void Update(UpdateSpecialtyBillingDlDto dto)
    {
        _repository.Update(dto, ent => Validation(dto, ent));
        CombineStatuses(_repository);
        if(IsValid)
            _unitOfWork.Save();
    }
    
    private void Validation<TDto>(SpecialtyBillingDlDto<TDto> dto, SpecialtyBilling entity)
        where TDto : SpecialtyBillingDlDto<TDto>
    {
        var query = _repository.AllAsQueryable;

        if(entity != null)
            query = query.Where(a => a.Id != entity.Id);
    }
}