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
    public class BankTranslateDlDto : 
        TranslateDto<BankTranslateDlDto, BankTranslate, BankTranslateColumn>,
        ILinkToEntity<BankTranslate>
    {
    }
  
    public class BankTranslateDlDtoConfig : PerDtoConfig<BankTranslateDlDto, BankTranslate>
    {
        public override Action<IMappingExpression<BankTranslate, BankTranslateDlDto>> AlterReadMapping =>
            cfg => cfg
                .ForMember(x => x.ColumnName, x => x.MapFrom(ent => ent.ColumnName.AsEnum<BankTranslateColumn>()));

        public override Action<IMappingExpression<BankTranslateDlDto, BankTranslate>> AlterSaveMapping => 
            cfg => cfg
                .ForMember(x => x.ColumnName, x => x.MapFrom(dto => dto.ColumnName));
    }
}
