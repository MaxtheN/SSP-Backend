
using SspUis.DataLayer;
using StatusGeneric;

using SspUis.Core.Security;
using SspUis.DataLayer.Repositories;
using WEBASE.Models;
using SspUis.DataLayer.EfClasses;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using WEBASE;

namespace SspUis.BizLogicLayer.Memship;

public class ContractorRatingService : StatusGenericHandler, IContractorRatingService
{
    private readonly IContractorRatingRepository _repository;
    private readonly IUnitOfWork _unitOfWork;
    

    public ContractorRatingService(IUnitOfWork unitOfWork)
    {
        _repository = unitOfWork.ContractorRatingRepository;
        _unitOfWork = unitOfWork;
        
    }

    public SelectList<int> AsSelectList() =>
         _repository.AllAsQueryable.AsSelectList();

    public HaveId<int> Create(CreateContractorRatingDlDto dto)
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
            AddError("������ �� ����� ���� ������");
        }
    }

    public ContractorRatingDto Get()
    {
        return new ContractorRatingDto();
    }
    public ContractorRatingDto GetById(int id)
    {
        var dto = _repository.ById<ContractorRatingDto>(id);
        CombineStatuses(_repository);
        return dto;
    }
    public PagedResult<ContractorRatingListDto> GetList(SortFilterPageOptions dto)
    {
        var result = _repository.ReadAsNoTracked<ContractorRatingListDto>().SortFilter(dto).AsPagedResult(dto);
        return result;
    }

    public void Update(UpdateContractorRatingDlDto dto)
    {
        _repository.Update(dto, ent => Validation(dto, ent));
        CombineStatuses(_repository);
        if (IsValid)
            _unitOfWork.Save();
    }

    private void Validation<TDto>(ContractorRatingDlDto<TDto> dto, ContractorRating entity)
           where TDto : ContractorRatingDlDto<TDto>
    {
        var query = _repository.AllAsQueryable;

        if (entity != null)
            query = query.Where(a => a.Id != entity.Id);

    }
}
