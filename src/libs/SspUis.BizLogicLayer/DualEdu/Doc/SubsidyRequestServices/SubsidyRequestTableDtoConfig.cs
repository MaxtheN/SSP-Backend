using System;
using AutoMapper;
using GenericServices.Configuration;
using SspUis.DataLayer.EfClasses.DualEdu;

namespace SspUis.BizLogicLayer;

public class SubsidyRequestTableDtoConfig : PerDtoConfig<SubsidyRequestTableDto, SubsidyRequestTable>
{
    public override Action<IMappingExpression<SubsidyRequestTable, SubsidyRequestTableDto>> AlterReadMapping =>
        cfg => cfg
        .ForMember(x => x.Pinfl, c => c.MapFrom(ent => ent.Person.Pinfl))
        .ForMember(x => x.Surname, c => c.MapFrom(ent => ent.Person.SurnameLatin))
        .ForMember(x => x.Name, c => c.MapFrom(ent => ent.Person.NameLatin))
        .ForMember(x => x.Seria, c => c.MapFrom(ent => ent.Person.PassportSeria))
        .ForMember(x => x.Number, c => c.MapFrom(ent => ent.Person.PassportNumber))
        .ForMember(x => x.Patronym, c => c.MapFrom(ent => ent.Person.PatronymLatin))
        .ForMember(x => x.DateOfBirth, c => c.MapFrom(ent => ent.Person.BirthDate))
        //.ForMember(x => x.ApplicationType, x => x.MapFrom(ent => ent.ApplicationType.Translates.AsQueryable()
        //    .FirstOrDefault(ApplicationTypeTranslate.GetExpr(TranslateColumn.full_name, ServiceProvider.CultureHelper.CurrentCulture.Id)).TranslateText ?? ent.ApplicationType.FullName))
        ;
}
