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

namespace SspUis.BizLogicLayer.Hrm.DepartmentServices
{
    public class DepartmentTranslateDto : DepartmentTranslateDlDto, ILinkToEntity<DepartmentTranslate>
    {
        public string Language { get; set; }
    }
    public class DepartmentTranslateDtoConfig : PerDtoConfig<DepartmentTranslateDto, DepartmentTranslate>
    {
        public override Action<IMappingExpression<DepartmentTranslate, DepartmentTranslateDto>> AlterReadMapping => cfg => cfg
                .IncludeBase<DepartmentTranslate, DepartmentTranslateDlDto>()
                .ForMember(x => x.Language, x => x.MapFrom(ent => ent.Language.FullName));
    }
}
