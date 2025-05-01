using SspUis.DataLayer.EfClasses.Public.Hl;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System;
using WEBASE.EF;
using WEBASE.Attributes;
using OpenXmlPowerTools.HtmlToWml;
using SspUis.Core;
using System.Collections.Generic;
using WEBASE.Utility;
using WEBASE;

namespace SspUis.DataLayer.Repositories;

public class PersonLogDlDto<TDto> : EntityDto<TDto, PersonLog>
        where TDto : PersonLogDlDto<TDto> 
{
    public int Id { get; set; }
    [LocalizedRequired]
    [LocalizedStringLength(50)]
    public string Pinfl { get; set; }
    [LocalizedRequired]
    [LocalizedStringLength(50)]
    public string PassportSeria { get; set; }
    [LocalizedRequired]
    [LocalizedStringLength(50)]
    public string PassportNumber { get; set; }
    public DateTime? PassportDate { get; set; }
    public DateTime? PassportExpiration { get; set; }
    [LocalizedRequired]
    [LocalizedStringLength(500)]
    public string PassportDivName { get; set; }
    [LocalizedRequired]
    public int PersonId { get; set; }
    [LocalizedRequired]
    public int EmployeeId { get; set; }

    public override PersonLog CreateEntity()
    {
        return base.CreateEntity();
    }
    public override void UpdateEntity(PersonLog entity)
    {
        base.UpdateEntity(entity);
    }
}

