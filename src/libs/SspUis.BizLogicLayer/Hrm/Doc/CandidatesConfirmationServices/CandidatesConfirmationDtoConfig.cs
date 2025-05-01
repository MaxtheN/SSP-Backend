using AutoMapper;
using GenericServices.Configuration;
using SspUis.DataLayer;
using SspUis.DataLayer.EfClasses;
using System;
using System.Linq;

namespace SspUis.BizLogicLayer
{
    public class CandidatesConfirmationDtoConfig : PerDtoConfig<CandidatesConfirmationDto, CandidatesConfirmation>
    {
        public override Action<IMappingExpression<CandidatesConfirmation, CandidatesConfirmationDto>> AlterReadMapping =>
        cfg => cfg
            .ForMember(d => d.Status, c => c.MapFrom(e => e.Status.Translates.AsQueryable()
                .FirstOrDefault(StatusTranslate.GetExpr(DataLayer.TranslateColumn.full_name, ServiceProvider.CultureHelper.CurrentCulture.Id))
                .TranslateText ?? e.Status.FullName))            
            .ForMember(x => x.Organization, x => x.MapFrom(ent => ent.Organization.Translates.AsQueryable()
				.FirstOrDefault(OrganizationTranslate.GetExpr(TranslateColumn.full_name, ServiceProvider.CultureHelper.CurrentCulture.Id)).TranslateText ?? ent.Organization.FullName))

			.ForMember(x => x.Department, x => x.MapFrom(ent => ent.Department.FullName))
            .ForMember(x => x.Position, x => x.MapFrom(ent => ent.Position.FullName));
    }
}