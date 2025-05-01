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

namespace SspUis.BizLogicLayer.PrtnCertificateServices
{
    public class PrtnCertificateDtoConfig : PerDtoConfig<PrtnCertificateDto, PrtnCertificate>
    {
        public override Action<IMappingExpression<PrtnCertificate, PrtnCertificateDto>> AlterReadMapping =>
            cfg => cfg
                .ForMember(x => x.Status, x => x.MapFrom(ent => ent.Status.Translates.AsQueryable()
                    .FirstOrDefault(StatusTranslate.GetExpr(TranslateColumn.full_name, ServiceProvider.CultureHelper.CurrentCulture.Id)).TranslateText ?? ent.Status.FullName))
                .ForMember(x => x.Contractor, x => x.MapFrom(ent => ent.Contractor.FullName))
                .ForMember(x => x.ContractorInn, x => x.MapFrom(ent => ent.Contractor.Inn))
                .ForMember(x => x.PrtnContractType, x => x.MapFrom(ent => ent.PrtnContractType.Translates.AsQueryable()
                    .FirstOrDefault(PrtnContractTypeTranslate.GetExpr(TranslateColumn.full_name, ServiceProvider.CultureHelper.CurrentCulture.Id)).TranslateText ?? ent.PrtnContractType.FullName))
                .ForMember(x => x.Contractor, x => x.MapFrom(ent => ent.Contractor.FullName))
                .ForMember(x => x.ContractorInn, x => x.MapFrom(ent => ent.Contractor.Inn))
            ;

    }
}
