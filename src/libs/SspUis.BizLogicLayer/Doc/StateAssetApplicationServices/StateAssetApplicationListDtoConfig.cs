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

namespace SspUis.BizLogicLayer.ApplicationServices
{
    public class StateAssetApplicationListDtoConfig : PerDtoConfig<StateAssetApplicationListDto, Application>
    {
        public override Action<IMappingExpression<Application, StateAssetApplicationListDto>> AlterReadMapping =>
            cfg => cfg
                .ForMember(x => x.Status, x => x.MapFrom(ent => ent.Status.Translates.AsQueryable()
                    .FirstOrDefault(StatusTranslate.GetExpr(TranslateColumn.full_name, ServiceProvider.CultureHelper.CurrentCulture.Id)).TranslateText ?? ent.Status.FullName))
                .ForMember(x => x.Contractor, x => x.MapFrom(ent => ent.Contractor.FullName))
                .ForMember(x => x.ContractorInn, x => x.MapFrom(ent => ent.Contractor.Inn))
                .ForMember(x => x.ContractorPhoneNumber, x => x.MapFrom(ent => ent.Contractor.BusinessmanUserInContractors.FirstOrDefault(a => a.BusinessmanUserId == ent.CreatedUserId).BusinessmanUser.UserName))
                .ForMember(x => x.Region, x => x.MapFrom(ent => ent.Region.Translates.AsQueryable()
                    .FirstOrDefault(RegionTranslate.GetExpr(TranslateColumn.full_name, ServiceProvider.CultureHelper.CurrentCulture.Id)).TranslateText ?? ent.Region.FullName))
                .ForMember(x => x.District, x => x.MapFrom(ent => ent.District.Translates.AsQueryable()
                    .FirstOrDefault(DistrictTranslate.GetExpr(TranslateColumn.full_name, ServiceProvider.CultureHelper.CurrentCulture.Id)).TranslateText ?? ent.Contractor.FullName))
                .ForMember(x => x.OkedCode, x => x.MapFrom(ent => ent.Contractor.Oked.Code))
                .ForMember(x => x.Oked, x => x.MapFrom(ent => ent.Contractor.Oked.Translates.AsQueryable().FirstOrDefault(OkedTranslate.GetExpr(TranslateColumn.full_name, ServiceProvider.CultureHelper.CurrentCulture.Id)).TranslateText ?? ent.Contractor.Oked.FullName))
                .ForMember(x => x.AuctionDocOn, x => x.MapFrom(ent => ent.StateAssetApplication.AuctionDocOn))
                .ForMember(x => x.AuctionDocNumber, x => x.MapFrom(ent => ent.StateAssetApplication.AuctionDocNumber))
                .ForMember(x => x.StateAssetName, x => x.MapFrom(ent => ent.StateAssetApplication.StateAssetName))
                ;

    }
}
