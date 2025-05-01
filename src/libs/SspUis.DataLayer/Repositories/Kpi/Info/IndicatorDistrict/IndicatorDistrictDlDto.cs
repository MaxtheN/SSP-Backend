using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using SspUis.DataLayer.EfClasses;
using WEBASE.Attributes;
using WEBASE;
using WEBASE.EF;

namespace SspUis.DataLayer.Repositories
{
    public class IndicatorDistrictDlDto<TDto> :
        EntityDto<TDto, IndicatorDistrict> where TDto : IndicatorDistrictDlDto<TDto>
    {
        [LocalizedRequired]
        [LocalizedStringLength(9)]
        public string Code { get; set; }
        [LocalizedRequired]
        [LocalizedStringLength(50)]
        public string OrderCode { get; set; }
        [LocalizedRequired]
        [LocalizedStringLength(250)]
        public string ShortName { get; set; }
        [LocalizedRequired]
        [LocalizedStringLength(500)]
        public string FullName { get; set; }
        public int? UniteOfMeasureId { get; set; }
        [LocalizedRequired]
        public int DepartmentId { get; set; }
        public int? ParentId { get; set; }
        public List<IndicatorDistrictTranslateDlDto> Translates { get; set; } = new();
        public List<IndicatorDistrictTableDlDto> Tables { get; set; } = new();

        protected override Action<IMappingExpression<TDto, IndicatorDistrict>> AlterMapping => cfg => cfg
                .ForMember(x => x.Tables, x => x.Ignore())
                .ForMember(x => x.Translates, x => x.Ignore());

        public override IndicatorDistrict CreateEntity()
        {
            var res = base.CreateEntity();
            res.StateId = StateIdConst.ACTIVE;
            Tables.AddTo(res.Tables);
            Translates.AddByUniqueFKTo(res.Translates);
            res.CreatedAt = DateTime.Now;
            return res;
        }
        public override void UpdateEntity(IndicatorDistrict entity)
        {
            base.UpdateEntity(entity);
            Translates.ApplyChangesByUniqueFKTo(entity.Translates);
            Tables.ApplyChangesTo<int, IndicatorDistrictTableDlDto, IndicatorDistrictTable>(entity.Tables);
        }
    }
}

