using System.Linq;
using Microsoft.EntityFrameworkCore;
using SspUis.Core.Security;
using SspUis.DataLayer;
using SspUis.DataLayer.EfClasses.Appeal;
using SspUis.DataLayer.Repositories;
using StatusGeneric;
using WEBASE;
using WEBASE.Models;

namespace SspUis.BizLogicLayer.AppealDescriptionServices;

public class AppealDescriptionService : StatusGenericHandler, IAppealDescriptionService
{
    private readonly IAppealDescriptionRepository _repository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IAuthService _authService;

    public AppealDescriptionService(IUnitOfWork unitOfWork, IAuthService authService)
    {
        _repository = unitOfWork.AppealDescriptionRepository;
        _unitOfWork = unitOfWork;
        _authService = authService;
    }

    public PagedResult<AppealDescriptionListDto> GetList(SortFilterPageOptions dto)
    {
        var result = _repository.ReadAsNoTracked<AppealDescriptionListDto>().SortFilter(dto).AsPagedResult(dto);
        return result;
    }

    public AppealDescriptionDto Get()
    {
        return new AppealDescriptionDto();
    }

    public AppealDescriptionDto Get(int id)
    {
        var dto = _repository.ById<AppealDescriptionDto>(id);
        CombineStatuses(_repository);
        return dto;
    }

    public SelectList<int> AsSelectList(bool hasParent)
    {
        return _repository.AllAsQueryable
                        .AsSelectList(hasParent);
    }

    public HaveId<int> Create(CreateAppealDescriptionDlDto dto)
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

    public void Update(UpdateAppealDescriptionDlDto dto)
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

    private void Validation<TDto>(AppealDescriptionDlDto<TDto> dto, AppealDescription entity)
        where TDto : AppealDescriptionDlDto<TDto>
    {
        var query = _repository.AllAsQueryable;

        if (entity != null)
            query = query.Where(a => a.Id != entity.Id);

        var exsisit = query.Any(ent => ent.Code == dto.Code);
        if (exsisit)
            AddError($"Запись с этим кодом ({dto?.Code}) уже существует.",
                    nameof(dto.Code));

    }
}
