using AutoMapper;
using GenericServices.Configuration;
using SspUis.DataLayer.EfClasses;
using System;
using System.Linq;

namespace SspUis.BizLogicLayer
{
    public class CandidatesConfirmationTableDtoConfig : PerDtoConfig<CandidatesConfirmationTableDto, CandidatesConfirmationTable>
    {
        public override Action<IMappingExpression<CandidatesConfirmationTable, CandidatesConfirmationTableDto>> AlterReadMapping =>
         cfg => cfg
                .ForMember(d => d.Status, c => c.MapFrom(e => e.Status.Translates.AsQueryable()
                    .FirstOrDefault(StatusTranslate.GetExpr(DataLayer.TranslateColumn.full_name, ServiceProvider.CultureHelper.CurrentCulture.Id))
                    .TranslateText ?? e.Status.FullName))

                .ForMember(d => d.Employee, c => c.MapFrom(e => e.Employee.Person.FullName))
                .ForMember(x => x.Files, x => x.MapFrom(ent => ent.Files));
    }
}