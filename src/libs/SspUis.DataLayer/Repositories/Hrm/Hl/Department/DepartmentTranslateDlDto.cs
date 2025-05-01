using AutoMapper;
using GenericServices;
using GenericServices.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using WEBASE;
using WEBASE.Attributes;
using SspUis.DataLayer.EfClasses.Hrm;

namespace SspUis.DataLayer.Repositories
{
    public class DepartmentTranslateDlDto : TranslateDto<DepartmentTranslateDlDto, DepartmentTranslate, TranslateColumn>, ILinkToEntity<DepartmentTranslate>
    {

    }

    public class DepartmentTranslateDlDtoConfig : PerDtoConfig<DepartmentTranslateDlDto, DepartmentTranslate>
    {
        public override Action<IMappingExpression<DepartmentTranslate, DepartmentTranslateDlDto>> AlterReadMapping => cfg => cfg
            .ForMember(x => x.ColumnName, x => x.MapFrom(ent => ent.ColumnName.AsEnum<TranslateColumn>()));

        public override Action<IMappingExpression<DepartmentTranslateDlDto, DepartmentTranslate>> AlterSaveMapping =>
            cfg => cfg
                .ForMember(x => x.ColumnName, x => x.MapFrom(dto => dto.ColumnName));
    }
}
