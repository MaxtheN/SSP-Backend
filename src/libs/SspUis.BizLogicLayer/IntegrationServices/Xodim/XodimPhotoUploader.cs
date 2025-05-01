using Microsoft.EntityFrameworkCore;
using SspUis.BizLogicLayer.PersonServices;
using SspUis.Core;
using SspUis.DataLayer;
using SspUis.DataLayer.EfClasses.Hrm;
using SspUis.DataLayer.Repositories;
using SspUis.Integration.DigitizationCenter;
using SspUis.Integration.DigitizationCenter.Models.GSP;
using SspUis.Integration.DigitizationCenter.Services.GSP;
using StatusGeneric;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using WbAccessControl.Sdk;
using WEBASE;
using WEBASE.Storage;

namespace SspUis.BizLogicLayer.IntegrationServices.Xodim;

public class XodimPhotoUploader : StatusGenericHandler, IXodimPhotoUploader
{
    private readonly IEmployeeRepository _repository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IEmployeeService _service;
    private readonly IPersonService _personService;
    private readonly IDigitizationCenterGspService _gspService;
    private readonly IStorageService _storageService;
    private readonly IWbacClientService _wbacClientService;
    private readonly Core.Security.IAuthService _authService;

    public XodimPhotoUploader(
        IUnitOfWork unitOfWork,
        IEmployeeService service,
        IPersonService personService,
        IDigitizationCenterGspService gspService,
        IStorageService storageService,
        IWbacClientService wbacClientService,
        Core.Security.IAuthService authService)
    {
        _unitOfWork = unitOfWork;
        _repository = unitOfWork.EmployeeRepository;
        _service = service;
        _personService = personService;
        _gspService = gspService;
        _storageService = storageService;
        _wbacClientService = wbacClientService;
        _authService = authService;
    }

    public int NoPhotoCount()
    {
        var allEmployees = _repository.AllAsQueryable
          .Where(t => t.StateId == StateIdConst.ACTIVE)
          .Include(p => p.Person).Where(p => p.Person.PictureId == null)
          .ToList();

        return allEmployees.Count();
    }

    public async Task UploadByEmployeeId(int Id)
    {
        var allEmployees = _repository.AllAsQueryable
          .Where(t => t.StateId == StateIdConst.ACTIVE && t.Id == Id)
          .Include(p => p.Person)
          .ToList();

        foreach (var item in allEmployees)
        {
            try
            {
                Employee employee = item;

                if (item.Person.PictureId == null)
                {
                    GSPNewApiRequestDto gSPNewApiRequestDto = new GSPNewApiRequestDto()
                    {
                        transaction_id = 3,
                        is_consent = "Y",
                        is_photo = "Y",
                        langId = 1,
                        document = item.Person.PassportSeria + item.Person.PassportNumber,
                        birth_date = item.Person.BirthDate.ToString("yyyy-MM-dd")
                    };

                    List<GSPNewApiData> data = _gspService.GetFromGSP(gSPNewApiRequestDto).Result;

                    if (data == null)
                    {
                        continue;
                    }

                    StorageFile storageFile = ConvertBase64ToStorageFile(data.FirstOrDefault().photo, "image.jpg");

                    var file = _personService.UploadFile(storageFile);
                    item.Person.PictureId = file.FileId;
                    _unitOfWork.Save();
                }

                if (item.Person.PictureId != null)
                {
                    _personService.AddOrUpdateFiles(item.Person.Id, (Guid)item.Person.PictureId);
                    CombineStatuses(_personService);

                    _storageService.MoveToPersistent(DocumentStorageConst.HL_PERSON_FILES, $"{item.PersonId}", (Guid)item.Person.PictureId);
                    CombineStatuses(_storageService);
                }

                if (employee.Person.PictureId != null && IsValid)
                {
                    var employeeManage = _unitOfWork.Context.Set<EmployeeManage>()
                          .Include(a => a.Organization)
                          .Include(a => a.Employee)
                          .ThenInclude(a => a.Person)
                          .FirstOrDefaultAsync(a => (a.Organization.OrganizationGroupId == OrganizationGroupIdConst.SSP
                                  || a.Organization.OrganizationGroupId == OrganizationGroupIdConst.ORGANIZATIONS_UNDER_SSP) && a.EndOn == null
                                  && a.IsDeleted == false && a.Employee.Person.Id == employee.PersonId).Result;

                    if (employeeManage != null)
                    {
                        var turnstilePersonDataResult = await _wbacClientService.UpSertPersonAsync(new WbacTurnstilePersonDto
                        {
                            PersonId = employeeManage.Employee.Id,
                            TableId = TableIdConst.HL_EMPLOYEE,
                            OrganizationId = employeeManage.Employee.OrganizationId,
                            FullName = employeeManage.Employee.Person.FullName,
                            ImageId = employeeManage.Employee.Person.PictureId != null ? employeeManage.Employee.Person.PictureId.ToString() : null
                        });

                        if (!turnstilePersonDataResult.IsSuccess || !turnstilePersonDataResult.Response)
                            CombineStatuses(turnstilePersonDataResult.GetStatusGeneric());
                    }
                }

            }
            catch (Exception e)
            {
                AddError(e.Message);
            }
        }
    }

    public async Task UploadPhotos()
    {
        _authService.ResetUserName("webaseadmin");
        var allEmployees = _repository.AllAsQueryable
          .Where(t => t.StateId == StateIdConst.ACTIVE)
          .Include(p => p.Person).Where(p => p.Person.PictureId == null)
          .ToList();

        foreach (var item in allEmployees)
        {
            try
            {
                Employee employee = item;

                if (item.Person.PictureId == null)
                {
                    GSPNewApiRequestDto gSPNewApiRequestDto = new GSPNewApiRequestDto()
                    {
                        transaction_id = 3,
                        is_consent = "Y",
                        is_photo = "Y",
                        langId = 1,
                        document = item.Person.PassportSeria + item.Person.PassportNumber,
                        birth_date = item.Person.BirthDate.ToString("yyyy-MM-dd")
                    };

                    List<GSPNewApiData> data = _gspService.GetFromGSP(gSPNewApiRequestDto).Result;

                    if (data == null)
                    {
                        continue;
                    }

                    StorageFile storageFile = ConvertBase64ToStorageFile(data.FirstOrDefault().photo, "image.jpg");

                    var file = _personService.UploadFile(storageFile);
                    item.Person.PictureId = file.FileId;
                    _unitOfWork.Save();
                }

                if (item.Person.PictureId != null)
                {
                    _personService.AddOrUpdateFiles(item.Person.Id, (Guid)item.Person.PictureId);
                    CombineStatuses(_personService);

                    _storageService.MoveToPersistent(DocumentStorageConst.HL_PERSON_FILES, $"{item.PersonId}", (Guid)item.Person.PictureId);
                    CombineStatuses(_storageService);
                }

                if (employee.Person.PictureId != null && IsValid)
                {
                    var employeeManage = _unitOfWork.Context.Set<EmployeeManage>()
                          .Include(a => a.Organization)
                          .Include(a => a.Employee)
                          .ThenInclude(a => a.Person)
                          .FirstOrDefaultAsync(a => (a.Organization.OrganizationGroupId == OrganizationGroupIdConst.SSP
                                                    || a.Organization.OrganizationGroupId == OrganizationGroupIdConst.ORGANIZATIONS_UNDER_SSP) 
                                                    && a.EndOn == null && a.IsDeleted == false 
                                                    && a.Employee.Person.Id == employee.PersonId).Result;

                    if (employeeManage != null)
                    {
                        var turnstilePersonDataResult = await _wbacClientService.UpSertPersonAsync(new WbacTurnstilePersonDto
                        {
                            PersonId = employeeManage.Employee.Id,
                            TableId = TableIdConst.HL_EMPLOYEE,
                            OrganizationId = employeeManage.Employee.OrganizationId,
                            FullName = employeeManage.Employee.Person.FullName,
                            ImageId = employeeManage.Employee.Person.PictureId != null ? employeeManage.Employee.Person.PictureId.ToString() : null
                        });

                        if (!turnstilePersonDataResult.IsSuccess || !turnstilePersonDataResult.IsSuccess)
                            CombineStatuses(turnstilePersonDataResult.GetStatusGeneric());
                    }
                }
            }
            catch (Exception e)
            {
                AddError(e.Message);
            }
        }
    }

    StorageFile ConvertBase64ToStorageFile(string base64String, string fileName)
    {
        byte[] fileBytes = Convert.FromBase64String(base64String);
        MemoryStream memoryStream = new MemoryStream(fileBytes);

        StorageFile storageFile = new StorageFile(fileName, memoryStream);
        return storageFile;
    }
}