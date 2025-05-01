using AutoMapper;
using GenericServices;
using GenericServices.Configuration;
using SspUis.DataLayer.EfClasses.Hrm;
using System;
using WEBASE;

namespace SspUis.DataLayer.Repositories
{
    public class  QualificationCategoryTranslateDlDto : TranslateDto< QualificationCategoryTranslateDlDto,  QualificationCategoryTranslate, TranslateColumn>, ILinkToEntity< QualificationCategoryTranslate>
    {
    }

    public class  QualificationCategoryTranslateDlDtoConfig : PerDtoConfig< QualificationCategoryTranslateDlDto,  QualificationCategoryTranslate>
    {
        public override Action<IMappingExpression< QualificationCategoryTranslate,  QualificationCategoryTranslateDlDto>> AlterReadMapping => cfg => cfg
            .ForMember(x => x.ColumnName,x => x.MapFrom(ent => ent.ColumnName.AsEnum<TranslateColumn>()));

        public override Action<IMappingExpression< QualificationCategoryTranslateDlDto,  QualificationCategoryTranslate>> AlterSaveMapping => 
            cfg => cfg
                .ForMember(x => x.ColumnName,x => x.MapFrom(dto => dto.ColumnName));
    }
}
