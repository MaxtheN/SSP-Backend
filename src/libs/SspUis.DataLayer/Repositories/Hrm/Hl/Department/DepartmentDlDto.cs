using AutoMapper;
using SspUis.DataLayer.EfClasses;
using SspUis.DataLayer.EfClasses.Hrm;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WEBASE;
using WEBASE.Attributes;
using WEBASE.EF;

namespace SspUis.DataLayer.Repositories;

public class DepartmentDlDto<TDto> : EntityDto<TDto, Department>
        where TDto : DepartmentDlDto<TDto>
{
    public string OrderCode { get; set; }
    //[LocalizedRequired]
    //[LocalizedRange(1, long.MaxValue)]
    public long? Code { get; set; }
    [LocalizedRequired]
    [LocalizedStringLength(250)]
    public string ShortName { get; set; }
    [LocalizedRequired]
    [LocalizedStringLength(500)]
    public string FullName { get; set; }
    public string IndexCode { get; set; }
    public int? ParentId { get; set; }
    public int? IndicatorDepartmentId { get; set; }
    public List<DepartmentTranslateDlDto> Translates { get; set; } = new List<DepartmentTranslateDlDto>();

    protected override Action<IMappingExpression<TDto, Department>> AlterMapping => cfg => cfg
        .ForMember(x => x.Translates, x => x.Ignore());

    public override Department CreateEntity()
    {
        var entity = base.CreateEntity();
        entity.StateId = StateIdConst.ACTIVE;
        Translates.AddByUniqueFKTo(entity.Translates);
        return entity;
    }

    public override void UpdateEntity(Department entity)
    {
        base.UpdateEntity(entity);
        Translates.AddByUniqueFKTo(entity.Translates);
    }

}
