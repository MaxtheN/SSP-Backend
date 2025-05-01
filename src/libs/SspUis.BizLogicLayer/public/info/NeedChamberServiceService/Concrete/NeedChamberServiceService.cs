using Microsoft.EntityFrameworkCore;
using SspUis.Core;
using SspUis.Core.Security;
using SspUis.DataLayer;
using SspUis.DataLayer.EfClasses;
using SspUis.DataLayer.Repositories;
using StatusGeneric;
using System;
using System.Collections.Generic;
using System.Linq;
using WEBASE;
using WEBASE.Models;
using WEBASE.Storage;

namespace SspUis.BizLogicLayer.Hrm.NeedChamberServiceServices
{
	public class NeedChamberServiceService : StatusGenericHandler, INeedChamberServiceService
	{
		private readonly INeedChamberServiceRepository _repository;
		private readonly IStorageService _storageService;
		private readonly IUnitOfWork _unitOfWork;
		private readonly IAuthService _authService;
		public NeedChamberServiceService(IUnitOfWork unitOfWork, IAuthService authService, IStorageService storageService)
		{
			_repository = unitOfWork.NeedChamberServiceRepository;
			_unitOfWork = unitOfWork;
			_authService = authService;
			_storageService = storageService;
		}
		public PagedResult<NeedChamberServiceListDto> GetList(NeedChamberServiceSortFilterDto dto)
		{
			var result = _repository.ReadAsNoTracked<NeedChamberServiceListDto>()
				.SortFilter(dto)
				.AsPagedResult(dto);

			return result;
		}
		public List<NeedChamberServiceGroupAndChildListDto> GetListGroupAndChild(bool isPaid)
		{
			var needChamber = _unitOfWork.Context.Set<NeedChamberService>().ToList();
			var group = _unitOfWork.Context.Set<NeedChamberServiceGroup>().ToList();

			List<NeedChamberServiceGroupAndChildListDto> result = new();

			for (int i = 0; i < group.Count; i++)
			{

				var groupRespons = new NeedChamberServiceGroupAndChildListDto()
				{
					Id = group[i].Id,
					Code = group[i].Code,
					ShortName = group[i].ShortName,
					FullName = group[i].FullName,
					NeedChambers = new(),
					OrderCode = group[i].OrderCode,
					StateId = group[i].StateId,
				};

				for (int j = 0; j < needChamber.Count; j++)
				{
					if (isPaid)
					{
						if (needChamber[j].NeedChamberServiceGroupId == groupRespons.Id && needChamber[j].ServicePriceTypeId == ServicePriceTypeIdConst.FREE)
						{
							var nedChamberRespons = new NeedChamberServiceListDto()
							{
								Id = needChamber[j].Id,
								Details = needChamber[j].Details,
								Code = needChamber[j].Code,
								FullName = needChamber[j].FullName,
								EmployeeManageId = needChamber[j].EmployeeManageId,
								MeetingTypeId = needChamber[j].MeetingTypeId,
								ShortName = needChamber[j].ShortName,
								NeedChamberServiceGroupId = needChamber[j].NeedChamberServiceGroupId,
								StateId = needChamber[j].StateId,
								IsOffer = needChamber[j].IsOffer,
								ServicePriceTypeId = needChamber[i].ServicePriceTypeId
							};
							groupRespons.NeedChambers.Add(nedChamberRespons);
						}
						else continue;
					}
					else
					{
						if (needChamber[j].NeedChamberServiceGroupId == groupRespons.Id && needChamber[j].ServicePriceTypeId != ServicePriceTypeIdConst.FREE)
						{
							var nedChamberRespons = new NeedChamberServiceListDto()
							{
								Id = needChamber[j].Id,
								Details = needChamber[j].Details,
								Code = needChamber[j].Code,
								FullName = needChamber[j].FullName,
								EmployeeManageId = needChamber[j].EmployeeManageId,
								MeetingTypeId = needChamber[j].MeetingTypeId,
								ShortName = needChamber[j].ShortName,
								NeedChamberServiceGroupId = needChamber[j].NeedChamberServiceGroupId,
								StateId = needChamber[j].StateId,
								IsOffer = needChamber[j].IsOffer,
								ServicePriceTypeId = needChamber[i].ServicePriceTypeId
							};
							groupRespons.NeedChambers.Add(nedChamberRespons);
						}
						else continue;
					}
				}
				result.Add(groupRespons);
			}
			return result;
		}


		public NeedChamberServiceDto Get()
		{
			return new NeedChamberServiceDto();
		}

		public NeedChamberServiceDto Get(int id)
		{
			var dto = _repository.ById<NeedChamberServiceDto>(id);
			CombineStatuses(_repository);
			return dto;
		}

		public SelectList<int> AsSelectList(int? groupId)
		{
			return _repository.AllAsQueryable
				.Where(x => !groupId.HasValue || x.NeedChamberServiceGroupId == groupId)
				.AsSelectList();
		}

		public Dictionary<int, bool> WithIsOfferta()
		{
			return _unitOfWork.Context.Set<NeedChamberService>()
				.Where(s => s.ServicePriceTypeId == ServicePriceTypeIdConst.PERCENTAGE_CONTRACT_SIZE
						 || s.ServicePriceTypeId == ServicePriceTypeIdConst.BXM)
				.ToDictionary(x => x.Id, x => x.IsOffer);
		}

		public HaveId<int> Create(CreateNeedChamberServiceDlDto dto)
		{
			using (var transaction = _unitOfWork.BeginTransaction())
			{
				try
				{
					var entity = _repository.Create(dto, ent => Validation(dto, ent));
					CombineStatuses(_repository);

					if (HasErrors)
					{
						transaction.Rollback();
						return null!;
					}

					_unitOfWork.Save();
					if (dto.Files is not null)
						_storageService.MoveToPersistent(
							DocumentStorageConst.INFO_NEED_CHAMBER_SERVICE_FILES,
							entity.Id.ToString(),
							dto.Files.Select(f => f.Id).ToArray());

					if (HasErrors) return null;

					if (IsValid)
						transaction.Commit();

					return HaveId.Create(entity.Id);
				}
				catch (Exception ex)
				{
					AddError($"{ex.Message} : {ex.InnerException}");
				}
			}
			return null;
		}

		public void Update(UpdateNeedChamberServiceDlDto dto)
		{
			using (var transaction = _unitOfWork.BeginTransaction())
			{
				var entity = _repository.Update(dto, ent => Validation(dto, ent));

				CombineStatuses(_repository);
				if (IsValid)
					_unitOfWork.Save();

				_storageService.ResolveMarkedFiles(
					DocumentStorageConst.INFO_NEED_CHAMBER_SERVICE_FILES,
					entity.Id.ToString());

				if (IsValid)
				{
					transaction.Commit();
				}
			}
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

		public List<NeedChamberServiceGroupingDto> GroupingByFreeServices(int? groupId)
		{
			List<NeedChamberServiceGroupingDto> res = new();

			var freeServices = _unitOfWork.Context.Set<NeedChamberService>()
				.Include(x => x.Translates)
				.IsActive()
				.Where(x => x.ServicePriceTypeId == ServicePriceTypeIdConst.FREE);

			var groups = _unitOfWork.Context.Set<NeedChamberServiceGroup>()
				.Include(x => x.Translates)
				.Where(x => !groupId.HasValue || groupId == x.Id)
				.IsActive().ToList();

			groups.ForEach(group =>
			{
				var needGroupDto = new NeedChamberServiceGroupingDto
				{
					Id = 0,
					GroupId = group.Id,
					Group = group.Translates.AsQueryable().FirstOrDefault(NeedChamberServiceGroupTranslate.GetExpr(
								TranslateColumn.full_name, ServiceProvider.CultureHelper.CurrentCulture.Id))
								?.TranslateText ?? group.FullName,

					Tables = freeServices.Where(x => x.NeedChamberServiceGroupId == group.Id)
						.Select(x => new NeedChamberServiceGroupingTableListDto
						{
							Id = 0,
							NeedChamberServiceId = x.Id,
							NeedChamberService = x.Translates.AsQueryable().FirstOrDefault(NeedChamberServiceTranslate.GetExpr(
								TranslateColumn.full_name, ServiceProvider.CultureHelper.CurrentCulture.Id))
								.TranslateText ?? x.FullName,
							ServicePriceTypeId = x.ServicePriceTypeId,
							ServicePriceType = x.ServicePriceType.Translates.AsQueryable().FirstOrDefault(ServicePriceTypeTranslate.GetExpr(
								TranslateColumn.full_name, ServiceProvider.CultureHelper.CurrentCulture.Id))
								.TranslateText ?? x.ServicePriceType.FullName,
						})
						.ToList()
				};

				res.Add(needGroupDto);
			});

			return res;
		}
		private void Validation<TDto>(NeedChamberServiceDlDto<TDto> dto, NeedChamberService entity)
			where TDto : NeedChamberServiceDlDto<TDto>
		{
			var query = _repository.AllAsQueryable;

			if (entity != null)
				query = query.Where(a => a.Id != entity.Id);

		}

		#region Files
		public IEnumerable<NeedChamberServiceFileDto> UploadFiles(params StorageFile[] files)
		{
			if (files.Count() == 0)
			{
				AddError("Файл не прикреплен");
				return null;
			}

			var result = _storageService
				.SaveTemp(DocumentStorageConst.INFO_NEED_CHAMBER_SERVICE_FILES, files)
				.Select(a => new NeedChamberServiceFileDto
				{
					Id = a.FileId,
					FileName = a.FileName
				});
			CombineStatuses(_storageService);
			return IsValid ? result : null;
		}

		public StorageFile DownloadFile(Guid fileId)
		{
			var entity = _unitOfWork.Context.Set<NeedChamberServiceFile>().FirstOrDefault(a => a.Id == fileId);
			return Download(fileId, entity, DocumentStorageConst.INFO_NEED_CHAMBER_SERVICE_FILES);
		}

		public void DeleteFile(Guid fileId)
		{
			var entity = _unitOfWork.Context
				.Set<NeedChamberServiceFile>()
				.FirstOrDefault(a => a.Id == fileId);

			Delete(fileId, entity, DocumentStorageConst.INFO_NEED_CHAMBER_SERVICE_FILES);
		}

		private void Delete(Guid fileId, FileEntity<int> entity, string storageDocument)
		{
			if (entity == null)
			{
				_storageService.DeleteTemp(storageDocument, fileId);
				CombineStatuses(_storageService);
			}
		}

		private StorageFile Download(Guid fileId, FileEntity<int> entity, string storageDocument)
		{
			StorageFile file;

			if (entity == null)
			{
				file = _storageService.GetTempFile(storageDocument, fileId);
				CombineStatuses(_storageService);
			}
			else
			{
				file = _storageService.GetFile(storageDocument, entity.OwnerId.ToString(), fileId);
				CombineStatuses(_storageService);

				if (IsValid)
					file.FileName = entity.FileName;
			}

			return file;
		}
		#endregion
	}
}
