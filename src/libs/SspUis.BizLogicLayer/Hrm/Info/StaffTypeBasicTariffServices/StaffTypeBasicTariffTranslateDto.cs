using AutoMapper;
using GenericServices;
using GenericServices.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SspUis.DataLayer.EfClasses;
using SspUis.DataLayer.Repositories;
using SspUis.DataLayer.EfClasses.Hrm;

namespace SspUis.BizLogicLayer.Hrm.StaffTypeBasicTariffServices;

public class StaffTypeBasicTariffTranslateDto : StaffTypeBasicTariffTranslateDlDto, ILinkToEntity<StaffTypeBasicTariffTranslate>
{
    public string Language { get; set; }
}
public class StaffTypeBasicTariffTranslateDtoConfig : PerDtoConfig<StaffTypeBasicTariffTranslateDto, StaffTypeBasicTariffTranslate>
{
    public override Action<IMappingExpression<StaffTypeBasicTariffTranslate, StaffTypeBasicTariffTranslateDto>> AlterReadMapping => cfg => cfg
            .IncludeBase<StaffTypeBasicTariffTranslate, StaffTypeBasicTariffTranslateDlDto>()
            .ForMember(x => x.Language, x => x.MapFrom(ent => ent.Language.FullName));
}
