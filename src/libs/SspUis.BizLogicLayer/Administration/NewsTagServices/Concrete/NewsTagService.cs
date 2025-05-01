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

namespace SspUis.BizLogicLayer.NewsTagServices
{
    public class NewsTagService : StatusGenericHandler, INewsTagService
    {
        private readonly INewsTagRepository _repository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IAuthService _authService;
        public NewsTagService(IUnitOfWork unitOfWork, IAuthService authService)
        {
            _repository = unitOfWork.NewsTagRepository;
            _unitOfWork = unitOfWork;
            _authService = authService;
        }
        public PagedResult<NewsTagListDto> GetList(SortFilterPageOptions options)
        {
            var result = _repository.ReadAsNoTracked<NewsTagListDto>()
                        .SortFilter(options)
                        .AsPagedResult(options);
            return result;
        }

        public NewsTagDto Get()
        {
            return new NewsTagDto();
        }

        public NewsTagDto Get(int id)
        {
            var dto = _repository.ById<NewsTagDto>(id);
            CombineStatuses(_repository);
            return dto;
        }
        public SelectList<int> AsSelectList()
        {
            return _repository.AllAsQueryable.AsSelectList();
        }

        public HaveId<int> Create(CreateNewsTagDlDto dto)
        {
            var entity = _repository.Create(dto);
            CombineStatuses(_repository);
            if(IsValid)
            {
                _unitOfWork.Save();
                return HaveId.Create(entity.Id);
            }
            return null;
        }

        public void Update(UpdateNewsTagDlDto dto)
        {
            _repository.Update(dto);
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
    }
}
