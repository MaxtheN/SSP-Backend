using System;
using System.Linq;
using AutoMapper;
using GenericServices.Configuration;
using SspUis.DataLayer;
using SspUis.DataLayer.EfClasses;
using SspUis.DataLayer.EfClasses.Appeal;

namespace SspUis.BizLogicLayer.Appeal;

public class AppealApplicationDtoConfig : PerDtoConfig<AppealApplicationDto, AppealApplication>
{
    public override Action<IMappingExpression<AppealApplication, AppealApplicationDto>> AlterReadMapping =>
        cfg => cfg

            .ForMember(d => d.AppealType, c => c.MapFrom(e => e.AppealType.FullName))
            .ForMember(d => d.ContractorInn, c => c.MapFrom(e => e.Contractor.Pinfl ?? e.Contractor.Inn))
            .ForMember(d => d.PersonFullName, c => c.MapFrom(e => e.PersonFullName))
            .ForMember(d => d.Details, c => c.MapFrom(e => e.Details))
            .ForMember(d => d.Organization, c => c.MapFrom(e => e.Organization.Translates.AsQueryable()
                             .FirstOrDefault(OrganizationTranslate.GetExpr(TranslateColumn.full_name, ServiceProvider.CultureHelper.CurrentCulture.Id))
                                        .TranslateText ?? e.Organization.FullName))
            .ForMember(d => d.Email, c => c.MapFrom(e => e.Email))
            .ForMember(d => d.AppealFormatType, c => c.MapFrom(e => e.AppealFormatType.FullName))

            .ForMember(d => d.Status, c => c.MapFrom(e => e.Status.Translates.AsQueryable()
                .FirstOrDefault(StatusTranslate.GetExpr(TranslateColumn.full_name, ServiceProvider.CultureHelper.CurrentCulture.Id))
                    .TranslateText ?? e.Status.FullName))

            .ForMember(d => d.AppealTypeArrive, c => c.MapFrom(e => e.AppealTypeArrive.Translates.AsQueryable()
                .FirstOrDefault(AppealTypeArriveTranslate.GetExpr(TranslateColumn.full_name, ServiceProvider.CultureHelper.CurrentCulture.Id))
                    .TranslateText ?? e.AppealTypeArrive.FullName))

            .ForMember(d => d.AppealDescription, c => c.MapFrom(e => e.AppealDescription.Translates.AsQueryable()
                .FirstOrDefault(AppealDescriptionTranslate.GetExpr(TranslateColumn.full_name, ServiceProvider.CultureHelper.CurrentCulture.Id))
                    .TranslateText ?? e.AppealDescription.FullName))

            .ForMember(x => x.Region, x => x.MapFrom(ent => ent.Contractor.Region.Translates.AsQueryable()
                .FirstOrDefault(RegionTranslate.GetExpr(TranslateColumn.full_name, ServiceProvider.CultureHelper.CurrentCulture.Id))
                    .TranslateText ?? ent.Region.FullName))

            .ForMember(x => x.District, x => x.MapFrom(ent => ent.Contractor.District.Translates.AsQueryable()
                .FirstOrDefault(DistrictTranslate.GetExpr(TranslateColumn.full_name, ServiceProvider.CultureHelper.CurrentCulture.Id))
                    .TranslateText ?? ent.District.FullName))
            .ForMember(x => x.ContractorFulName, x => x.MapFrom(ent => ent.Contractor.FullName))
            .ForMember(x => x.Director, x => x.MapFrom(ent => ent.Contractor.Director))
            .ForMember(x => x.Address, x => x.MapFrom(ent => ent.Contractor.Address ?? ent.Address))
            .ForMember(x => x.Gender, x => x.MapFrom(ent => ent.Person.Gender.FullName))
            .ForMember(x => x.BirthDay, x => x.MapFrom(ent => ent.Person.BirthDate))
            .ForMember(x => x.PhoneNumber, x => x.MapFrom(ent => ent.PhoneNumber))
            //.ForMember(x => x.Contractor, x => x.MapFrom(ent => ent.Contractor))
            //.ForMember(x => x.Person, x => x.MapFrom(ent => ent.Person))
            .ForMember(x => x.PersonFullName, x => x.MapFrom(ent =>
                ent.PersonId != null
                ? ent.Person.FullName
                : ent.PersonFullName))
        ;
}
