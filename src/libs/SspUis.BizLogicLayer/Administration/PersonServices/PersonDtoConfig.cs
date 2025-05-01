using System;
using System.Linq;
using AutoMapper;
using GenericServices.Configuration;
using SspUis.DataLayer;
using SspUis.DataLayer.EfClasses;

namespace SspUis.BizLogicLayer.PersonServices
{
    public class PersonDtoConfig : PerDtoConfig<PersonDto, Person>
    {
        public override Action<IMappingExpression<Person, PersonDto>> AlterReadMapping =>
            cfg => cfg
                .ForMember(x => x.State, x => x.MapFrom(ent => ent.State.Translates.AsQueryable()
                    .FirstOrDefault(StateTranslate.GetExpr(TranslateColumn.full_name, ServiceProvider.CultureHelper.CurrentCulture.Id)).TranslateText ?? ent.State.FullName))
                .ForMember(x => x.Gender, x => x.MapFrom(ent => ent.Gender.Translates.AsQueryable()
                    .FirstOrDefault(GenderTranslate.GetExpr(TranslateColumn.full_name, ServiceProvider.CultureHelper.CurrentCulture.Id)).TranslateText ?? ent.Gender.FullName))
                .ForMember(x => x.Nationality, x => x.MapFrom(ent => ent.Nationality.Translates.AsQueryable()
                    .FirstOrDefault(NationalityTranslate.GetExpr(TranslateColumn.full_name, ServiceProvider.CultureHelper.CurrentCulture.Id)).TranslateText ?? ent.Nationality.FullName))
                .ForMember(x => x.Citizenship, x => x.MapFrom(ent => ent.Citizenship.Translates.AsQueryable()
                    .FirstOrDefault(CitizenshipTranslate.GetExpr(TranslateColumn.full_name, ServiceProvider.CultureHelper.CurrentCulture.Id)).TranslateText ?? ent.Citizenship.FullName))
                .ForMember(x => x.BirthCountry, x => x.MapFrom(ent => ent.BirthCountry.Translates.AsQueryable()
                    .FirstOrDefault(CountryTranslate.GetExpr(TranslateColumn.full_name, ServiceProvider.CultureHelper.CurrentCulture.Id)).TranslateText ?? ent.BirthCountry.FullName))
                //.ForMember(x => x.BirthRegion, x => x.MapFrom(ent => ent.BirthRegion.Translates.AsQueryable()
                //    .FirstOrDefault(RegionTranslate.GetExpr(TranslateColumn.full_name, ServiceProvider.CultureHelper.CurrentCulture.Id)).TranslateText ?? ent.BirthRegion.FullName))
                .ForMember(x => x.BirthDistrict, x => x.MapFrom(ent => ent.BirthDistrict.Translates.AsQueryable()
                    .FirstOrDefault(DistrictTranslate.GetExpr(TranslateColumn.full_name, ServiceProvider.CultureHelper.CurrentCulture.Id)).TranslateText ?? ent.BirthDistrict.FullName))
                .ForMember(x => x.LivingRegion, x => x.MapFrom(ent => ent.LivingRegion.Translates.AsQueryable()
                    .FirstOrDefault(RegionTranslate.GetExpr(TranslateColumn.full_name, ServiceProvider.CultureHelper.CurrentCulture.Id)).TranslateText ?? ent.LivingRegion.FullName))
                .ForMember(x => x.LivingDistrict, x => x.MapFrom(ent => ent.LivingDistrict.Translates.AsQueryable()
                    .FirstOrDefault(DistrictTranslate.GetExpr(TranslateColumn.full_name, ServiceProvider.CultureHelper.CurrentCulture.Id)).TranslateText ?? ent.LivingDistrict.FullName));
    }
}
