using AutoMapper;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SspUis.DataLayer.EfClasses;
using WEBASE;
using WEBASE.Attributes;
using WEBASE.EF;
using Minio.DataModel;
using Org.BouncyCastle.Math.EC.Rfc7748;
using Microsoft.EntityFrameworkCore;
using SspUis.DataLayer.Repositories.Info.OrganizationalStructure;

namespace SspUis.DataLayer.Repositories
{
    public class OrganizationalStructureDlDto<TDto> : EntityDto<TDto, OrganizationalStructure>
        where TDto : OrganizationalStructureDlDto<TDto>
    {
        public int? OrderCode { get; set; }
        [LocalizedRequired]
        [LocalizedStringLength(50)]
        public string Code { get; set; } = null!;
        [LocalizedRequired]
        public int StructureType { get; set; }
        [LocalizedRequired]
        [LocalizedStringLength(250)]
        public string ShortName { get; set; } = null!;
        [LocalizedRequired]
        [LocalizedStringLength(500)]
        public string FullName { get; set; } = null!;
        public decimal? CorrCoef { get; set; }

        public List<OrganizationalStructureTranslateDlDto> Translates { get; set; } = new List<OrganizationalStructureTranslateDlDto>();
        public List<OrganizationalStructureCalculationKindDlDto> StructureCalculationKind { get; set; } = new List<OrganizationalStructureCalculationKindDlDto>();
        public List<OrganizationalStructurePositionDlDto> StructurePosition { get; set; } = new List<OrganizationalStructurePositionDlDto>();
        public List<OrganizationalStructureStaffingIndicatorDlDto> StructureStaffingIndicator { get; set; } = new List<OrganizationalStructureStaffingIndicatorDlDto>();
        protected override Action<IMappingExpression<TDto, OrganizationalStructure>> AlterMapping => cfg => cfg
            .ForMember(x => x.Translates, x => x.Ignore())
            .ForMember(x => x.StructureCalculationKind, x => x.Ignore())
            .ForMember(x => x.StructurePosition, x => x.Ignore())
            .ForMember(x => x.StructureStaffingIndicator, x => x.Ignore());

        public override OrganizationalStructure CreateEntity()
        {
            var entity = base.CreateEntity();
            entity.StateId = StateIdConst.ACTIVE;
            Translates.AddByUniqueFKTo(entity.Translates);
            StructureCalculationKind.AddTo(entity.StructureCalculationKind);
            StructurePosition.AddTo(entity.StructurePosition);
            StructureStaffingIndicator.AddTo(entity.StructureStaffingIndicator/*, onAddToNavigationProperty: (a, b) => a.Tables.AddFromForeignKeys(b.IndicatorTables)*/);
            return entity;
        }

        public override void UpdateEntity(OrganizationalStructure entity)
        {
            base.UpdateEntity(entity);
            StructurePosition.ApplyChangesTo<int, OrganizationalStructurePositionDlDto, OrganizationalStructurePosition>(entity.StructurePosition);
            StructureCalculationKind.ApplyChangesTo<int, OrganizationalStructureCalculationKindDlDto, OrganizationalStructureCalculationKind>(entity.StructureCalculationKind);
            StructureStaffingIndicator.ApplyChangesTo<int, OrganizationalStructureStaffingIndicatorDlDto, OrganizationalStructureStaffingIndicator>(entity.StructureStaffingIndicator/*, onUpdateFromNavigationProperty: (a, b) => a.Tables.UpdateFromForeignKeys(b.IndicatorTables)*/);
            Translates.ApplyChangesByUniqueFKTo(entity.Translates);
        }
    }
}

