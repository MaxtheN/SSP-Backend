using System;
using System.Linq;
using AutoMapper;
using GenericServices.Configuration;
using SspUis.Core;
using SspUis.DataLayer.EfClasses;

namespace SspUis.BizLogicLayer
{
    public class SrvContractDtoConfig : PerDtoConfig<SrvContractDto, ServiceContract>
    {
        public override Action<IMappingExpression<ServiceContract, SrvContractDto>> AlterReadMapping =>
        cfg => cfg
            .ForMember(d => d.Status, c => c.MapFrom(e => e.Status.Translates.AsQueryable()
                .FirstOrDefault(StatusTranslate.GetExpr(DataLayer.TranslateColumn.full_name, ServiceProvider.CultureHelper.CurrentCulture.Id))
                .TranslateText ?? e.Status.FullName))
            .ForMember(x => x.ContractorInn, x => x.MapFrom(ent => ent.Contractor.Inn))
            .ForMember(x => x.ContractorFullName, x => x.MapFrom(ent => ent.Contractor.FullName))
            .ForMember(x => x.ContractorRegion, x => x.MapFrom(ent => ent.Contractor.Region.FullName))
            .ForMember(x => x.ContractorDistrict, x => x.MapFrom(ent => ent.Contractor.District.FullName))
            .ForMember(x => x.TotalPrice, x => x.MapFrom(ent => ent.Groups.SelectMany(gr => gr.Tables).Sum(table => table.Price)))
            .ForMember(x => x.Organization, x => x.MapFrom(ent => ent.Organization.FullName))

            .ForMember(x => x.CanSign, x => x.MapFrom(ent => ServiceProvider.AuthService.Contractor != null
                            ? StatusIdConst.CanApplySrvContractStatus(ent.StatusId, StatusIdConst.SIGNED)
                            : StatusIdConst.CanApplySrvContractStatus(ent.StatusId, StatusIdConst.SIGNING)))

            .ForMember(x => x.CanReject, x => x.MapFrom(ent => ServiceProvider.AuthService.Contractor != null
                            ? StatusIdConst.CanApplySrvContractStatus(ent.StatusId, StatusIdConst.REJECTED)
                            : false))
            //.ForMember(x => x.CanReject, x => x.MapFrom(ent => ServiceProvider.AuthService.Contractor != null
            //                ? false
            //                : StatusIdConst.CanApplySrvContractStatus(ent.StatusId, StatusIdConst.REJECTED)))

            .ForMember(x => x.CanCancel, x => x.MapFrom(ent => ServiceProvider.AuthService.Contractor != null
                            ? false
                            : StatusIdConst.CanApplySrvContractStatus(ent.StatusId, StatusIdConst.CANCELED)))
            //.ForMember(x => x.CanCancel, x => x.MapFrom(ent => ServiceProvider.AuthService.Contractor != null
            //                ? StatusIdConst.CanApplySrvContractStatus(ent.StatusId, StatusIdConst.CANCELED)
            //                : false))

            .ForMember(x => x.CanCreatePaymentOrder, x => x.MapFrom(ent => ServiceProvider.AuthService.Contractor != null
                            ? ent.StatusId == StatusIdConst.SIGNED
                            : false));
    }
}
