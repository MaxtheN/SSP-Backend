using AutoMapper;
using Newtonsoft.Json;
using SspUis.DataLayer.EfClasses.Hrm;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WEBASE;
using WEBASE.Attributes;
using WEBASE.EF;
using WEBASE.Models;

namespace SspUis.DataLayer.Repositories.Hrm
{
    public class StaffingIndicatorValueDlDto : EntityDto<StaffingIndicatorValueDlDto, StaffingIndicatorValue>, IHaveIdProp<long>
    {
        public long Id { get; set; }
        [LocalizedRequired]
        [LocalizedRange(1, int.MaxValue)]
        public int StaffingIndicatorId { get; set; }
        [LocalizedRequired]
        public decimal Quantity { get; set; }
        public decimal? TotalSum { get; set; }

        public bool CanEdit { get; set; }

        public override StaffingIndicatorValue CreateEntity()
        {
            var entity = base.CreateEntity();
            return entity;
        }

        public override void UpdateEntity(StaffingIndicatorValue entity)
        {
            base.UpdateEntity(entity);
        }
    }
}
