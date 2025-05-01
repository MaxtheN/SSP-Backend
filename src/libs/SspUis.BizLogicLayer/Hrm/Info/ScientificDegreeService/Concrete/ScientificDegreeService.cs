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
using SspUis.DataLayer.EfClasses.Hrm;

namespace SspUis.BizLogicLayer.Hrm;

public class ScientificDegreeService : StatusGenericHandler, IScientificDegreeService
{
    private readonly IScientificDegreeRepository _repository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IAuthService _authService;

    public ScientificDegreeService(IUnitOfWork unitOfWork, IAuthService authService)
    {
        _repository = unitOfWork.ScientificDegreeRepository;
        _unitOfWork = unitOfWork;
        _authService = authService;
    }

    public PagedResult<ScientificDegreeListDto> GetList(SortFilterPageOptions dto)
    {
        var result = _repository.ReadAsNoTracked<ScientificDegreeListDto>().SortFilter(dto).AsPagedResult(dto);
        return result;
    }

    public ScientificDegreeDto Get()
    {
        return new ScientificDegreeDto();
    }

    public ScientificDegreeDto Get(int id)
    {
        var dto = _repository.ById<ScientificDegreeDto>(id);
        CombineStatuses(_repository);
        return dto;
    }

    public SelectList<int> AsSelectList()
    {
        return _repository.AllAsQueryable
            .AsSelectList();
    }

    public HaveId<int> Create(CreateScientificDegreeDlDto dto)
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

    public void Update(UpdateScientificDegreeDlDto dto)
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

    private void Validation<TDto>(ScientificDegreeDlDto<TDto> dto, ScientificDegree entity)
        where TDto : ScientificDegreeDlDto<TDto>
    {
        var query = _repository.AllAsQueryable;

        if (entity != null)
            query = query.Where(a => a.Id != entity.Id);

    }
}
