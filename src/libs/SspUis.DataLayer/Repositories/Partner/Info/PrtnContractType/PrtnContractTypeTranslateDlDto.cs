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

namespace SspUis.DataLayer.Repositories
{
    public class PrtnContractTypeTranslateDlDto :
        TranslateDto<PrtnContractTypeTranslateDlDto, PrtnContractTypeTranslate, TranslateColumn>,
        ILinkToEntity<PrtnContractTypeTranslate>
    {

    }
    public class PrtnContractTypeTranslateDlDtoConfig : PerDtoConfig<PrtnContractTypeTranslateDlDto, PrtnContractTypeTranslate>
    {
        public override Action<IMappingExpression<PrtnContractTypeTranslate, PrtnContractTypeTranslateDlDto>> AlterReadMapping =>
            cfg => cfg
                .ForMember(x => x.ColumnName, x => x.MapFrom(ent => ent.ColumnName.AsEnum<TranslateColumn>()));

        public override Action<IMappingExpression<PrtnContractTypeTranslateDlDto, PrtnContractTypeTranslate>> AlterSaveMapping =>
            cfg => cfg
                .ForMember(x => x.ColumnName, x => x.MapFrom(dto => dto.ColumnName));
    }
}
