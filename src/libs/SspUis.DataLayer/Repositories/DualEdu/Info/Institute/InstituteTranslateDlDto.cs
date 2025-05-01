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
using SspUis.DataLayer.EfClasses;
using SspUis.DataLayer.EfClasses.DualEdu;

namespace SspUis.DataLayer.Repositories
{
    public class InstituteTranslateDlDto : TranslateDto<InstituteTranslateDlDto, InstituteTranslate, TranslateColumn>, ILinkToEntity<InstituteTranslate>
    {

    }

    public class InstituteTranslateDlDtoConfig : PerDtoConfig<InstituteTranslateDlDto, InstituteTranslate>
    {
        public override Action<IMappingExpression<InstituteTranslate, InstituteTranslateDlDto>> AlterReadMapping => cfg => cfg
            .ForMember(x => x.ColumnName, x => x.MapFrom(ent => ent.ColumnName.AsEnum<TranslateColumn>()));

        public override Action<IMappingExpression<InstituteTranslateDlDto, InstituteTranslate>> AlterSaveMapping =>
            cfg => cfg
                .ForMember(x => x.ColumnName, x => x.MapFrom(dto => dto.ColumnName));
    }
}
