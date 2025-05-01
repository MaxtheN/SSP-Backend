using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.StaticFiles;
using Microsoft.EntityFrameworkCore;
using SspUis.Core;
using SspUis.DataLayer;
using SspUis.DataLayer.EfClasses;
using SspUis.DataLayer.EfClasses.Hrm;
using StatusGeneric;
using WbAccessControl.Sdk;
using WEBASE.Storage;

namespace SspUis.BizLogicLayer.IntegrationServices.Wbac;

public class WbacIntegrationService : StatusGenericHandler, IWbacIntegrationService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IStorageService _storageService;

    public WbacIntegrationService(IUnitOfWork unitOfWork, IStorageService storageService)
    {
        _unitOfWork = unitOfWork;
        _storageService = storageService;
    }

    public async Task<List<WbacOrganizationDto>> GetOrganizationsAsync()
    {
        var organizations = await _unitOfWork.Context.Set<Organization>()
            .Where(a => a.OrganizationGroupId == OrganizationGroupIdConst.SSP
            || a.OrganizationGroupId == OrganizationGroupIdConst.REGIONAL_BRANCH
            || a.OrganizationGroupId == OrganizationGroupIdConst.ORGANIZATIONS_UNDER_SSP)
            .Select(a => new WbacOrganizationDto
            {
                OrganizationId = a.Id,
                ShortName = a.ShortName,
                FullName = a.FullName,
                Inn = a.Inn,
                Address = a.Address,
                PhoneNumber = a.PhoneNumber
            })
            .AsNoTracking()
            .ToListAsync();

        return organizations;
    }

    public async Task<List<WbacTurnstilePersonDto>> GetPeopleAsync()
    {
        var employees = await _unitOfWork.Context.Set<EmployeeManage>()
            .Where(a => (a.Organization.OrganizationGroupId == OrganizationGroupIdConst.SSP
            || a.Organization.OrganizationGroupId == OrganizationGroupIdConst.REGIONAL_BRANCH || a.Organization.OrganizationGroupId == OrganizationGroupIdConst.ORGANIZATIONS_UNDER_SSP)
                        && a.EndOn == null && a.IsDeleted == false)
            .Include(a => a.Organization)
            .Include(a => a.Employee)
            .ThenInclude(a => a.Person)
            .Select(a => new WbacTurnstilePersonDto
            {
                PersonId = a.EmployeeId,
                FullName = a.Employee.Person.FullName,
                OrganizationId = a.OrganizationId,
                TableId = TableIdConst.HL_EMPLOYEE,
                ImageId = a.Employee.Person.PictureId != null ? a.Employee.Person.PictureId.ToString() : null
            })
            .AsNoTracking()
            .Distinct()
            .ToListAsync();

        return employees;
    }

    public async Task<(Stream, string)?> GetPersonImageAsync(WbacImageRequestDto dto)
    {
        var employee = await _unitOfWork.Context.Set<Employee>()
            .Include(a => a.Person)
            .Where(a => a.Id == dto.PersonId && a.OrganizationId == dto.OrganizationId && dto.TableId == TableIdConst.HL_EMPLOYEE)
            .AsNoTracking()
            .FirstOrDefaultAsync();
        if (employee == null)
        {
            AddError("Employee is not found");
            return null;
        }

        var image = _storageService.GetFile(DocumentStorageConst.HL_PERSON_FILES, employee.PersonId.ToString(), (Guid)employee.Person.PictureId);
        if (image == null)
        {
            AddError("Employee Image is not found");
            return null;
        }

        CombineStatuses(_storageService);
        if (HasErrors)
            return null;

        //var contentType = string.IsNullOrEmpty(Path.GetExtension(image.FileName)) ? "application/octet-stream" : Path.GetExtension(image.FileName);
        var provider = new FileExtensionContentTypeProvider();
        string contentType;
        if (!provider.TryGetContentType(image.FileName, out contentType))
        {
            contentType = "application/octet-stream"; // Default
        }

        return (image.GetStream(), contentType);
    }

    public async Task<bool> PostTurnstilePersonTimeLogAsync(WbacPersonLogDto dto)
    {
        try
        {
            var entity = new EmployeeTurnstileLog
            {
                Id = Guid.NewGuid(),
                EmployeeId = (int)dto.PersonId,
                OrganizationId = dto.OrganizationId,
                EventAt = dto.EventAt,
                EventOn = DateOnly.FromDateTime(dto.EventAt),
                EmployeeTurnstileLogTypeId = (int)dto.EventType,
                DateOfCreated = DateTime.Now
            };

            await _unitOfWork.Context.Set<EmployeeTurnstileLog>().AddAsync(entity);

            if (IsValid)
            {
                await _unitOfWork.Context.SaveChangesAsync();
                return true;
            }
        }
        catch (Exception ex)
        {
            AddError(ex.Message);
        }
        return true;
    }
}
