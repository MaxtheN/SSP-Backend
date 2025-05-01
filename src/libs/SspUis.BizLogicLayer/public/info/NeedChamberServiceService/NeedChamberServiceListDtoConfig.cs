using AutoMapper;
using GenericServices.Configuration;
using SspUis.DataLayer;
using SspUis.DataLayer.EfClasses;
using System;
using System.Linq;

namespace SspUis.BizLogicLayer.Hrm.NeedChamberServiceServices
{
    public class NeedChamberServiceListDtoConfig : PerDtoConfig<NeedChamberServiceListDto, NeedChamberService>
    {
        public override Action<IMappingExpression<NeedChamberService, NeedChamberServiceListDto>> AlterReadMapping => cfg => cfg
        .ForMember(x => x.State, x => x.MapFrom(ent =>
                ent.State.Translates.AsQueryable().FirstOrDefault(
                    StateTranslate.GetExpr(
                        TranslateColumn.full_name,
                        ServiceProvider.CultureHelper.CurrentCulture.Id))
                .TranslateText ?? ent.State.FullName))
        .ForMember(x => x.MeetingType, x => x.MapFrom(ent =>
                ent.MeetingTypes.Translates.AsQueryable().FirstOrDefault(
                    MeetingTypeTranslate.GetExpr(
                        TranslateColumn.full_name,
                        ServiceProvider.CultureHelper.CurrentCulture.Id))
                .TranslateText ?? ent.MeetingTypes.FullName))

        .ForMember(x => x.ServicePriceType, x => x.MapFrom(ent =>
                ent.ServicePriceType.Translates.AsQueryable().FirstOrDefault(
                    ServicePriceTypeTranslate.GetExpr(
                        TranslateColumn.full_name,
                        ServiceProvider.CultureHelper.CurrentCulture.Id))
                .TranslateText ?? ent.ServicePriceType.FullName));
    }
}
