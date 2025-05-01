using AutoMapper;
using SspUis.Core;
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
    public class ApplicationDlDto<TDto> : EntityDto<TDto, Application>
        where TDto : ApplicationDlDto<TDto>
    {
        [LocalizedRequired]
        public DateOnly DocOn { get; set; }
        [LocalizedRequired]
        [LocalizedStringLength(50)]
        public string DocNumber { get; set; }
        [LocalizedRequired]
        [LocalizedStringLength(250)]
        public string ContractorPositionName { get; set; }
        [LocalizedRequired]
        [LocalizedRange(1, int.MaxValue)]
        public int ApplicationTypeId { get; set; }
        public long? ContractorSettlementAccountId { get; set; }
        public override Application CreateEntity()
        {
            var entity = base.CreateEntity();
            entity.Id2 = Guid.NewGuid();
            entity.StatusId = StatusIdConst.CREATED;
            entity.TableId = TableIdConst.DOC_APPLICATION;
            return entity;
        }

        public override void UpdateEntity(Application entity)
        {
            base.UpdateEntity(entity);
            entity.StatusId = StatusIdConst.MODIFIED;
        }
    }
}
