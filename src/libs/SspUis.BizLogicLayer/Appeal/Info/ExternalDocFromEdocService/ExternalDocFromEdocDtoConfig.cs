using AutoMapper;
using GenericServices.Configuration;
using SspUis.BizLogicLayer.Appeal.Info.ExternalDocFromEdocService;
using SspUis.DataLayer;
using SspUis.DataLayer.EfClasses;
using System;
using System.Linq;

namespace SspUis.BizLogicLayer;

public class ExternalDocFromEdocDtoConfig : PerDtoConfig<ExternalDocFromEdocDto, ExternalDocumentFromEdoc>
{
    public override Action<IMappingExpression<ExternalDocumentFromEdoc, ExternalDocFromEdocDto>> AlterReadMapping => cfg => cfg
     .ForMember(x => x.RegNumber, x => x.MapFrom(ent => ent.RegNumber))
     .ForMember(x => x.Organization, x => x.MapFrom(ent => ent.Organization.Translates.AsQueryable()
                             .FirstOrDefault(OrganizationTranslate.GetExpr(TranslateColumn.full_name, ServiceProvider.CultureHelper.CurrentCulture.Id))
                                        .TranslateText ?? ent.Organization.FullName))
            ;
}

