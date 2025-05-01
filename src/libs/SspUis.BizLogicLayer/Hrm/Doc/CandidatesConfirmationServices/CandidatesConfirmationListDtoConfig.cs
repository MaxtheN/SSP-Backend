using AutoMapper;
using GenericServices.Configuration;
using SspUis.Core;
using SspUis.Core.Security;
using SspUis.DataLayer;
using SspUis.DataLayer.EfClasses;
using System;
using System.Linq;

namespace SspUis.BizLogicLayer
{
    public class CandidatesConfirmationListDtoConfig : PerDtoConfig<CandidatesConfirmationListDto, CandidatesConfirmation>
    {
        public override Action<IMappingExpression<CandidatesConfirmation, CandidatesConfirmationListDto>> AlterReadMapping =>
        cfg => cfg
            .ForMember(d => d.Status, c => c.MapFrom(e => e.Status.Translates.AsQueryable()
                .FirstOrDefault(StatusTranslate.GetExpr(DataLayer.TranslateColumn.full_name, ServiceProvider.CultureHelper.CurrentCulture.Id))
                .TranslateText ?? e.Status.FullName))
            .ForMember(x => x.Organization, x => x.MapFrom(ent => ent.Organization.Translates.AsQueryable()
				.FirstOrDefault(OrganizationTranslate.GetExpr(TranslateColumn.full_name, ServiceProvider.CultureHelper.CurrentCulture.Id)).TranslateText ?? ent.Organization.FullName))

			.ForMember(x => x.Department, x => x.MapFrom(ent => ent.Department.FullName))
            .ForMember(x => x.Position, x => x.MapFrom(ent => ent.Position.FullName))

              .ForMember(x => x.CanCancel, x => x.MapFrom(ent => ServiceProvider.AuthService.HasPermission(ModuleCode.CandidatesConfirmationCancel) != null ? StatusIdConst.CanCandidateConfirmationStatus(ent.StatusId, StatusIdConst.NOT_ACCEPTED) : false))
               .ForMember(x => x.CanSend, x => x.MapFrom(ent => ServiceProvider.AuthService.HasPermission(ModuleCode.CandidatesConfirmationSend) != null ? StatusIdConst.CanCandidateConfirmationStatus(ent.StatusId, StatusIdConst.SENT_FOR_REVIEW) : false))
             .ForMember(x => x.CanEdit, x => x.MapFrom(ent => ServiceProvider.AuthService.HasPermission(ModuleCode.CandidatesConfirmationEdit) != null ? StatusIdConst.CanCandidateConfirmationStatus(ent.StatusId, StatusIdConst.MODIFIED) : false))
             .ForMember(x => x.CanDelete, x => x.MapFrom(ent => ServiceProvider.AuthService.HasPermission(ModuleCode.CandidatesConfirmationDelete) != null ? StatusIdConst.CanCandidateConfirmationStatus(ent.StatusId, StatusIdConst.DELETED) : false));
    }
}