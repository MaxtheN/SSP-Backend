using System.Linq;
using Microsoft.EntityFrameworkCore;
using SspUis.Core.Security;
using SspUis.DataLayer;
using SspUis.DataLayer.EfClasses.Appeal;
using SspUis.DataLayer.Repositories;
using StatusGeneric;
using WEBASE;
using WEBASE.Models;

namespace SspUis.BizLogicLayer.AppealTypeArriveServices;

public class AppealTypeArriveService : StatusGenericHandler, IAppealTypeArriveService
{
    private readonly IAppealTypeArriveRepository _repository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IAuthService _authService;

    public AppealTypeArriveService(IUnitOfWork unitOfWork, IAuthService authService)
    {
        _repository = unitOfWork.AppealTypeArriveRepository;
        _unitOfWork = unitOfWork;
        _authService = authService;
    }

    public PagedResult<AppealTypeArriveListDto> GetList(SortFilterPageOptions dto)
    {
        var result = _repository.ReadAsNoTracked<AppealTypeArriveListDto>().SortFilter(dto).AsPagedResult(dto);
        return result;
    }

    public AppealTypeArriveDto Get()
    {
        return new AppealTypeArriveDto();
    }

    public AppealTypeArriveDto Get(int id)
    {
        var dto = _repository.ById<AppealTypeArriveDto>(id);
        CombineStatuses(_repository);
        return dto;
    }

    public SelectList<int> AsSelectList()
    {
        return _repository.AllAsQueryable
                        .AsSelectList();
    }

    public HaveId<int> Create(CreateAppealTypeArriveDlDto dto)
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

    public void Update(UpdateAppealTypeArriveDlDto dto)
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

    private void Validation<TDto>(AppealTypeArriveDlDto<TDto> dto, AppealTypeArrive entity)
        where TDto : AppealTypeArriveDlDto<TDto>
    {
        var query = _repository.AllAsQueryable;

        if (entity != null)
            query = query.Where(a => a.Id != entity.Id);

    }
}
