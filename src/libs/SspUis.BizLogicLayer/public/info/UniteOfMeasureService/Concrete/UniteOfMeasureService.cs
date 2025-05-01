using Microsoft.EntityFrameworkCore;
using SspUis.DataLayer;
using SspUis.DataLayer.EfClasses;
using SspUis.DataLayer.Repositories;
using StatusGeneric;
using System.Linq;
using WEBASE;
using WEBASE.Models;

namespace SspUis.BizLogicLayer;

public class UniteOfMeasureService : StatusGenericHandler, IUniteOfMeasureService
{
	protected readonly IUniteOfMeasureRepository _repository;
	private readonly IUnitOfWork _unitOfWork;

	public UniteOfMeasureService(IUniteOfMeasureRepository repository,
		IUnitOfWork unitOfWork)
	{
		_repository = repository;
		this._unitOfWork = unitOfWork;
	}


	public SelectList<int> AsSelectList() =>
		 _repository.AllAsQueryable.AsSelectList();

	public HaveId<int> Create(CreateUniteOfMeasureDlDto dto)
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

	void IUniteOfMeasureService.Delete(int id)
	{
		try
		{
			var entity = GetById(id);
			if (entity != null)
				entity.StateId = StateIdConst.PASSIVE;

			_repository.Update(entity);
			CombineStatuses(_repository);
			if (IsValid)
				_unitOfWork.Save();
		}
		catch (DbUpdateException)
		{
			AddError("������ �� ����� ���� ������");
		}
	}

	public UniteOfMeasureDto Get()
	{
		return new UniteOfMeasureDto();
	}

	public UniteOfMeasureDto GetById(int id)
	{
		var dto = _repository.ById<UniteOfMeasureDto>(id);
		CombineStatuses(_repository);
		return dto;
	}

	public PagedResult<UniteOfMeasureListDto> GetList(SortFilterPageOptions option)
	{
		var result = _repository.ReadAsNoTracked<UniteOfMeasureListDto>().SortFilter(option).AsPagedResult(option);
		return result;
	}

	public void Update(UpdateUniteOfMeasureDlDto dto)
	{
		_repository.Update(dto, ent => Validation(dto, ent));
		CombineStatuses(_repository);
		if (IsValid)
			_unitOfWork.Save();
	}
	private void Validation<TDto>(UniteOfMeasureDlDto<TDto> dto, UniteOfMeasure entity)
		   where TDto : UniteOfMeasureDlDto<TDto>
	{
		var query = _repository.AllAsQueryable;

		if (entity != null)
			query = query.Where(a => a.Id != entity.Id);

	}


}

