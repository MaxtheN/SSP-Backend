using Microsoft.EntityFrameworkCore;
using SspUis.BizLogicLayer.Hrm.QualificationCategoryServices;
using SspUis.DataLayer;
using SspUis.DataLayer.EfClasses;
using SspUis.DataLayer.Repositories;
using StatusGeneric;
using System.Linq;
using WEBASE;
using WEBASE.Models;

namespace SspUis.BizLogicLayer;

public class IndicatorDepartmentService : StatusGenericHandler, IIndicatorDepartmentService
{
	protected readonly IIndicatorDepartmentRepository _repository;
	private readonly IUnitOfWork _unitOfWork;

	public IndicatorDepartmentService(IIndicatorDepartmentRepository repository,
		IUnitOfWork unitOfWork)
	{
		_repository = repository;
		this._unitOfWork = unitOfWork;
	}


	public SelectList<int> AsSelectList() =>
		 _repository.AllAsQueryable.AsSelectList();

	public HaveId<int> Create(CreateIndicatorDepartmentDlDto dto)
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

	void IIndicatorDepartmentService.Delete(int id)
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

	public IndicatorDepartmentDto Get()
	{
		return new IndicatorDepartmentDto();
	}

	public IndicatorDepartmentDto GetById(int id)
	{
		var dto = _repository.ById<IndicatorDepartmentDto>(id);
		CombineStatuses(_repository);
		return dto;
	}

	public PagedResult<IndicatorDepartmentListDto> GetList(SortFilterPageOptions option)
	{
		var result = _repository.ReadAsNoTracked<IndicatorDepartmentListDto>().SortFilter(option).AsPagedResult(option);
		return result;
	}

	public void Update(UpdateIndicatorDepartmentDlDto dto)
	{
		_repository.Update(dto, ent => Validation(dto, ent));
		CombineStatuses(_repository);
		if (IsValid)
			_unitOfWork.Save();
	}
	private void Validation<TDto>(IndicatorDepartmentDlDto<TDto> dto, IndicatorDepartment entity)
		   where TDto : IndicatorDepartmentDlDto<TDto>
	{
		var query = _repository.AllAsQueryable;

		if (entity != null)
			query = query.Where(a => a.Id != entity.Id);

	}


}

