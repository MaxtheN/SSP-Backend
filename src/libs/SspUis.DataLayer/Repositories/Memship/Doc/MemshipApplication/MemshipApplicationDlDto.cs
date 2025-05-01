using System;
using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using SspUis.Core;
using SspUis.DataLayer.EfClasses;
using SspUis.DataLayer.Repositories.Doc.BaseApplication;
using WEBASE.Attributes;
using WEBASE.EF;

namespace SspUis.DataLayer.Repositories;

public class MemshipApplicationDlDto<TDto> : BaseApplicationDlDto<TDto, MemshipApplication>
    where TDto : MemshipApplicationDlDto<TDto>
{
    public int? ContractorActivityTypeId { get; set; }
    [LocalizedRequired]
    public int ContractorCategoryId { get; set; }
    [LocalizedRequired]
    public int EmployeesCount { get; set; }

    public bool IsRead { get; set; }
    public decimal? YearlyEarnings { get; set; }
    public decimal? YearlyTaxes { get; set; }
    public decimal? YearlyExport { get; set; }
    public decimal? YearlyImport { get; set; }
    public decimal? YearlyManufacture { get; set; }
    public string? ContractorEmail { get; set; }
    public string? ContractorMobilePhoneNumber { get; set; }
    public string? ContractorWorkPhoneNumber { get; set; }
    public string? ContractorAdditionalPhoneNumber { get; set; }
    public string? ContractorFaks { get; set; }
    public string? ContractorSkype { get; set; }
    public string? ContractorFacebook { get; set; }
    public string? ContractorTelegram { get; set; }
    public string? ContractorWebSite { get; set; }
    [LocalizedRequired]
    [LocalizedRange(1, int.MaxValue)]
    public int? OkedId { get; set; }
    public bool ChooseLocation { get; set; }
    public int? ChoosedRegionId { get; set; }
    public int? ChoosedDistrictId { get; set; }
    public int? OrganizationId { get; set; }
    public string? OwnerName { get; set; }
    public string? ContractorInn { get; set; }
    public List<MemshipApplicationFileDlDto> Files { get; set; }
    protected override Action<IMappingExpression<TDto, MemshipApplication>> AlterMapping =>
            cfg =>
            {
                base.AlterMapping(cfg);
                cfg.ForMember(x => x.Files, x => x.Ignore());
            };
    public override MemshipApplication CreateEntity()
    {
        var entity = base.CreateEntity();
        entity.Application.StatusId = StatusIdConst.CREATED;
        entity.IsRead = false;
        entity.Application.ApplicationTypeId = ApplicationTypeIdConst.MEMSHIP;
        entity.Files.AddFromTempFiles(DocumentStorageConst.DOC_MEMSHIP_APPLICATION_FILES, Files.Select(a => a.Id).ToList());
        return entity;
    }

    public override void UpdateEntity(MemshipApplication entity)
    {
        base.UpdateEntity(entity);
        entity.Application.StatusId = StatusIdConst.MODIFIED;
        entity.Files.UpdateFromFiles(DocumentStorageConst.DOC_MEMSHIP_APPLICATION_FILES, entity.Id.ToString(), Files.Select(a => a.Id).ToList());

    }
}
