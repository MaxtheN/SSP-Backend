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

public class AcademicDegreeService : StatusGenericHandler, IAcademicDegreeService
{
    private readonly IAcademicDegreeRepository _repository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IAuthService _authService;

    public AcademicDegreeService(IUnitOfWork unitOfWork, IAuthService authService)
    {
        _repository = unitOfWork.AcademicDegreeRepository;
        _unitOfWork = unitOfWork;
        _authService = authService;
    }

    public PagedResult<AcademicDegreeListDto> GetList(SortFilterPageOptions dto)
    {
        var result = _repository.ReadAsNoTracked<AcademicDegreeListDto>().SortFilter(dto).AsPagedResult(dto);
        return result;
    }

    public AcademicDegreeDto Get()
    {
        return new AcademicDegreeDto();
    }

    public AcademicDegreeDto Get(int id)
    {
        var dto = _repository.ById<AcademicDegreeDto>(id);
        CombineStatuses(_repository);
        return dto;
    }

    public SelectList<int> AsSelectList()
    {
        return _repository.AllAsQueryable
            .AsSelectList();
    }

    public HaveId<int> Create(CreateAcademicDegreeDlDto dto)
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

    public void Update(UpdateAcademicDegreeDlDto dto)
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

    private void Validation<TDto>(AcademicDegreeDlDto<TDto> dto, AcademicDegree entity)
        where TDto : AcademicDegreeDlDto<TDto>
    {
        var query = _repository.AllAsQueryable;

        if (entity != null)
            query = query.Where(a => a.Id != entity.Id);

    }
}
