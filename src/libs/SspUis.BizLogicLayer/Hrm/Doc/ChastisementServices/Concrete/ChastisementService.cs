using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;
using OpenXmlPowerTools;
using SspUis.BizLogicLayer.DocumentChangeLogServices;
using SspUis.BizLogicLayer.Srv;
using SspUis.Core;
using SspUis.Core.Configurations;
using SspUis.Core.Security;
using SspUis.DataLayer;
using SspUis.DataLayer.EfClasses;
using SspUis.DataLayer.EfClasses.Hrm;
using SspUis.DataLayer.Repositories.Hrm;
using SspUis.Integration.DocxToPdf;
using SspUis.ServiceLayer.NumberServices;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using WbImzo.Models;
using WbImzo.Proxy.Sdk;
using WEBASE;
using WEBASE.Integration.EImzo;
using WEBASE.Models;
using WEBASE.OfficeTools;
using WEBASE.OfficeTools.Factory;
using WEBASE.OfficeTools.Handlers;
using WEBASE.Storage;

namespace SspUis.BizLogicLayer.Hrm;

public class ChastisementService
	: BaseEntityService<long, Chastisement, ChastisementListDto, ChastisementDto, CreateChastisementDlDto, UpdateChastisementDlDto, IChastisementRepository, ChastisementSortFilterOptions>
	, IChastisementService
{
	private readonly IDocumentChangeLogService _documentChangeLogService;
	private readonly IChastisementRepository _repository;
	private readonly IAuthService _authService;
	private readonly IUnitOfWork _unitOfWork;
	private readonly INumberService _numberService;
	private readonly IEImzoService _eImzoService;
	private readonly IStorageService _storageService;
	private readonly IConvertService _pdfConverter;
    private readonly IWbImzoService _wbImzoService;
    private readonly WbImzoConfig _wbImzoConfig;
    private readonly LinkConfig _linkConfig;

    public ChastisementService(IUnitOfWork unitOfWork, IAuthService authService,
		INumberService numberService,
		IEImzoService eImzoService,
		IStorageService storageService,
		IDocumentChangeLogService documentChangeLogService,
        WbImzoConfig wbImzoConfig,
        List<LinkConfig> linkConfigs,
        IWbImzoService wbImzoService,
        IConvertService pdfConverter) : base(unitOfWork)
	{
		this._repository = unitOfWork.ChastisementRepository;
		this._unitOfWork = unitOfWork;
		this._documentChangeLogService = documentChangeLogService;
		this._numberService = numberService;
		_authService = authService;
		_eImzoService = eImzoService;
		_storageService = storageService;
		_pdfConverter = pdfConverter;
        _wbImzoService = wbImzoService;
        _wbImzoConfig = wbImzoConfig;
        _linkConfig = linkConfigs.FirstOrDefault(config => config.Name == "Chastisement");
    }
	public PagedResult<ChastisementListDto> GetList(ChastisementSortFilterOptions options)
	{
		var result = _repository.ReadAsNoTracked<ChastisementListDto>().Where(a =>
			new int[]
			{
				StatusIdConst.CREATED,
				StatusIdConst.CANCELED,
				StatusIdConst.ACCEPTED,
				StatusIdConst.MODIFIED,
				StatusIdConst.SIGNED,
				StatusIdConst.SIGNING
			}
			.Contains(a.StatusId))
			.SortFilter(options)
			.AsPagedResult(options);

		return result;
	}

	public SelectList<long> AsSelectList(int? employeeId = null)
	{
		return _repository.AllAsQueryable
			.Include(a => a.Tables)
			.Where(a => employeeId.HasValue ? a.Tables.Any(a => a.EmployeeId == employeeId.Value) : true && a.StatusId == StatusIdConst.ACCEPTED)
						.AsSelectList();
	}
	public PagedResult<ChastisementListDto> GetListForSigner(ChastisementSortFilterOptions dto)
	{
		var result = _repository.ReadAsNoTracked<ChastisementListDto>(q =>
					q.Signer.Any(a => a.EmployeeManageId == _authService.User.EmployeeManageId)
				&& (new int[] { StatusIdConst.ACCEPTED, StatusIdConst.SIGNED, StatusIdConst.SIGNING }).Contains(q.StatusId))
		   .SortFilter(dto)
		   .AsPagedResult(dto);

		return result;
	}
	//public SelectList<long> GetTableAsSelectList(long ownerId, long? employeeId)
	//{
	//    return _repository.Context.Set<ChastisementTable>()
	//        .Include(a => a.Owner)
	//        .Include(a => a.Employee).ThenInclude(a => a.Person)
	//        .GetTableAsSelectList(ownerId, employeeId);
	//}
	public ChastisementDto Get()
	{
		return new ChastisementDto
		{
			DocOn = DateOnly.FromDateTime(DateTime.Now),
			DocNumber = _numberService.GetNext(NumberTemplateDocumentConst.DOC_EMPLOYEE_lEAVE_ORDER, 1).Item2
		};
	}

	public ChastisementDto Get(long id)
	{
		var dto = _repository.ById<ChastisementDto>(id);
		CombineStatuses(_repository);
		if (IsValid)
		{
			var nextSigner = _unitOfWork.Context.Set<ChastisementSigner>()
					.Where(a => a.OwnerId == dto.Id && !a.SignedAt.HasValue)
					.OrderBy(a => a.SignOrder).FirstOrDefault();

			dto.CanSign = StatusIdConst.CanApplyChastisement(dto.StatusId, StatusIdConst.SIGNING) && _authService.HasPermission(ModuleCode.ChastisementSign) && (nextSigner != null && nextSigner.EmployeeManageId == _authService.User.EmployeeManageId);
			dto.CanModify = StatusIdConst.CanApplyChastisement(dto.StatusId, StatusIdConst.MODIFIED) && _authService.HasPermission(ModuleCode.ChastisementEdit);
			dto.CanAccept = StatusIdConst.CanApplyChastisement(dto.StatusId, StatusIdConst.ACCEPTED) && _authService.HasPermission(ModuleCode.ChastisementAccept);
			dto.CanCancel = StatusIdConst.CanApplyChastisement(dto.StatusId, StatusIdConst.NOT_ACCEPTED) && _authService.HasPermission(ModuleCode.ChastisementCancel);
			dto.CanDelete = StatusIdConst.CanApplyChastisement(dto.StatusId, StatusIdConst.DELETED) && _authService.HasPermission(ModuleCode.ChastisementDelete);
		}
		return dto;
	}

	public async ValueTask<HaveId<long>> Create(CreateChastisementDlDto dto)
	{
		using (var transaction = UnitOfWork.BeginTransaction())
		{
			try
			{
				var entity = Repository.Create(dto, ent => Validation(dto, ent));
				CombineStatuses(Repository);

				if (!IsValid)
				{
					transaction.Rollback();
					return null;
				}

				UnitOfWork.Save();
                _storageService.MoveToPersistent(DocumentStorageConst.DOC_CHASTISEMENT, entity.Id.ToString(), dto.Files.Select(a => a.Id).ToArray());

                var res = CreateDocumentChangeLog(entity.Id, entity.StatusId, "CreateChastisement");

				if (res == null)
				{
					transaction.Rollback();
					return null;
				}

				transaction.Commit();

                //var model = _unitOfWork.Context.Set<Chastisement>()
                //                                  .Include(a => a.Signer)
                //                                      .ThenInclude(a => a.EmployeeManage)
                //                                          .ThenInclude(em => em.Employee)
                //                                              .ThenInclude(e => e.Organization)
                //                                  .Include(a => a.Signer)
                //                                      .ThenInclude(a => a.EmployeeManage)
                //                                          .ThenInclude(em => em.Employee)
                //                                              .ThenInclude(e => e.Person)
                //                                  .FirstOrDefault(a => a.Id == entity.Id);

                //await PostToIMZOAndSentUrl(model);

                return HaveId.Create(entity.Id);
			}
			catch (DbUpdateException e)
			{
				transaction.Rollback();
				AddError(e.Message);
				if (e.InnerException != null) AddError(e.InnerException.Message);
			}
			catch (Exception ex)
			{
				transaction.Rollback();
				AddError(ex.Message);
			}
			return null;
		}
	}

    public async ValueTask<string> PostToIMZOAndSentUrl(Chastisement contract)
    {
        var canDispose = _unitOfWork.CurrentTransaction == null;
        var transaction = _unitOfWork.CurrentTransaction ?? _unitOfWork.BeginTransaction();

        WbImzoCreateSignRequestDto signRequestCreateDto = CreatDtoForRequestSign(contract);


        foreach (var signer in contract.Signer)
        {

            if (contract.Signer.Count == signer.SignOrder)
            {

                signRequestCreateDto.SignRequestUsers.Add(
                                                        new WbImzoCreateSignRequestUserDto
                                                        {
                                                            UserKey = !string.IsNullOrEmpty(signer.EmployeeManage.Employee.Organization.Inn) ? signer.EmployeeManage.Employee.Organization.Inn : signer.EmployeeManage.Employee.Person.Pinfl,
                                                            UserInfo = signer.EmployeeManage.Employee.Person.FullName,
                                                            UserId = (int)contract.CreatedUserId,
                                                            DocStatusId = StatusIdConst.SIGNED,
                                                            SignPriority = signer.SignOrder,
                                                            IpAddress = _authService.UserIp,
                                                            UserAgent = _authService.UserAgent,
                                                            UserPhoneNumber = signer.EmployeeManage.Employee.Person.PassportNumber,
                                                        }
                                                    );
            }

            else
            {
                signRequestCreateDto.SignRequestUsers.Add(
                                                        new WbImzoCreateSignRequestUserDto
                                                        {
                                                            UserKey = !string.IsNullOrEmpty(signer.EmployeeManage.Employee.Person.Inn) ? signer.EmployeeManage.Employee.Person.Inn : signer.EmployeeManage.Employee.Person.Pinfl,
                                                            UserInfo = signer.EmployeeManage.Employee.Person.FullName,
                                                            UserId = (int)contract.CreatedUserId,
                                                            DocStatusId = StatusIdConst.SIGNING,
                                                            SignPriority = signer.SignOrder,
                                                            IpAddress = _authService.UserIp,
                                                            UserAgent = _authService.UserAgent,
                                                            UserPhoneNumber = signer.EmployeeManage.Employee.Person.PassportNumber,
                                                        }
                                                    );
            }
        }

        if (contract.WebImzoSecretKey != null)
            return await SendUrl(contract.Id);

        var wbImzoResult = await _wbImzoService.UpsertStringFilePrintableLinkRequestAsync(signRequestCreateDto);
        if (!wbImzoResult.IsSuccess && wbImzoResult.Response is null)
            CombineStatuses(wbImzoResult.GetStatusGeneric());

        if (HasErrors || wbImzoResult.Response is null)
        {
            AddError("Intizomiy jazo qollash buyuruqni imzolash uchun yuborilayotgan jarayonda xatolik yuz berdi");
            return null;
        }

        try
        {
            contract.WebImzoRequestId = wbImzoResult.Response.RequestId;
            contract.WebImzoSecretKey = wbImzoResult.Response.SecretKey;

            _unitOfWork.Save();
            CombineStatuses(this);
            if (canDispose)
                transaction.Commit();
        }
        catch (Exception ex)
        {
            AddError("Document did not update" + ex.Message);
        }

        return await SendUrl(contract.Id);
    }

    public async ValueTask<string?> SendUrl(long contractId)
    {

        var contract = _unitOfWork.Context.Set<Chastisement>()
                                                          .Include(a => a.Signer)
                                                              .ThenInclude(a => a.EmployeeManage)
                                                                  .ThenInclude(em => em.Employee)
                                                                      .ThenInclude(e => e.Organization)
                                                          .Include(a => a.Signer)
                                                              .ThenInclude(a => a.EmployeeManage)
                                                                  .ThenInclude(em => em.Employee)
                                                                      .ThenInclude(e => e.Person)
                                                          .FirstOrDefault(a => a.Id == contractId);


        var entity = _repository.ById(contractId);

        if (entity.Signer.Count() > 0)
        {
            var nextSigner = _unitOfWork.Context.Set<ChastisementSigner>()
                                                                 .Where(a => a.OwnerId == contractId)
                                                                 .OrderBy(a => a.SignOrder);
            if (nextSigner == null)
                AddError("Imzolovchi topilmadi");
            else if (!nextSigner.Any(a => a.EmployeeManageId != _authService.User.EmployeeManageId))
                AddError("Sizda imzolash huquqi yo'q");
            if (HasErrors)
                return null;

            if (contract.WebImzoRequestId == null || contract.WebImzoSecretKey.IsNullOrEmpty())
            {
                await PostToIMZOAndSentUrl(contract);
            }

            var url = (_wbImzoConfig.Api.Contains("api/") ? _wbImzoConfig.Api.Replace("api/", null) : _wbImzoConfig.Api) + $"app?requestId={contract.WebImzoRequestId}&secretKey={contract.WebImzoSecretKey}";

            return url;
        }

        return null;
    }

    public WbImzoCreateSignRequestDto CreatDtoForRequestSign(Chastisement contract)
    {
        string documentDataAsString = JsonConvert.SerializeObject(ConvertToDto(contract));
        var filePrintableLink = $"{_linkConfig.Api}{_linkConfig.DownloadRoute}?id2={contract.Id2}";

        return new WbImzoCreateSignRequestDto
        {
            DocumentId = contract.Id,
            ApiKey = _wbImzoConfig.ApiKey,
            Title = contract.DocNumber,
            IsForceCreate = false,
            TableId = TableIdConst.HRM__DOC_CHASTISEMENT,
            SignData = documentDataAsString,
            OrganizationInn = contract.Organization.Inn,
            OrganizationName =  contract.Organization.FullName,
            PrintableLink = filePrintableLink,
            SignRequestActionTypes = new()
            {
                new WbImzoCreateSignRequestActionTypeDto
                {
                    ActionTypeId =  StatusIdConst.SIGNED,
                    ActionTypeName = "SIGNED",
                        Translates = new()
                        {
                            new ActionTranslateTypeDto
                            {
                                LanguageCode = LanguageCodeConst.RU,
                                Name =  "Я согласен"
                            },
                           new ActionTranslateTypeDto
                           {
                                LanguageCode = LanguageCodeConst.UZ_LATN,
                                Name = "Roziman"
                           },
                           new ActionTranslateTypeDto
                           {
                                LanguageCode = LanguageCodeConst.UZ_CYRL,
                                Name = "Розиман"
                           },
                        }
                },
            },
            SignatureMethodIds = new List<int> { SignatureMethodIdConst.E_IMZO },
            TemplateId = 28
        };
    }
    public WebImzoDto ConvertToDto(Chastisement dto)
    => new()
    {
        DocOn = dto.DocOn,
        StatusId = dto.StatusId,
        DocNumber = dto.DocNumber,
        Id = dto.Id,
        OrganizationId = dto.OrganizationId,
    };

    public override void Update(UpdateChastisementDlDto dto)
	{
		using (var transaction = UnitOfWork.BeginTransaction())
		{
			try
			{
				var entity = Repository.Update(dto, ent => Validation(dto, ent));
				CombineStatuses(Repository);
				if (IsValid)
				{
					UnitOfWork.Save();
                    _storageService.ResolveMarkedFiles(DocumentStorageConst.DOC_CHASTISEMENT, dto.Id.ToString());

                    var res = CreateDocumentChangeLog(entity.Id, entity.StatusId, "UpdateChastisement");
					transaction.Commit();
				}
			}
			catch (DbUpdateException e)
			{
				AddError(e.Message);
				if (e.InnerException != null)
					AddError(e.InnerException.Message);
				transaction.Rollback();
			}
		}
	}
	public void Accept(UpdateStatusChastisementDto dTo, bool isLoged = false)
	{
		var canCommit = _unitOfWork.Context.Database.CurrentTransaction == null;
        using (var transaction = canCommit ? _unitOfWork.BeginTransaction() : _unitOfWork.CurrentTransaction)
		{
			try
			{
				if (isLoged)
				{
					Repository.AllAsQueryable.Lock(dTo.Id);
				}

				var entity = Repository.AllAsQueryable.Include(x => x.Tables).FirstOrDefault(x => x.Id == dTo.Id);
				if (entity == null)
				{ AddError("Not found"); return; }

				var dto = new UpdateStatusChastisementDlDto { Id = dTo.Id, StatusId = StatusIdConst.ACCEPTED };

				UpdateStatus(dto, ent =>
				{
					if (!StatusIdConst.CanApplyChastisement(ent.StatusId, dto.StatusId))
					{
						_repository.AddError("Нет доступа");
					}

					bool isDeleted = _unitOfWork.Context.Set<EmployeeManage>()
						.Where(a => ent.Tables.Select(a => a.EmployeeManageId).Contains(a.Id))
						.Any(a => a.IsDeleted);

					if (isDeleted)
					{
						_repository.AddError("Сотрудник был удален");
					}
				});
				
				if (HasErrors)
				{
					transaction.Rollback();
					return;
				}

				foreach (var table in entity.Tables)
				{
					table.IsBlocked = true;
				}

				UnitOfWork.Save();

				if (canCommit)
				{
					transaction?.Commit();
				}
			}
			catch (Exception ex)
			{
				AddError(ex.Message);
				transaction?.Rollback();
			}
		}
	}

	public void Cancel(UpdateStatusChastisementDto dTo)
	{
		var dto = new UpdateStatusChastisementDlDto { Id = dTo.Id, StatusId = StatusIdConst.NOT_ACCEPTED };
		UpdateStatus(dto, ent =>
		{
			if (!StatusIdConst.CanApplyChastisement(ent.StatusId, dto.StatusId))
				_repository.AddError("Нет доступа");
			var res = CreateDocumentChangeLog(dto.Id, dto.StatusId);
			//_hrmPackageContext.DocControlPackage.ChastisementCancelValidation(
			//    organizationId: _authService.CurrentOrganizationId,
			//    id: dto.Id,
			//    userId: (int)_authService.UserId
			//);
			var entity = Repository.AllAsQueryable.Include(x => x.Tables).FirstOrDefault(x => x.Id == dTo.Id);
			foreach (var table in entity.Tables)
			{
				table.IsBlocked = false;
			}

			CombineStatuses(_repository);
		});
	}
	public override void Delete(long id)
	{
		using (var transaction = UnitOfWork.BeginTransaction())
		{
			try
			{

				var entity = Repository.ById(id);
				entity.StatusId = StatusIdConst.DELETED;
				CombineStatuses(Repository);
				var dto = new UpdateStatusChastisementDlDto { Id = id, StatusId = StatusIdConst.DELETED };
				var ent = _repository.UpdateStatus(dto, ent =>
				{
					if (!StatusIdConst.CanApplyChastisement(ent.StatusId, dto.StatusId))
						_repository.AddError("Нет доступа");
				});
				if (IsValid)
				{
					UnitOfWork.Save();
					var res = CreateDocumentChangeLog(entity.Id, StatusIdConst.DELETED, "DeleteChastisement");
					transaction.Commit();
				}
			}
			catch (DbUpdateException)
			{
				AddError("Запись не может быть удален");
				transaction.Rollback();
			}
		}
	}

	private HaveId<long> UpdateStatus(UpdateStatusChastisementDlDto dto, Action<Chastisement> validation)
	{
		using (var transaction = _unitOfWork.BeginTransaction())
		{
			try
			{
				var entity = _repository.UpdateStatus(dto, validation);
				CombineStatuses(_repository);
				if (HasErrors)
					return null;
				_unitOfWork.Save();
				var res = CreateDocumentChangeLog(entity.Id, entity.StatusId, "Chastisement");
				if (IsValid)
					transaction.Commit();
				return res;
			}
			catch (Exception ex)
			{
				AddError(ex.Message);
				transaction.Rollback();
			}
		}
		return null;
	}

	public HaveId<long> CreateDocumentChangeLog(long id, int statusId, string message = null)
	{
		var moveDto = Repository.ById<ChastisementDto>(id, applyFilter: false);
		if (moveDto == null)
		{
			AddError($"No ChastisementDto found for id: {id}");
			return null;
		}
		_documentChangeLogService.Create(
			dto: moveDto,
			tableId: TableIdConst.HRM__DOC_CHASTISEMENT,
			organizationId: null,
			statusId: statusId,
			message: message);
		CombineStatuses(_documentChangeLogService);

		if (HasErrors)
			return null;

		return HaveId.Create(id);
	}

	private void Validation<TDto>(ChastisementDlDto<TDto> dto, Chastisement entity)
		  where TDto : ChastisementDlDto<TDto>
	{
		var query = Repository.AllAsQueryable;

		if (entity != null)
		{
			query = query.Where(a => a.Id != entity.Id);

			if (!StatusIdConst.CanModifyStatus.Contains(entity.StatusId))
				AddError("Таҳрирлаш мумкин эмас / Невозможно редактировать");
		}

		if (query.ByDocNumber(dto.DocNumber).Any())
			_repository.AddError($"Документ с этим кодом {dto.DocNumber} уже существует", nameof(dto.DocNumber));

		if (dto.Signer.Count() > 0)
		{
			if (dto.Signer.Count(a => a.IsHr) == 0)
				AddError("Имзоловчи кадр киритилмаган");
			if (dto.Signer.Count(a => a.IsHr) > 1)
				AddError("Имзоловчи кадр лавозимидаги ходим 1 та болиши керак");
			if (dto.Signer.Count(a => a.IsDirector) == 0)
				AddError("Имзоловчи директор киритилмаган");
			if (dto.Signer.Count(a => a.IsDirector) > 1)
				AddError("Имзоловчи дтректор лавозимидаги ходим 1 та болиши керак");
		}
		else
			AddError("Имзоловчи киритилмаган ");

	}
	public async Task Sign(SignStatusChastisementDto dto)
	{
		using (var transaction = _unitOfWork.BeginTransaction())
		{
			try
			{
				var entity = _repository.ById(dto.Id);

				if (entity == null)
				{ AddError("Not found"); return; }

				ChastisementSigner nextSigner = null;

				if (entity.Tables.Any(x => x.EmployeeManageId == _authService.User.EmployeeManageId))
				{
					if (entity.StatusId != StatusIdConst.ACCEPTED)
					{
						AddError("Hujjat tasdiqlanmagan");
						return;
					}
					var employeeManage = _unitOfWork.EmployeeManageRepository.AllAsQueryable
						.FirstOrDefault(x => x.Id == _authService.User.EmployeeManageId
						&& x.EndOn == null && x.IsDeleted == false);
					if (employeeManage == null)
					{
						AddError("employeeManage is null for authorized user");
						return;
					}
					nextSigner = new()
					{
						IsEmployee = true,
						OwnerId = dto.Id,
						EmployeeManageId = employeeManage.Id,
						PositionId = employeeManage.PositionId,
						DepartmentId = employeeManage.DepartmentId,
						SignOrder = _unitOfWork.Context.Set<ChastisementSigner>()
										.Where(a => a.OwnerId == dto.Id && !a.SignedAt.HasValue)
										.OrderByDescending(a => a.SignOrder).FirstOrDefault()?.SignOrder + 1 ?? 1
                    };
					entity.Signer.Add(nextSigner);
					var tableItem = entity.Tables.FirstOrDefault(x => x.EmployeeManageId == _authService.User.EmployeeManageId);
					if (tableItem == null)
					{
						AddError("Hujjatda yo'q foydalanuvchi.");
						transaction.Rollback();
						return;
					}
					tableItem.IsBlocked = false;
				}

				if (nextSigner == null)
				{
					nextSigner = _unitOfWork.Context.Set<ChastisementSigner>()
										.Where(a => a.OwnerId == dto.Id && !a.SignedAt.HasValue)
										.OrderBy(a => a.SignOrder).FirstOrDefault();
					if (nextSigner == null)
						AddError("Imzolovchi topilmadi");
					else if (nextSigner.EmployeeManageId != _authService.User.EmployeeManageId)
						AddError("Sizda imzolash huquqi yo'q");
					if (HasErrors)
						return;
				}

				var eImzoTimstampDto = new EImzoTimeStampDto
				{
					SignData = dto.SignedData,
					Inn = null,
					Pinfl = _authService.User.Pinfl
				};

				var timeStamp = _eImzoService.TimeStamp(eImzoTimstampDto).Result;
				CombineStatuses(_eImzoService);

				if (HasErrors)
				{
					transaction.Rollback();
					return;
				};

				var eImzoVerifyAttached = _eImzoService.VerifyAttached(new EImzoVerifyDto
				{
					SignData = timeStamp.Pkcs7b64,
					Inn = null,
					Pinfl = _authService.User.Pinfl
				}).Result;

				nextSigner.SignFile = SaveFile(dto.Id, timeStamp.Pkcs7b64, "sign.txt");
				nextSigner.DataFile = SaveFile(dto.Id, JsonConvert.SerializeObject(dto), "data.txt");
				nextSigner.SignedAt = DateTime.Now;
				nextSigner.SignedUserInfo = _authService.User.ToTextForDocumentLog();
				//_unitOfWork.Context.Entry(nextSigner).State = EntityState.Modified;
				_unitOfWork.Save();

				//var dto = new UpdateStatusAppointEmployeeDlDto { Id = dto.Id, StatusId = StatusIdConst.SIGNING };
				if (entity.StatusId != dto.StatusId)
				{
					var ent = _repository.UpdateStatus(dto, ent =>
					{
						if (!StatusIdConst.CanApplyChastisement(ent.StatusId, dto.StatusId))
							_repository.AddError("Нет доступа");
					});
				}
				
				CombineStatuses(_repository);
				if (HasErrors)
					return;

				_unitOfWork.Save();
				
				if (IsValid)
				{
					var res = CreateDocumentChangeLog(entity.Id, StatusIdConst.SIGNING);
					if (nextSigner.IsDirector)
					{
						Accept(new UpdateStatusChastisementDto
						{
							Id = dto.Id,
							StatusId = StatusIdConst.ACCEPTED
						});
					}
					transaction.Commit();
				}
			}
			catch (DbUpdateException ex)
			{
				AddError(ex.Message + ", " + ex.InnerException);
				transaction.Rollback();
			}
            catch (Exception ex)
            {
                AddError(ex.Message);
                transaction.Rollback();
            }
        }
	}
	private Guid SaveFile(long docId, string data, string fileName)
	{
		var ms = new MemoryStream();
		var writer = new StreamWriter(ms);
		writer.Write(data);
		writer.Flush();
		ms.Position = 0;
		var fileInfo = _storageService.Save($"{nameof(TableIdConst.HRM__DOC_CHASTISEMENT)}_SIGN_DATA", docId.ToString(), new StorageFile(Guid.NewGuid(), fileName, ms));

		return fileInfo.FirstOrDefault().FileId;
	}

	#region Files
	public byte[]? GetWordTemplate()
	{
		var lang = "uz-cyrl";
		
		var wordFile = _storageService.GetStaticFile(
			StaticFileConst.WordTemplate.GetFileName(lang, StaticFileConst.WordTemplate.CHASTISEMENT));
		
		CombineStatuses(_storageService);
		if(HasErrors)
			return null;
		
		return wordFile.ToArray();
	}
    public StorageFile DownloadByIdFile(Guid fileId)
    {
        var entity = _unitOfWork.Context
            .Set<MemshipContractFile>()
            .FirstOrDefault(a => a.Id == fileId);

        return Download(fileId, entity, DocumentStorageConst.DOC_CHASTISEMENT);
    }
    public byte[] DownloadFile(Guid? id2, Guid? fileId)
    {
        if (id2 == null && fileId == null)
        {
            AddError("Both id2 and fileId are null. At least one must be provided.");
            return null;
        }

        byte[] fileBytes = null;

        if (id2 != null)
        {
            Chastisement entity = null;

            entity = UnitOfWork.Context.Set<Chastisement>().Include(x => x.Files).FirstOrDefault(a => a.Id2 == id2);
            fileId = entity.Files.OrderByDescending(a => a.Id).FirstOrDefault()?.Id;
            if (fileId == null)
                return null;
            if (entity == null)
            {
                AddError($"File with id2 '{id2}'{(fileId != null ? $" and fileId '{fileId}'" : "")} not found.");
                return null;
            }

            var fileEntity = UnitOfWork.Context.Set<ChastisementFile>().FirstOrDefault(a => a.Id == fileId);

            var storageFile = Download(fileId.Value, fileEntity, DocumentStorageConst.DOC_CHASTISEMENT);

            if (storageFile != null)
            {
                using (var memoryStream = new MemoryStream())
                {
                    storageFile.GetStream().CopyTo(memoryStream);
                    fileBytes = memoryStream.ToArray();
                }
            }
        }

        return fileBytes;
    }
    private StorageFile Download(Guid fileId, FileEntity<long> entity, string storageDocument)
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
	public object UploadFiles(params StorageFile[] files)
	{
		if (files == null || files.Length <= 0)
		{
			AddError("Empty file");
			return null;
		}

		var result = _storageService.SaveTemp(DocumentStorageConst.DOC_CHASTISEMENT, files)
			.Select(a => new MemshipContractFileDto
			{
				Id = a.FileId,
				FileName = a.FileName,
				CreatedAt = DateTime.Now,
			});

		CombineStatuses(_storageService);
		return IsValid ? result : null;
	}
	public async Task<byte[]> DownloadPdf(Guid id2)
	{
		var dto = Repository.ReadAsNoTracked<ChastisementDto>(applyFilter: false)
							.FirstOrDefault(x => x.Id2 == id2);

		MemoryStream wordFile = null;

        var lang = "ru";

        wordFile = _storageService.GetStaticFile(
		   StaticFileConst.WordTemplate.GetFileName( lang /*ServiceProvider.CultureHelper.CurrentCulture.Code*/, StaticFileConst.WordTemplate.CHASTISEMENT));

        var entity = UnitOfWork.Context.Set<Chastisement>().Include(x => x.Files).FirstOrDefault(a => a.Id2 == id2);
		if (entity == null)
		{
			AddError("404 not found");
			return null;
		}
        var fileId = entity.Files.OrderByDescending(a => a.Id).FirstOrDefault()?.Id;
        var fileEntity = UnitOfWork.Context.Set<ChastisementFile>().FirstOrDefault(a => a.Id == fileId);

        var storageFile = DownloadFile(id2: entity.Id2, Guid.Empty);

        if (storageFile != null && (!fileEntity.IsReject.HasValue || fileEntity.IsReject == true))
        {
            var tempPath = "TempWordFileChastisement.docx";
            File.WriteAllBytes(tempPath, storageFile);

            using (FileStream file = new FileStream(tempPath, FileMode.Open, FileAccess.Read))
            {
                wordFile = new MemoryStream();
                file.CopyTo(wordFile);
                wordFile.Position = 0;
                //var tempFile = File.OpenRead(tempPath);

            }
            //tempFile.Close();
            File.Delete(tempPath);
        }


        if (wordFile is null)
		{
			AddError("Word fayl topilmadi");
			return null;
		}

		if (dto == null)
		{
			AddError("Hujjat topilmadi.");
			return null;
		}

		#region Capitalize
		foreach (var item in dto.Employees)
		{
			item.FullName = item.FullName.CapitalizeEachWord(" ");
		}

		#endregion


		dto.Signer.OrderByDescending(x => x.SignOrder);

		for (int i = 0; i < dto.Signer.Count; i++)
		{

			if (dto.Signer[i].IsDirector)
				dto.Signer[i].Position = dto.Signer[i].Position + " ";
			else if (dto.Signer[i].IsHr)
				dto.Signer[i].Position = "Kiritildi: <br/>" + dto.Signer[i].Position + " ";
			else
				dto.Signer[i].Position = "Kelishildi: <br/>" + dto.Signer[i].Position + " ";
			if (!dto.Signer[i].SignedAt.HasValue)
				dto.Signer[i].QrSign = null;
			if (!dto.Signer[i].SignedAt.HasValue)
				dto.Signer[i].QrSignValue = " ";
		}



		var plh2 = WordFactory.MakePlaceholders(dto);

        var handler = new DocXHandler(wordFile, plh2);
		handler.ReplaceLists();
		handler.ReplaceTexts();
		handler.ReplaceImages();
		var res = await _pdfConverter.DocxToPdfAsync(wordFile, new());
		CombineStatuses(_pdfConverter);
		return res;
	}
	#endregion
}
