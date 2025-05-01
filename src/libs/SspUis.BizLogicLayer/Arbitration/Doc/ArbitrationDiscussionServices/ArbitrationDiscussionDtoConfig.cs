using AutoMapper;
using GenericServices.Configuration;
using SspUis.DataLayer;
using SspUis.DataLayer.EfClasses;
using System;
using System.Linq;

namespace SspUis.BizLogicLayer;

public class ArbitrationDiscussionDtoConfig : PerDtoConfig<ArbitrationDiscussionDto, ArbitrationDiscussion>
{
	public override Action<IMappingExpression<ArbitrationDiscussion, ArbitrationDiscussionDto>> AlterReadMapping =>
	   cfg => cfg
		 .ForMember(x => x.Status, x => x.MapFrom(ent => ent.Status.Translates.AsQueryable()
				.FirstOrDefault(StatusTranslate.GetExpr(TranslateColumn.full_name, ServiceProvider.CultureHelper.CurrentCulture.Id)).TranslateText ?? ent.Status.FullName))
		  .ForMember(x => x.ArbitrationStep, x => x.MapFrom(ent => ent.ArbitrationCourtApplication.Application.CurrentStep.FullName))
		  .ForMember(x => x.ResponsibleContractorName, x => x.MapFrom(ent => ent.ArbitrationCourtApplication.ResponsibleContractor.FullName))
		  .ForMember(x => x.ResponsibleContractorId, x => x.MapFrom(ent => ent.ArbitrationCourtApplication.ResponsibleContractorId))
		  .ForMember(x => x.ResponsibleContractorInnPinfl, x => x.MapFrom(ent => ent.ArbitrationCourtApplication.ResponsibleContractor.Inn?? ent.ResponsibleContractor.Pinfl ))
		  .ForMember(x => x.ContractorId, x => x.MapFrom(ent => ent.ArbitrationCourtApplication.Application.ContractorId))
		  //.ForMember(x => x.Files, x => x.MapFrom(ent => ent.Files))
		  //.ForMember(x => x.Signs, x => x.MapFrom(ent => ent.Signs))
		   .ForMember(x => x.ContractorName, x => x.MapFrom(ent => ent.ArbitrationCourtApplication.Application.Contractor.FullName))
	   .ForMember(x => x.ContractorInnPinfl, x => x.MapFrom(ent => ent.ArbitrationCourtApplication.Application.Contractor.Inn?? ent.ArbitrationCourtApplication.ResponsibleContractor.Pinfl))
	   ;
}
