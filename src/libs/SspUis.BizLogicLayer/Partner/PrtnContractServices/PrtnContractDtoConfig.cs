using GenericServices;
using SspUis.DataLayer.EfClasses;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WEBASE.Attributes;
using GenericServices.Configuration;
using AutoMapper;
using WEBASE.Utility;
using SspUis.DataLayer;
using WEBASE;
using System.Linq.Dynamic.Core;
using SspUis.Core;

namespace SspUis.BizLogicLayer.PrtnContractServices
{
    public class PrtnContractDtoConfig : PerDtoConfig<PrtnContractDto, PrtnContract>
    {
        public override Action<IMappingExpression<PrtnContract, PrtnContractDto>> AlterReadMapping =>
            cfg => cfg
                .ForMember(x => x.Status, x => x.MapFrom(ent => ent.Status.Translates.AsQueryable()
                    .FirstOrDefault(StatusTranslate.GetExpr(TranslateColumn.full_name, ServiceProvider.CultureHelper.CurrentCulture.Id)).TranslateText ?? ent.Status.FullName))
                .ForMember(x => x.Contractor, x => x.MapFrom(ent => ent.Contractor.FullName))
                .ForMember(x => x.ContractorInn, x => x.MapFrom(ent => ent.Contractor.Inn))
                .ForMember(x => x.ContractorRegionSoato, x => x.MapFrom(ent => ent.Contractor.Region.Soato))
                .ForMember(x => x.PrtnContractType, x => x.MapFrom(ent => ent.PrtnContractType.Translates.AsQueryable()
                    .FirstOrDefault(PrtnContractTypeTranslate.GetExpr(TranslateColumn.full_name, ServiceProvider.CultureHelper.CurrentCulture.Id)).TranslateText ?? ent.PrtnContractType.FullName))
                .ForMember(x => x.ApplicationDocNumber, x => x.MapFrom(ent => ent.Application.DocNumber))
                .ForMember(x => x.ApplicationDocOn, x => x.MapFrom(ent => ent.Application.DocOn))
                .ForMember(x => x.PrtnCertificateId, x => x.MapFrom(ent => ent.PrtnCertificate.Id))
                .ForMember(x => x.PrtnCertificateStatusId, x => x.MapFrom(ent => ent.PrtnCertificate.StatusId))
                .ForMember(x => x.PrtnCertificateStatus, x => x.MapFrom(ent => ent.PrtnCertificate.Status.Translates.AsQueryable()
                    .FirstOrDefault(StatusTranslate.GetExpr(TranslateColumn.full_name, ServiceProvider.CultureHelper.CurrentCulture.Id)).TranslateText ?? ent.PrtnCertificate.Status.FullName))
                .ForMember(x => x.CurrentPrtnContractSignId, x => x.MapFrom(ent => ent.Signs.Where(a => !a.IsSigned).OrderBy(a => a.PrtnContractTypeTable.OrderNumber).FirstOrDefault().Id))
            .ForMember(x => x.CurrentPrtnContractSignPinfl, x => x.MapFrom(ent => ent.Signs.Where(a => !a.IsSigned).OrderBy(a => a.PrtnContractTypeTable.OrderNumber).FirstOrDefault().OrganizationSign.Pinfl))
                .ForMember(x => x.Message, x => x.MapFrom(ent => ent.Message))
            ;
    }
}
