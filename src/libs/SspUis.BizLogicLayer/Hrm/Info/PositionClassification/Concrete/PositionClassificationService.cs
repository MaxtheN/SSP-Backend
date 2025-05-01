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

namespace SspUis.BizLogicLayer.Hrm.PositionClassificationServices
{
    public class PositionClassificationService : StatusGenericHandler, IPositionClassificationService
    {
        private readonly IPositionClassificationRepository _repository;
        private readonly IUnitOfWork _unitOfWork;
        public PositionClassificationService(IUnitOfWork unitOfWork)
        {
            _repository = unitOfWork.PositionClassificationRepository;
            _unitOfWork = unitOfWork;
        }

        public PagedResult<PositionClassificationListDto> GetList(SortFilterPageOptions dto)
        {
            var result = _repository.ReadAsNoTracked<PositionClassificationListDto>().SortFilter(dto).AsPagedResult(dto);
            return result;
        }

        public PositionClassificationDto Get()
        {
            return new PositionClassificationDto();
        }

        public PositionClassificationDto Get(int id)
        {
            var dto = _repository.ById<PositionClassificationDto>(id);
            CombineStatuses(_repository);
            return dto;
        }

        public SelectList<int> AsSelectList()
        {
            return _repository.AllAsQueryable
                .AsSelectList();
        }

        public HaveId<int> Create(CreatePositionClassificationDlDto dto)
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

        public void Update(UpdatePositionClassificationDlDto dto)
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

        private void Validation<TDto>(PositionClassificationDlDto<TDto> dto, PositionClassification entity)
            where TDto : PositionClassificationDlDto<TDto>
        {
            var query = _repository.AllAsQueryable;

            if (entity != null)
                query = query.Where(a => a.Id != entity.Id);

        }
    }
}
