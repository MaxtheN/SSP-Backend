using AutoMapper;
using GenericServices;
using GenericServices.Configuration;
using SspUis.DataLayer.EfClasses.Hrm;
using SspUis.DataLayer.Repositories;
using System;

namespace SspUis.BizLogicLayer.Hrm.QualificationCategoryServices
{
    public class  QualificationCategoryTranslateDto :  QualificationCategoryTranslateDlDto, ILinkToEntity< QualificationCategoryTranslate>
    {
        public string Language { get; set; }
    }

    public class  QualificationCategoryTranslateDtoConfig : PerDtoConfig< QualificationCategoryTranslateDto,  QualificationCategoryTranslate>
    {
        public override Action<IMappingExpression< QualificationCategoryTranslate,  QualificationCategoryTranslateDto>> AlterReadMapping =>
            cfg => cfg
                .IncludeBase< QualificationCategoryTranslate,  QualificationCategoryTranslateDlDto>()
                .ForMember(x => x.Language, x => x.MapFrom(ent => ent.Language.FullName));
    }
}
