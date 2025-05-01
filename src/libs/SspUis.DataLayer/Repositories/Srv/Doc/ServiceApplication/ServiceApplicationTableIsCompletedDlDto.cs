using AutoMapper;
using GenericServices;
using SspUis.DataLayer.EfClasses;
using System;
using System.Collections.Generic;
using WEBASE.Attributes;
using WEBASE.EF;
using WEBASE.Models;

namespace SspUis.DataLayer.Repositories;

public class ServiceApplicationGroupIsCompletedDlDto
    : EntityDto<ServiceApplicationGroupDlDto, ServiceApplicationGroup>,
        IHaveIdProp<long>, ILinkToEntity<ServiceApplicationGroup>
{
    [LocalizedRequired]
    [LocalizedRange(1, long.MaxValue)]
    public long Id { get; set; }
    public int? GroupId { get; set; }
    public List<ServiceApplicationTableIsCompletedDlDto> Tables { get; set; } = new();
    protected override Action<IMappingExpression<ServiceApplicationGroupDlDto, ServiceApplicationGroup>> AlterMapping =>
            cfg => cfg.ForMember(x => x.Tables, opt => opt.Ignore());

    public override void UpdateEntity(ServiceApplicationGroup entity)
    {
        base.UpdateEntity(entity);
        Tables.ApplyChangesTo<long, ServiceApplicationTableIsCompletedDlDto, ServiceApplicationTable>(entity.Tables);
    }
}

public class ServiceApplicationTableIsCompletedDlDto
    : EntityDto<ServiceApplicationTableIsCompletedDlDto, ServiceApplicationTable>,
        IHaveIdProp<long>
{
    [LocalizedRequired]
    [LocalizedRange(1, long.MaxValue)]
    public long Id { get; set; }

    [LocalizedRequired]
    public bool IsCompleted { get; set; }

    public override void UpdateEntity(ServiceApplicationTable entity)
    {
        base.UpdateEntity(entity);
        entity.IsCompleted = IsCompleted;
    }
}