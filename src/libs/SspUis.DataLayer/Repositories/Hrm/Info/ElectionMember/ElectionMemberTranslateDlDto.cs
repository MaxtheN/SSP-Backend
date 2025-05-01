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
using SspUis.DataLayer.EfClasses.Hrm;

namespace SspUis.DataLayer.Repositories
{
    public class ElectionMemberTranslateDlDto : TranslateDto<ElectionMemberTranslateDlDto, ElectionMemberTranslate, TranslateColumn>, ILinkToEntity<ElectionMemberTranslate>
    {

    }

    public class ElectionMemberTranslateDlDtoConfig : PerDtoConfig<ElectionMemberTranslateDlDto, ElectionMemberTranslate>
    {
        public override Action<IMappingExpression<ElectionMemberTranslate, ElectionMemberTranslateDlDto>> AlterReadMapping => cfg => cfg
            .ForMember(x => x.ColumnName, x => x.MapFrom(ent => ent.ColumnName.AsEnum<TranslateColumn>()));

        public override Action<IMappingExpression<ElectionMemberTranslateDlDto, ElectionMemberTranslate>> AlterSaveMapping =>
            cfg => cfg
                .ForMember(x => x.ColumnName, x => x.MapFrom(dto => dto.ColumnName));
    }
}
