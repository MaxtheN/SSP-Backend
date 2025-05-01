using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using SspUis.DataLayer;
using SspUis.DataLayer.EfClasses;
using SspUis.DataLayer.Repositories;
using StatusGeneric;
using WEBASE;
using WEBASE.Models;
using WEBASE.Utility;

namespace SspUis.BizLogicLayer
{
    public class IndicatorDistrictService : StatusGenericHandler, IIndicatorDistrictService
    {
        protected readonly IIndicatorDistrictRepository _repository;
        private readonly IUnitOfWork _unitOfWork;

        public IndicatorDistrictService(IIndicatorDistrictRepository repository, IUnitOfWork unitOfWork)
        {
            _repository = repository;
            _unitOfWork = unitOfWork;
        }

        public SelectList<int> AsSelectList() => _repository.AllAsQueryable.AsSelectList();

        public HaveId<int> Create(CreateIndicatorDistrictDlDto dto)
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

     

        public void Delete(int id)
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

        public IndicatorDistrictDto Get()
        {
            return new IndicatorDistrictDto();
        }

        public IndicatorDistrictDto GetById(int id)
        {
            var dto = _repository.ById<IndicatorDistrictDto>(id);
            CombineStatuses(_repository);
            return dto;
        }

        public PagedResult<IndicatorDistrictListDto> GetList(SortFilterPageOptions option)
        {
            var result = _repository.ReadAsNoTracked<IndicatorDistrictListDto>().SortFilter(option).AsPagedResult(option);
            return result;
        }

        public void Update(UpdateIndicatorDistrictDlDto dto)
        {
            _repository.Update(dto, ent => Validation(dto, ent));
            CombineStatuses(_repository);
            if (IsValid)
                _unitOfWork.Save();
        }

        //private void Validation(CreateIndicatorDistrictDlDto dto, IndicatorDistrict ent)
        //{
        //    throw new NotImplementedException();
        //}

        private void Validation<TDto>(IndicatorDistrictDlDto<TDto> dto, IndicatorDistrict entity)
        where TDto : IndicatorDistrictDlDto<TDto>
        {
            var query = _repository.AllAsQueryable;

            if (entity != null)
                query = query.Where(a => a.Id != entity.Id);

        }
    }
}
