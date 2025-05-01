using Microsoft.EntityFrameworkCore;
using SspUis.Core.Security;
using SspUis.DataLayer;
using SspUis.DataLayer.EfClasses;
using SspUis.DataLayer.Repositories;
using StatusGeneric;
using System.Linq;
using WEBASE;
using WEBASE.Models;

namespace SspUis.BizLogicLayer.Hrm.SignCriterionService;

public class SignCriterionService : StatusGenericHandler, ISignCriterionService
{
    private readonly ISignCriterionRepository _repository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IAuthService _authService;

    public SignCriterionService(IUnitOfWork unitOfWork, IAuthService authService)
    {
        _repository = unitOfWork.SignCriterionRepository;
        _unitOfWork = unitOfWork;
        _authService = authService;
    }

    public PagedResult<SignCriterionListDto> GetList(SortFilterPageOptions dto)
    {
        var result = _repository.ReadAsNoTracked<SignCriterionListDto>().SortFilter(dto).AsPagedResult(dto);
        return result;
    }

    public SignCriterionDto Get()
    {
        return new SignCriterionDto();
    }

    public SignCriterionDto Get(int id)
    {
        var dto = _repository.ById<SignCriterionDto>(id);
        CombineStatuses(_repository);
        return dto;
    }

    public SelectList<int> AsSelectList()
    {
        return _repository.AllAsQueryable.AsSelectList();
    }

    public HaveId<int> Create(CreateSignCriterionDlDto dto)
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

    public void Update(UpdateSignCriterionDlDto dto)
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
            AddError("������ �� ����� ���� ������");
        }
    }

    private void Validation<TDto>(SignCriterionDlDto<TDto> dto, SignCriterion entity)
        where TDto : SignCriterionDlDto<TDto>
    {
        var query = _repository.AllAsQueryable;

        if (entity != null)
            query = query.Where(a => a.Id != entity.Id);

    }
}
