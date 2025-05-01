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

namespace SspUis.BizLogicLayer.Hrm.ItemOfExpenseServices
{
    public class ItemOfExpenseService : StatusGenericHandler, IItemOfExpenseService
    {
        private readonly IItemOfExpenseRepository _repository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IAuthService _authService;

        public ItemOfExpenseService(IUnitOfWork unitOfWork, IAuthService authService)
        {
            _repository = unitOfWork.ItemOfExpenseRepository;
            _unitOfWork = unitOfWork;
            _authService = authService;
        }

        public PagedResult<ItemOfExpenseListDto> GetList(SortFilterPageOptions dto)
        {
            var result = _repository.ReadAsNoTracked<ItemOfExpenseListDto>().SortFilter(dto).AsPagedResult(dto);
            return result;
        }

        public ItemOfExpenseDto Get()
        {
            return new ItemOfExpenseDto();
        }

        public ItemOfExpenseDto Get(int id)
        {
            var dto = _repository.ById<ItemOfExpenseDto>(id);
            CombineStatuses(_repository);
            return dto;
        }

        public SelectList<int> AsSelectList(int? itemOfExpenseId = null)
        {
            return _repository.AllAsQueryable
                .AsSelectList(itemOfExpenseId);
        }

        public HaveId<int> Create(CreateItemOfExpenseDlDto dto)
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

        public void Update(UpdateItemOfExpenseDlDto dto)
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

        private void Validation<TDto>(ItemOfExpenseDlDto<TDto> dto, ItemOfExpense entity)
            where TDto : ItemOfExpenseDlDto<TDto>
        {
            var query = _repository.AllAsQueryable;

            if (entity != null)
                query = query.Where(a => a.Id != entity.Id);

        }
    }
}
