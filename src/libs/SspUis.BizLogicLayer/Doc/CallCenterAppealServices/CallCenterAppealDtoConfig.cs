using System;
using System.Linq;
using AutoMapper;
using GenericServices.Configuration;
using SspUis.DataLayer;
using SspUis.DataLayer.EfClasses;
using SspUis.DataLayer.EfClasses.Appeal;

namespace SspUis.BizLogicLayer.Appeal;

public class CallCenterAppealDtoConfig : PerDtoConfig<CallCenterAppealDto, CallCenterAppeal>
{
    public override Action<IMappingExpression<CallCenterAppeal, CallCenterAppealDto>> AlterReadMapping =>
        cfg => cfg

            .ForMember(d => d.AppealType, c => c.MapFrom(e => e.AppealType.FullName))
            .ForMember(d => d.ContractorInn, c => c.MapFrom(e => e.Contractor.Pinfl ?? e.Contractor.Inn))
            .ForMember(d => d.PersonFullName, c => c.MapFrom(e => e.PersonFullName))
            .ForMember(d => d.Details, c => c.MapFrom(e => e.Details))
            .ForMember(d => d.PhoneNumber, c => c.MapFrom(e => e.Phonenumber ?? e.Contractor.PhoneNumber))
            .ForMember(d => d.AppealFormatType, c => c.MapFrom(e => e.AppealFormatType.FullName))
            .ForMember(d => d.Pinfl, c => c.MapFrom(e => e.Person.Pinfl))
            .ForMember(d => d.PassportSeria, c => c.MapFrom(e => e.Person.PassportSeria))
            .ForMember(d => d.PassportNumber, c => c.MapFrom(e => e.Person.PassportNumber))
            .ForMember(d => d.EdocInfo, c => c.Ignore())
            .ForMember(d => d.OkedCode, c => c.MapFrom(e => e.Oked.Code))
            .ForMember(d => d.ContractorCategory, c => c.MapFrom(e => e.ContractorCategory.FullName))
            .ForMember(d => d.Status, c => c.MapFrom(e => e.Status.Translates.AsQueryable()
                .FirstOrDefault(StatusTranslate.GetExpr(TranslateColumn.full_name, ServiceProvider.CultureHelper.CurrentCulture.Id))
                    .TranslateText ?? e.Status.FullName))
             .ForMember(d => d.Oked, c => c.MapFrom(e => e.Oked.Translates.AsQueryable()
                .FirstOrDefault(OkedTranslate.GetExpr(TranslateColumn.full_name, ServiceProvider.CultureHelper.CurrentCulture.Id))
                    .TranslateText ?? e.Oked.FullName))
            .ForMember(d => d.BusinessType, c => c.MapFrom(e => e.Busyness ? "xa" : "yo'q"))
            .ForMember(d => d.AppealDescription, c => c.MapFrom(e => e.AppealDescription.Translates.AsQueryable()
                .FirstOrDefault(AppealDescriptionTranslate.GetExpr(TranslateColumn.full_name, ServiceProvider.CultureHelper.CurrentCulture.Id))
                    .TranslateText ?? e.AppealDescription.FullName))

            .ForMember(d => d.AppealTypeArrive, c => c.MapFrom(e => e.AppealTypeArrive.Translates.AsQueryable()
                .FirstOrDefault(AppealTypeArriveTranslate.GetExpr(TranslateColumn.full_name, ServiceProvider.CultureHelper.CurrentCulture.Id))
                    .TranslateText ?? e.AppealTypeArrive.FullName))

            .ForMember(x => x.Region, x => x.MapFrom(ent => ent.Contractor.Region.Translates.AsQueryable()
                .FirstOrDefault(RegionTranslate.GetExpr(TranslateColumn.full_name, ServiceProvider.CultureHelper.CurrentCulture.Id))
                    .TranslateText ?? ent.Region.FullName))

            .ForMember(x => x.District, x => x.MapFrom(ent => ent.Contractor.District.Translates.AsQueryable()
                .FirstOrDefault(DistrictTranslate.GetExpr(TranslateColumn.full_name, ServiceProvider.CultureHelper.CurrentCulture.Id))
                    .TranslateText ?? ent.District.FullName))
            .ForMember(x => x.ContractorFulName, x => x.MapFrom(ent => ent.Contractor.FullName))
            .ForMember(x => x.Director, x => x.MapFrom(ent => ent.Contractor.Director))
            .ForMember(x => x.Address, x => x.MapFrom(ent => ent.Contractor.Address ?? ent.Address))
            .ForMember(x => x.ImportOrExpot, x => x.MapFrom(ent => ent.Isexporter ? "expot qiluvchi" : ent.Isimporter ? "import qiluvchi" : " "))
            .ForMember(x => x.Gender, x => x.MapFrom(ent => ent.Person.Gender.FullName))
            .ForMember(x => x.BirthDay, x => x.MapFrom(ent => ent.Person.BirthDate))
            .ForMember(x => x.PersonFullName, x => x.MapFrom(ent =>
                ent.PersonId != null
                ? ent.Person.FullName
                : ent.PersonFullName))
        ;
}
