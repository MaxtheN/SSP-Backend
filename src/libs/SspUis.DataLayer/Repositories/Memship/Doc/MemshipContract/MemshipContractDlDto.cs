using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using AutoMapper;
using Newtonsoft.Json;
using SspUis.Core;
using SspUis.DataLayer.EfClasses;
using SspUis.DataLayer.EfClasses.Claim.Info;
using WEBASE.Attributes;
using WEBASE.EF;

namespace SspUis.DataLayer.Repositories.Memship;

public class MemshipContractDlDto<TDto> : EntityDto<TDto, MemshipContract>
    where TDto : MemshipContractDlDto<TDto>
{
    [LocalizedRequired]
    public DateOnly DocOn { get; set; } = DateOnly.FromDateTime(DateTime.Now);
    [LocalizedRequired]
    public string DocNumber { get; set; }
    [LocalizedRequired]
    [LocalizedRange(1, long.MaxValue)]
    public long? ApplicationId { get; set; }
    [LocalizedRequired]
    [LocalizedRange(1, int.MaxValue)]
    public int MemshipContractTypeId { get; set; }
    public bool IsRead { get; set; }
    public string WebImzoSecretKey { get; set; }
    public Guid WebImzoRequestId { get; set; }
    public long? ContractorSettlementAccountId { get; set; }
    public int? ContractorCategoryId { get; set; }
    public long? OrganizationSettlementAccountId { get; set; }
    [LocalizedRequired]
    public decimal BaseFixedMinimumValue { get; set; }
    [JsonIgnore]
    [System.Text.Json.Serialization.JsonIgnore]
    [IgnoreMap]
    public int? OrganizationId { get; set; }
	public string? Details { get; set; }
	public int? RegionalOrganizationId { get; set; }
    public List<MemshipContractFileDlDto> Files { get; set; } = new();
    protected override Action<IMappingExpression<TDto, MemshipContract>> AlterMapping =>
       cfg => cfg
       .ForMember(x => x.Files, x => x.Ignore());
    public override MemshipContract CreateEntity()
    {
        var entity = base.CreateEntity();
        entity.IsRead = false;
        entity.Id2 = Guid.NewGuid();
        entity.StatusId = StatusIdConst.CREATED;
        entity.Files.AddFromTempFiles(DocumentStorageConst.DOC_MEMSHIP_CONTRACT_FILE, Files.Select(a => a.Id).ToList());
        return entity;
    }
    public override void UpdateEntity(MemshipContract entity)
    {
        base.UpdateEntity(entity);
        entity.StatusId = StatusIdConst.MODIFIED;
        entity.Files.UpdateFromFiles(DocumentStorageConst.DOC_MEMSHIP_CONTRACT_FILE, entity.Id.ToString(), Files.Select(a => a.Id).ToList());
    }
}
