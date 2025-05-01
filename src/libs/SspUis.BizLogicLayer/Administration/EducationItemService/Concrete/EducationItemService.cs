using GenericServices;
using StatusGeneric;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WEBASE;
using WEBASE.AspNet;
using WEBASE.Models;
using SspUis.Core.Security;
using SspUis.DataLayer.EfClasses;
using SspUis.DataLayer;
using SspUis.DataLayer.Repositories;
using Microsoft.EntityFrameworkCore;
using SspUis.Core;

namespace SspUis.BizLogicLayer;

public class EducationItemService : StatusGenericHandler, IEducationItemService
{
    private readonly IEducationItemRepository _repository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IAuthService _authService;

    public EducationItemService(IUnitOfWork unitOfWork, IAuthService authService)
    {
        _repository = unitOfWork.EducationItemRepository;
        _unitOfWork = unitOfWork;
        _authService = authService;
    }

    public PagedResult<EducationItemListDto> GetList(SortFilterPageOptions dto)
    {
        var result = _repository.ReadAsNoTracked<EducationItemListDto>().SortFilter(dto).AsPagedResult(dto);
        return result;
    }

    public EducationItemDto Get()
    {
        return new EducationItemDto();
    }

    public EducationItemDto Get(int id)
    {
        var dto = _repository.ById<EducationItemDto>(id);
        CombineStatuses(_repository);
        return dto;
    }

    public SelectList<int> AsSelectList()
    {
        return _repository.AllAsQueryable.Include(a => a.Translates)
            .AsSelectList();
    }

    public HaveId<int> Create(CreateEducationItemDlDto dto)
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

    public void Update(UpdateEducationItemDlDto dto)
    {
        _repository.Update(dto, ent => Validation(dto, ent));
        CombineStatuses(_repository);
        if (IsValid)
            _unitOfWork.Save();
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

    private void Validation<TDto>(EducationItemDlDto<TDto> dto, EducationItem entity)
        where TDto : EducationItemDlDto<TDto>
    {
        var query = _repository.AllAsQueryable;

        if (entity != null)
            query = query.Where(a => a.Id != entity.Id);

    }
}
