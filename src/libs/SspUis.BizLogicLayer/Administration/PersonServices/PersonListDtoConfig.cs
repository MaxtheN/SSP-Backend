using AutoMapper;
using SspUis.DataLayer;
using SspUis.DataLayer.EfClasses;
using GenericServices.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SspUis.BizLogicLayer.PersonServices
{
    public class PersonListDtoConfig : PerDtoConfig<PersonListDto, Person>
    {
        public override Action<IMappingExpression<Person, PersonListDto>> AlterReadMapping =>
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
                .ForMember(x => x.BirthRegion, x => x.MapFrom(ent => ent.BirthRegion.Translates.AsQueryable()
                    .FirstOrDefault(RegionTranslate.GetExpr(TranslateColumn.full_name, ServiceProvider.CultureHelper.CurrentCulture.Id)).TranslateText ?? ent.BirthRegion.FullName))
                .ForMember(x => x.BirthDistrict, x => x.MapFrom(ent => ent.BirthDistrict.Translates.AsQueryable()
                    .FirstOrDefault(DistrictTranslate.GetExpr(TranslateColumn.full_name, ServiceProvider.CultureHelper.CurrentCulture.Id)).TranslateText ?? ent.BirthDistrict.FullName))
                .ForMember(x => x.LivingRegion, x => x.MapFrom(ent => ent.LivingRegion.Translates.AsQueryable()
                    .FirstOrDefault(RegionTranslate.GetExpr(TranslateColumn.full_name, ServiceProvider.CultureHelper.CurrentCulture.Id)).TranslateText ?? ent.LivingRegion.FullName))
                .ForMember(x => x.LivingDistrict, x => x.MapFrom(ent => ent.LivingDistrict.Translates.AsQueryable()
                    .FirstOrDefault(DistrictTranslate.GetExpr(TranslateColumn.full_name, ServiceProvider.CultureHelper.CurrentCulture.Id)).TranslateText ?? ent.LivingDistrict.FullName));
    }
}
