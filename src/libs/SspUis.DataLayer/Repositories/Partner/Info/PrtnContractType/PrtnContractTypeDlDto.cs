using AutoMapper;
using SspUis.DataLayer.EfClasses;
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
    public class PrtnContractTypeDlDto<TDto> : EntityDto<TDto, PrtnContractType>
        where TDto : PrtnContractTypeDlDto<TDto>
    {
        [LocalizedStringLength(50)]
        public string OrderCode { get; set; }
        [LocalizedRequired]
        [LocalizedStringLength(250)]
        public string ShortName { get; set; }
        [LocalizedRequired]
        [LocalizedStringLength(250)]
        public string FullName { get; set; }
        [LocalizedRequired]
        [LocalizedRange(0, int.MaxValue)]
        public int EmployeeRangeFrom { get; set; }
        public int? EmployeeRangeTo { get; set; }
        [LocalizedRequired]
        [LocalizedRange(1, int.MaxValue)]
        public int CertificatePeriodInYears { get; set; }

        public List<PrtnContractTypeTableDlDto> Tables { get; set; } = new List<PrtnContractTypeTableDlDto>();
        public List<PrtnContractTypeTranslateDlDto> Translates { get; set; } = new List<PrtnContractTypeTranslateDlDto>();

        protected override Action<IMappingExpression<TDto, PrtnContractType>> AlterMapping => cfg => cfg
            .ForMember(x => x.Tables, x => x.Ignore())
            .ForMember(x => x.Translates, x => x.Ignore());

        public override PrtnContractType CreateEntity()
        {
            var entity = base.CreateEntity();  
            entity.StateId = StateIdConst.ACTIVE;
            Tables.AddTo(entity.Tables);
            Translates.AddByUniqueFKTo(entity.Translates);
            return entity;
        }
        public override void UpdateEntity(PrtnContractType entity)
        {
            base.UpdateEntity(entity);
            Tables.ApplyChangesTo<int, PrtnContractTypeTableDlDto, PrtnContractTypeTable>(entity.Tables);
            Translates.ApplyChangesByUniqueFKTo(entity.Translates);
        }
    }
}
