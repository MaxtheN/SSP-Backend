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

namespace SspUis.BizLogicLayer.TagServices
{
    public class TagService : StatusGenericHandler, ITagService
    {
        private readonly ITagRepository _repository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IAuthService _authService;
        public TagService(IUnitOfWork unitOfWork, IAuthService authService)
        {
            _repository = unitOfWork.TagRepository;
            _unitOfWork = unitOfWork;
            _authService = authService;
        }
        public PagedResult<TagListDto> GetList(SortFilterPageOptions options)
        {
            var result = _repository.ReadAsNoTracked<TagListDto>()
                        .SortFilter(options)
                        .AsPagedResult(options);
            return result;
        }

        public TagDto Get()
        {
            return new TagDto();
        }

        public TagDto Get(int id)
        {
            var dto = _repository.ById<TagDto>(id);
            CombineStatuses(_repository);
            return dto;
        }
        public SelectList<int> AsSelectList()
        {
            return _repository.AllAsQueryable.AsSelectList();
        }

        public HaveId<int> Create(CreateTagDlDto dto)
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

        public void Update(UpdateTagDlDto dto)
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
