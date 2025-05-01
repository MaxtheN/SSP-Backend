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
using WEBASE.Storage;
using SspUis.Core;
using System.IO;
using Microsoft.EntityFrameworkCore;
using SspUis.BizLogicLayer.Models;

namespace SspUis.BizLogicLayer.NewsServices
{
    public class NewsService : StatusGenericHandler, INewsService
    {
        private readonly INewsRepository _repository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IAuthService _authService;
        private readonly IStorageService _storageService;
        private readonly ICrudServices _service;
        public NewsService(IUnitOfWork unitOfWork, IAuthService authService, IStorageService storageService)
        {
            _repository = unitOfWork.NewsRepository;
            _unitOfWork = unitOfWork;
            _authService = authService;
            _storageService = storageService;
        }

        public PagedResult<NewsListDto> GetList(WEBASE.TableSortFilterPageOptions options)
        {
            var result = _repository.ReadAsNoTracked<NewsListDto>()
                        .SortFilter(options)
                        .ToTableData(options);
            return result;
        }

        public PagedResult<NewsListDto> GetListByTag(string tag, SortFilterPageOptions options)
        {
            var result = _repository.ReadAsNoTracked<NewsListDto>(a => a.Tags.Any(s => s.Tag.Name == tag))
                        .SortFilter(options)
                        .AsPagedResult(options);
            return result;
        }

        public NewsDto Get()
        {
            return new NewsDto();
        }

        public NewsDto Get(int id)
        {
            var dto = _repository.ById<NewsDto>(id);
            CombineStatuses(_repository);
            return dto;
        }

        public NewsViewDto GetForView(int id)
        {
            var dto = _repository.ById<NewsViewDto>(id);
            _repository.AddViewCount(id);
            _unitOfWork.Save();
            return dto;
        }

        public List<TagDto> GetTags()
        {
            return _unitOfWork.TagRepository.ReadAsNoTracked<TagDto>(a => a.StateId == StateIdConst.ACTIVE).ToList();
        }

        public SelectList<int> AsSelectList()
        {
            return _repository.AllAsQueryable.AsSelectList();
        }

        public HaveId<int> Create(CreateNewsDlDto dto)
        {
            using (var transaction = _unitOfWork.BeginTransaction())
            {
                try
                {
                    ResolveTags(dto);

                    var entity = _repository.Create(dto);

                    CombineStatuses(_repository);
                    if (HasErrors)
                    {
                        transaction.Rollback();
                        return null;
                    }
                    _unitOfWork.Save();

                    if (dto.Image != null)
                    {
                        _storageService.MoveToPersistent(DocumentStorageConst.NEWS_IMAGE, $"{entity.Id}", dto.Image.Id);
                        CombineStatuses(_storageService);

                        if (HasErrors)
                        {
                            transaction.Rollback();
                            return null;
                        }
                    }

                    transaction.Commit();
                    return HaveId.Create(entity.Id);
                }
                catch
                {
                    transaction.Rollback();
                    throw;
                }
            }
        }

        private void ResolveTags<T>(NewsDlDto<T> dto)
            where T : NewsDlDto<T>
        {
            var existingTags = _unitOfWork.TagRepository.ByNames<TagDto>(dto.Tags.Select(a => a.Name).ToList()).ToList();

            var newTagNames = dto.Tags.Where(a => !existingTags.Any(s => s.Name == a.Name));
            var newTags = newTagNames.Select(a => new Tag
            {
                Name = a.Name,
                StateId = StateIdConst.ACTIVE
            }).ToList();
            _unitOfWork.TagRepository.DbSet.AddRange(newTags);
            CombineStatuses(_unitOfWork.TagRepository);
            if (HasErrors)
            {
                throw new Exception(Message);
            }
            _unitOfWork.Save();
            dto.Tags.Clear();
            dto.Tags.AddRange(existingTags);
            var tagIds = newTags.Select(a => a.Id).ToArray();
            dto.Tags.AddRange(_unitOfWork.TagRepository.ReadAsNoTracked<TagDto>(a => tagIds.Contains(a.Id)));
        }

        public void Update(UpdateNewsDlDto dto)
        {
            using (var transaction = _unitOfWork.BeginTransaction())
            {
                try
                {
                    ResolveTags(dto);

                    var entity = _repository.Update(dto);

                    CombineStatuses(_repository);
                    if (HasErrors)
                    {
                        transaction.Rollback();
                        return;
                    }
                    _unitOfWork.Save();

                    if (dto.Image != null)
                    {
                        _storageService.ResolveMarkedFiles(DocumentStorageConst.NEWS_IMAGE, $"{entity.Id}");
                        CombineStatuses(_storageService);

                        if (HasErrors)
                        {
                            transaction.Rollback();
                            return;
                        }
                    }

                    transaction.Commit();
                }
                catch
                {
                    transaction.Rollback();
                    throw;
                }
            }
        }
        public IStorageFileInfo UploadNewsImage(StorageFile file)
        {
            var result = _storageService.SaveTemp(DocumentStorageConst.NEWS_IMAGE, file);
            CombineStatuses(_storageService);
            return IsValid ? result.First() : null;
        }
        public (byte[], string)? GetNewsImage(Guid newsImageId)
        {
            var entity = _unitOfWork.Context.Set<NewsImage>().Include(a => a.Owner).FirstOrDefault(a => a.Id == newsImageId);
            if (entity != null)
            {
                var tempFile = _storageService.GetFile(DocumentStorageConst.NEWS_IMAGE, entity.Owner.Id.ToString(), newsImageId);
                CombineStatuses(_storageService);
                if (HasErrors)
                    return null;
                return (((MemoryStream)tempFile.GetStream()).ToArray(), Path.GetExtension(tempFile.FileName));
            }
            else
            {
                AddError("Изображение не найдено");
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
                AddError("Запись не может быть удален");
            }
        }
    }
}
