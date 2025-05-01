using System;
using System.Linq;
using AutoMapper;
using GenericServices.Configuration;
using SspUis.DataLayer;
using SspUis.DataLayer.EfClasses;

namespace SspUis.BizLogicLayer;

public class ArbitrationDiscussionSignDtoConfig : PerDtoConfig<ArbitrationDiscussionSignDto, ArbitrationDiscussionSign>
{
    public override Action<IMappingExpression<ArbitrationDiscussionSign, ArbitrationDiscussionSignDto>> AlterReadMapping =>
       cfg => cfg
         .ForMember(x => x.Status, x => x.MapFrom(ent => ent.Status.Translates.AsQueryable()
                .FirstOrDefault(StatusTranslate.GetExpr(TranslateColumn.full_name, ServiceProvider.CultureHelper.CurrentCulture.Id)).TranslateText ?? ent.Status.FullName))
          .ForMember(x => x.FirstName, x => x.MapFrom(ent => ent.CreatedUser.Person.NameLatin))
          .ForMember(x => x.LastName, x => x.MapFrom(ent => ent.CreatedUser.Person.SurnameLatin))
          .ForMember(x => x.MiddleName, x => x.MapFrom(ent => ent.CreatedUser.Person.PatronymLatin))
          .ForMember(x => x.PositionName, x => x.MapFrom(ent => ent.CreatedUser.EmployeeManage.Position.FullName))
          ;
}
