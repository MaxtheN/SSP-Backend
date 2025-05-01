using AutoMapper;
using SspUis.DataLayer.EfClasses;
using SspUis.DataLayer.EfClasses.Hrm;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WEBASE;
using WEBASE.Attributes;
using WEBASE.EF;

namespace SspUis.DataLayer.Repositories
{
    public class CalculationKindDlDto<TDto> : EntityDto<TDto, CalculationKind>
        where TDto : CalculationKindDlDto<TDto>
    {
        [LocalizedStringLength(50)]
        public string OrderCode { get; set; }
        [LocalizedRequired]
        [LocalizedStringLength(50)]
        public string Code { get; set; }
        [LocalizedRequired]
        [LocalizedStringLength(250)]
        public string ShortName { get; set; }
        [LocalizedRequired]
        [LocalizedStringLength(500)]
        public string FullName { get; set; }
        [LocalizedRequired]
        [LocalizedStringLength(600)]
        public string NormativeDoc { get; set; }
        public int? ItemOfExpenseId { get; set; }
        [LocalizedRequired]
        [LocalizedRange(1, int.MaxValue)]
        public int CalculationTypeId { get; set; }
        public int? CalculationMethodId { get; set; }
        public int? MinimumValueTypeId { get; set; }
        public int? CalculateByTimeTypeId { get; set; }
        [LocalizedRequired]
        public bool DependOnRate { get; set; }
        [LocalizedRequired]
        public bool ByEnrolment { get; set; }
        [LocalizedRequired]
        public bool IsMandatory { get; set; }

        public List<CalculationKindTranslateDlDto> Translates { get; set; } = new List<CalculationKindTranslateDlDto>();
        public List<CalculationKindUsedTableDlDto> UsedTables { get; set; } = new List<CalculationKindUsedTableDlDto>();
        public List<CalculationKindPercentDlDto> Percents { get; set; } = new List<CalculationKindPercentDlDto>();
        public List<CalculationKindAllowedDocDlDto> AllowedDocs { get; set; } = new List<CalculationKindAllowedDocDlDto>();
        public List<CalculationKindStructureDlDto> CalculationStructure { get; set; } = new List<CalculationKindStructureDlDto>();

        protected override Action<IMappingExpression<TDto, CalculationKind>> AlterMapping => cfg => cfg
            .ForMember(x => x.Translates, x => x.Ignore())
            .ForMember(x => x.UsedTables, x => x.Ignore())
            .ForMember(x => x.Percents, x => x.Ignore())
            .ForMember(x => x.AllowedDocs, x => x.Ignore())
            .ForMember(x => x.CalculationStructure, x => x.Ignore());

        public override CalculationKind CreateEntity()
        {
            var entity = base.CreateEntity();
            entity.StateId = StateIdConst.ACTIVE;
            Translates.AddByUniqueFKTo(entity.Translates);
            UsedTables.AddTo(entity.UsedTables);
            Percents.AddTo(entity.Percents);
            AllowedDocs.AddTo(entity.AllowedDocs);
            CalculationStructure.AddTo(entity.CalculationStructure);
            return entity;
        }

        public override void UpdateEntity(CalculationKind entity)
        {
            base.UpdateEntity(entity);
            Translates.AddByUniqueFKTo(entity.Translates); 
            UsedTables.ApplyChangesTo<long, CalculationKindUsedTableDlDto, CalculationKindUsedTable>(entity.UsedTables);
            Percents.ApplyChangesTo<long, CalculationKindPercentDlDto, CalculationKindPercent>(entity.Percents);
            AllowedDocs.ApplyChangesTo<long, CalculationKindAllowedDocDlDto, CalculationKindAllowedDoc>(entity.AllowedDocs);
            CalculationStructure.ApplyChangesTo<long, CalculationKindStructureDlDto, CalculationKindStructure>(entity.CalculationStructure);
        }
    }
}
