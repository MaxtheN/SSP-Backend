using AutoMapper;
using GenericServices;
using GenericServices.Configuration;
using SspUis.DataLayer.EfClasses.Hrm;
using SspUis.DataLayer.Repositories;
using System;

namespace SspUis.BizLogicLayer.Hrm.StaffingIndicatorServices
{
    public class  StaffingIndicatorTranslateDto :  StaffingIndicatorTranslateDlDto, ILinkToEntity< StaffingIndicatorTranslate>
    {
        public string Language { get; set; }
    }

    public class  StaffingIndicatorTranslateDtoConfig : PerDtoConfig< StaffingIndicatorTranslateDto,  StaffingIndicatorTranslate>
    {
        public override Action<IMappingExpression< StaffingIndicatorTranslate,  StaffingIndicatorTranslateDto>> AlterReadMapping =>
            cfg => cfg
                .IncludeBase< StaffingIndicatorTranslate,  StaffingIndicatorTranslateDlDto>()
                .ForMember(x => x.Language, x => x.MapFrom(ent => ent.Language.FullName));
    }
}
