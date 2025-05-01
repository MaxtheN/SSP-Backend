using AutoMapper;
using GenericServices;
using GenericServices.Configuration;
using SspUis.DataLayer.EfClasses;
using SspUis.DataLayer.EfClasses.Hrm;
using SspUis.DataLayer.Repositories;
using System;

namespace SspUis.BizLogicLayer.Hrm.NeedChamberServiceServices
{
    public class  NeedChamberServiceTranslateDto :  NeedChamberServiceTranslateDlDto, ILinkToEntity< NeedChamberServiceTranslate>
    {
        public string Language { get; set; }
    }

    public class  NeedChamberServiceTranslateDtoConfig : PerDtoConfig< NeedChamberServiceTranslateDto,  NeedChamberServiceTranslate>
    {
        public override Action<IMappingExpression< NeedChamberServiceTranslate,  NeedChamberServiceTranslateDto>> AlterReadMapping =>
            cfg => cfg
                .IncludeBase< NeedChamberServiceTranslate,  NeedChamberServiceTranslateDlDto>()
                .ForMember(x => x.Language, x => x.MapFrom(ent => ent.Language.FullName));
    }
}
