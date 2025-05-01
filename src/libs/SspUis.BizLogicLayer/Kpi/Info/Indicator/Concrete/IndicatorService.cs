using Microsoft.EntityFrameworkCore;
using SspUis.BizLogicLayer.Hrm.QualificationCategoryServices;
using SspUis.DataLayer;
using SspUis.DataLayer.EfClasses;
using SspUis.DataLayer.Repositories;
using StatusGeneric;
using System;
using System.Linq;
using WEBASE;
using WEBASE.Models;

namespace SspUis.BizLogicLayer;

public class IndicatorService : StatusGenericHandler, IIndicatorService
{
	protected readonly IIndicatorRepository _repository;
	private readonly IUnitOfWork _unitOfWork;

	public IndicatorService(IIndicatorRepository repository,
		IUnitOfWork unitOfWork)
	{
		_repository = repository;
		this._unitOfWork = unitOfWork;
	}


	public SelectList<int> AsSelectList() =>
		 _repository.AllAsQueryable.AsSelectList();

	//public HaveId<int> Create(CreateIndicatorDlDto dto)
	//{
	//	var entity = _repository.Create(dto, ent => Validation(dto, ent));
	//	CombineStatuses(_repository);
	//	if (IsValid)
	//	{
	//		_unitOfWork.Save();
	//		return HaveId.Create(entity.Id);
	//	}
	//	return null;
	//}
	public HaveId<int> Create(CreateIndicatorDlDto dto)
	{
		try
		{
			var entity = _repository.Create(dto, ent => Validation(dto, ent));
			CombineStatuses(_repository);
			if (IsValid)
			{
				_unitOfWork.Save();
				return HaveId.Create(entity.Id);
			}
		}
		catch (Exception ex)
		{
			AddError($"{ex.Message} - {ex.InnerException} - {ex.StackTrace} - {ex.TargetSite} - {ex.Source}");
		}

		return null;
	}

	void IIndicatorService.Delete(int id)
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

	public IndicatorDto Get()
	{
		return new IndicatorDto();
	}

	public IndicatorDto GetById(int id)
	{
		var dto = _repository.ById<IndicatorDto>(id);
		CombineStatuses(_repository);
		return dto;
	}

	public PagedResult<IndicatorListDto> GetList(SortFilterPageOptions option)
	{
		var result = _repository.ReadAsNoTracked<IndicatorListDto>().SortFilter(option).AsPagedResult(option);
		return result;
	}

	public void Update(UpdateIndicatorDlDto dto)
	{
		_repository.Update(dto, ent => Validation(dto, ent));
		CombineStatuses(_repository);
		if (IsValid)
			_unitOfWork.Save();
	}
	private void Validation<TDto>(IndicatorDlDto<TDto> dto, Indicator entity)
		   where TDto : IndicatorDlDto<TDto>
	{
		var query = _repository.AllAsQueryable;

		if (entity != null)
			query = query.Where(a => a.Id != entity.Id);

	}


}

