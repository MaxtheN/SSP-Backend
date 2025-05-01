using AutoMapper;
using GenericServices;
using GenericServices.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SspUis.DataLayer.EfClasses;
using SspUis.DataLayer.Repositories;
using SspUis.DataLayer.EfClasses.Hrm;

namespace SspUis.BizLogicLayer.Hrm;

public class ElectionMemberTranslateDto : ElectionMemberTranslateDlDto, ILinkToEntity<ElectionMemberTranslate>
{
    public string Language { get; set; }
}
public class ElectionMemberTranslateDtoConfig : PerDtoConfig<ElectionMemberTranslateDto, ElectionMemberTranslate>
{
    public override Action<IMappingExpression<ElectionMemberTranslate, ElectionMemberTranslateDto>> AlterReadMapping => cfg => cfg
            .IncludeBase<ElectionMemberTranslate, ElectionMemberTranslateDlDto>()
            .ForMember(x => x.Language, x => x.MapFrom(ent => ent.Language.FullName));
}
