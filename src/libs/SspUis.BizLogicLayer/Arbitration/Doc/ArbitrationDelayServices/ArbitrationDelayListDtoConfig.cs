using System;
using System.Linq;
using AutoMapper;
using GenericServices.Configuration;
using SspUis.Core;
using SspUis.DataLayer;
using SspUis.DataLayer.EfClasses;

namespace SspUis.BizLogicLayer;

public class ArbitrationDelayListDtoConfig : PerDtoConfig<ArbitrationDelayListDto, ArbitrationDelay>
{
    public override Action<IMappingExpression<ArbitrationDelay, ArbitrationDelayListDto>> AlterReadMapping =>
        cfg => cfg
        .ForMember(x => x.Contractor, c => c.MapFrom(ent => ent.ArbitrationCourtApplication.IsForeignContractor ? ent.ArbitrationCourtApplication.ForeignContractorName : ent.Contractor.FullName))
        .ForMember(x => x.ContractorInn, c => c.MapFrom(ent => ent.Contractor.Inn))
        .ForMember(x => x.ResponsibleContractor, c => c.MapFrom(ent => ent.ArbitrationCourtApplication.IsForeignResponsible ? ent.ArbitrationCourtApplication.ForeignResponsibleName : ent.ResponsibleContractor.FullName))
        .ForMember(x => x.ResponsibleContractorInn, c => c.MapFrom(ent => ent.ResponsibleContractor.Inn))
        .ForMember(x => x.ResponsibleContractorId, c => c.MapFrom(ent => ent.ResponsibleContractorId))
        .ForMember(x => x.ContractorId, c => c.MapFrom(ent => ent.ContractorId))
        .ForMember(x => x.ArbitrationCourtApplicationId, c => c.MapFrom(ent => ent.ArbitrationCourtApplicationId))
        .ForMember(x => x.OrganizationId, c => c.MapFrom(ent => ent.ArbitrationCourtApplication.OrganizationId))
        .ForMember(x => x.Organization, c => c.MapFrom(ent => ent.ArbitrationCourtApplication.Organization.FullName))
        .ForMember(x => x.Status, c => c.MapFrom(ent => ent.Status.Translates.AsQueryable()
            .FirstOrDefault(StatusTranslate.GetExpr(TranslateColumn.full_name, ServiceProvider.CultureHelper.CurrentCulture.Id)).TranslateText ?? ent.Status.FullName))

        .ForMember(x => x.CanDelete, c => c.MapFrom(ent => (ServiceProvider.AuthService.Contractor == null)
            ? StatusIdConst.CanArbitrationDelayApplyStatus(ent.StatusId, StatusIdConst.DELETED)
            : false
        ))

        .ForMember(x => x.CanEdit, c => c.MapFrom(ent => (ServiceProvider.AuthService.Contractor == null)
            ? StatusIdConst.CanArbitrationDelayApplyStatus(ent.StatusId, StatusIdConst.MODIFIED)
            : false
        ))

        .ForMember(x => x.CanSign, c => c.MapFrom(ent => (ServiceProvider.AuthService.Contractor == null)
            ? StatusIdConst.CanArbitrationDelayApplyStatus(ent.StatusId, StatusIdConst.SIGNED)
            : false
        ))
        ;
}
