using AutoMapper;
using GenericServices.Configuration;
using SspUis.DataLayer;
using SspUis.DataLayer.EfClasses;
using SspUis.DataLayer.EfClasses.DualEdu;
using System;
using System.Linq;
using System.Linq.Dynamic.Core;
using WEBASE.OfficeTools.Models;

namespace SspUis.BizLogicLayer.DualEdu;

public class DualContractDtoConfig : PerDtoConfig<DualContractDto, DualContract>
{
    public override Action<IMappingExpression<DualContract, DualContractDto>> AlterReadMapping =>
    cfg => cfg
        .ForMember(d => d.Status, c => c.MapFrom(e => e.Status.Translates.AsQueryable()
            .FirstOrDefault(StatusTranslate.GetExpr(DataLayer.TranslateColumn.full_name, ServiceProvider.CultureHelper.CurrentCulture.Id))
            .TranslateText ?? e.Status.FullName))
        .ForMember(d => d.Speciality, c => c.MapFrom(e => e.Speciality.FullName))
        .ForMember(d => d.EduSpeciality, c => c.MapFrom(e => e.Speciality.FullName))
        .ForMember(d => d.EduType, c => c.MapFrom(e => e.EduType.Translates.AsQueryable()
            .FirstOrDefault(EduTypeTranslate.GetExpr(DataLayer.TranslateColumn.full_name, ServiceProvider.CultureHelper.CurrentCulture.Id))
            .TranslateText ?? e.EduType.FullName))
        .ForMember(d => d.Institute, c => c.MapFrom(e => e.Institute.FullName))
        .ForMember(d => d.InstituteInn, c => c.MapFrom(e => e.Institute.Inn ?? ""))
        .ForMember(d => d.DualEducationType, c => c.MapFrom(e => e.DualEducationType.Translates.AsQueryable()
            .FirstOrDefault(DualEducationTypeTranslate.GetExpr(DataLayer.TranslateColumn.full_name, ServiceProvider.CultureHelper.CurrentCulture.Id))
            .TranslateText ?? e.DualEducationType.FullName))
        .ForMember(d => d.Contractor, c => c.MapFrom(e => e.Application.Contractor.FullName))
        .ForMember(d => d.ContractorDirectorNameForSign, c => c.MapFrom(e => e.Application.Contractor.FullName))
        .ForMember(d => d.ContractorOrganizationName, c => c.MapFrom(e => e.Application.Contractor.FullName))
        .ForMember(d => d.StudentNameForSign, c => c.MapFrom(e => e.StudentFullname))
        .ForMember(d => d.RectorNameForSign, c => c.MapFrom(e => e.RektorName))
        .ForMember(d => d.OrganizationTel, c => c.MapFrom(e => e.OrganizationPhoneNumber))
        .ForMember(d => d.CurrentDualContractSignId, c => c.MapFrom(ent => ent.Signs.Where(a => !a.IsSigned).OrderBy(a => a.Owner.Id).FirstOrDefault().Id))
        .ForMember(d => d.CurrentDualContractSignPinfl, c => c.MapFrom(ent => ent.Signs.Where(a => !a.IsSigned).OrderBy(a => a.Owner.Id).FirstOrDefault().Owner.Pinfl))
        .ForMember(d => d.QrSign0, c => c.MapFrom(e => new QrCodeModel()
        {
            Dpi = 512,
            Text = "Buyruq Sanasi:" + e.DocDate + ", \n Buyruq Raqam" + e.DocNumber + ", ",
            Width = 256,
            Height = 256,
        }))
       .ForMember(d => d.QrSignA, c => c.MapFrom(e => new QrCodeModel()
       {
           Dpi = 512,
           Text = "Buyruq Sanasi:" + e.DocDate + ", \n Buyruq Raqam" + e.DocNumber + ", ",
           Width = 256,
           Height = 256,
       }))
        .ForMember(d => d.QrSignB, c => c.MapFrom(e => new QrCodeModel()
        {
            Dpi = 512,
            Text = "Buyruq Sanasi:" + e.DocDate + ", \n Buyruq Raqam" + e.DocNumber + ", ",
            Width = 256,
            Height = 256,
        }))
        .ForMember(d => d.StudentName, c => c.MapFrom(e => e.StudentFullname))
        .ForMember(d => d.StudentNameForSign, c => c.MapFrom(e => e.StudentFullname))
        .ForMember(d => d.PassportInfo, c => c.MapFrom(e => e.PassportSeria + e.PassportNumber))
        .ForMember(d => d.ContractorPhoneNumber, c => c.MapFrom(e => e.Application.Contractor.PhoneNumber))
        .ForMember(d => d.ContractorAddress, c => c.MapFrom(e => e.Application.Contractor.Address))
        .ForMember(d => d.ContractorDirector, c => c.MapFrom(e => e.Application.Contractor.Director))
        .ForMember(d => d.ContractorDirectorNameForSign, c => c.MapFrom(e => e.Application.Contractor.Director))
        .ForMember(d => d.ContractorBankName, c => c.MapFrom(e => e.Application.Contractor.Bank.Translates.AsQueryable()
                    .FirstOrDefault(BankTranslate.GetExpr(BankTranslateColumn.bank_name, ServiceProvider.CultureHelper.CurrentCulture.Id)).TranslateText ?? e.Application.Contractor.Bank.BankName))

    ;
}