using AutoMapper;
using GenericServices;
using GenericServices.Configuration;
using SspUis.DataLayer.EfClasses.Hrm;
using System;
using WEBASE;

namespace SspUis.DataLayer.Repositories
{
    public class  StaffingIndicatorTranslateDlDto : TranslateDto< StaffingIndicatorTranslateDlDto,  StaffingIndicatorTranslate, TranslateColumn>, ILinkToEntity< StaffingIndicatorTranslate>
    {
    }

    public class  StaffingIndicatorTranslateDlDtoConfig : PerDtoConfig< StaffingIndicatorTranslateDlDto,  StaffingIndicatorTranslate>
    {
        public override Action<IMappingExpression< StaffingIndicatorTranslate,  StaffingIndicatorTranslateDlDto>> AlterReadMapping => cfg => cfg
            .ForMember(x => x.ColumnName,x => x.MapFrom(ent => ent.ColumnName.AsEnum<TranslateColumn>()));

        public override Action<IMappingExpression< StaffingIndicatorTranslateDlDto,  StaffingIndicatorTranslate>> AlterSaveMapping => 
            cfg => cfg
                .ForMember(x => x.ColumnName,x => x.MapFrom(dto => dto.ColumnName));
    }
}
