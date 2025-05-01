using AutoMapper;
using GenericServices.Configuration;
using SspUis.DataLayer.EfClasses;
using SspUis.DataLayer.EfClasses.DualEdu;
using System;
using System.Linq;

namespace SspUis.BizLogicLayer.DualEdu;

public class DualContractListDtoConfig : PerDtoConfig<DualContractListDto, DualContract>
{
    public override Action<IMappingExpression<DualContract, DualContractListDto>> AlterReadMapping =>
    cfg => cfg
        .ForMember(d => d.ContracttorFullName, c => c.MapFrom(e => e.Application.Contractor.FullName))
        .ForMember(d => d.ContracttorInn, c => c.MapFrom(e => e.Application.Contractor.Inn))
        .ForMember(d => d.DocNumber, c => c.MapFrom(e => e.DocNumber))
        .ForMember(d => d.DocDate, c => c.MapFrom(e => e.DocDate))
        .ForMember(d=>d.Id2 , c=>c.MapFrom(c => c.Id2))
        .ForMember(d => d.Status, c => c.MapFrom(e => e.Status.Translates.AsQueryable()
            .FirstOrDefault(StatusTranslate.GetExpr(DataLayer.TranslateColumn.full_name, ServiceProvider.CultureHelper.CurrentCulture.Id))
            .TranslateText ?? e.Status.FullName))
        .ForMember(d => d.Speciality, c => c.MapFrom(e => e.Speciality.FullName))
        .ForMember(d => d.EduType, c => c.MapFrom(e => e.EduType.Translates.AsQueryable()
            .FirstOrDefault(EduTypeTranslate.GetExpr(DataLayer.TranslateColumn.full_name, ServiceProvider.CultureHelper.CurrentCulture.Id))
            .TranslateText ?? e.EduType.FullName))
        .ForMember(d => d.Institute, c => c.MapFrom(e => e.Institute.FullName))
        .ForMember(d => d.DualEducationType, c => c.MapFrom(e => e.DualEducationType.Translates.AsQueryable()
            .FirstOrDefault(DualEducationTypeTranslate.GetExpr(DataLayer.TranslateColumn.full_name, ServiceProvider.CultureHelper.CurrentCulture.Id))
            .TranslateText ?? e.DualEducationType.FullName))
    ;
}