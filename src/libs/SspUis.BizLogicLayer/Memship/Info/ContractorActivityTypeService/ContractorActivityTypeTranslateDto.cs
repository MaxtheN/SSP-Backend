using AutoMapper;
using GenericServices;
using GenericServices.Configuration;
using SspUis.DataLayer.EfClasses;
using SspUis.DataLayer.Repositories;
using System;

namespace SspUis.BizLogicLayer.Hrm.ContractorActivityTypeServices
{
	public class ContractorActivityTypeTranslateDto :
		ContractorActivityTypeTranslateDlDto, ILinkToEntity<ContractorActivityTypeTranslate>
	{
		public string Language { get; set; }
	}

	public class ContractorActivityTypeTranslateDtoConfig : PerDtoConfig<ContractorActivityTypeTranslateDto, ContractorActivityTypeTranslate>
	{
		public override Action<IMappingExpression<ContractorActivityTypeTranslate, ContractorActivityTypeTranslateDto>> AlterReadMapping =>
			cfg => cfg
				.IncludeBase<ContractorActivityTypeTranslate, ContractorActivityTypeTranslateDlDto>()
				.ForMember(x => x.Language, x => x.MapFrom(ent => ent.Language.FullName));
	}
}
