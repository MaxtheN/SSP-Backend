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

namespace SspUis.BizLogicLayer.RoleServices
{
    public class RoleTranslateDto : RoleTranslateDlDto, ILinkToEntity<RoleTranslate>
    {
        public string Language { get; set; }
    }

    public class RoleTranslateDtoConfig : PerDtoConfig<RoleTranslateDto, RoleTranslate>
    {
        public override Action<IMappingExpression<RoleTranslate, RoleTranslateDto>> AlterReadMapping =>
            cfg => cfg
                .IncludeBase<RoleTranslate, RoleTranslateDlDto>()
                .ForMember(x => x.Language, x => x.MapFrom(ent => ent.Language.FullName));
    }
}
