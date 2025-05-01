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

namespace SspUis.BizLogicLayer.BankServices
{
    public class BankTranslateDto : BankTranslateDlDto, ILinkToEntity<BankTranslate>
    {
        public string Language { get; set; }
    }

    public class BankTranslateDtoConfig : PerDtoConfig<BankTranslateDto, BankTranslate>
    {
        public override Action<IMappingExpression<BankTranslate, BankTranslateDto>> AlterReadMapping =>
            cfg => cfg
                .IncludeBase<BankTranslate, BankTranslateDlDto>()
                .ForMember(x => x.Language, x => x.MapFrom(ent => ent.Language.FullName));
    }
}
