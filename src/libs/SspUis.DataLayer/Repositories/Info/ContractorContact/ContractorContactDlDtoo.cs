using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using DocumentFormat.OpenXml.Bibliography;
using SspUis.DataLayer.EfClasses;
using WEBASE;
using WEBASE.Attributes;
using WEBASE.EF;
using WEBASE.Models;

namespace SspUis.DataLayer.Repositories
{
    public class ContractorContactDlDtoo<TDto>: EntityDto<TDto, ContractorContact>, IHaveIdProp<long>
        where TDto : ContractorContactDlDtoo<TDto>
    {
        
        [LocalizedRequired]
        [LocalizedStringLength(500)]
        public string Contact { get; set; }
        [LocalizedRequired]
        [LocalizedRange(1, int.MaxValue)]
        public int ContactTypeId { get; set; }
        [LocalizedRequired]
        [LocalizedRange(1, int.MaxValue)]
        public int OwnerId { get; set; }
        [JsonIgnore]
        public long Id { get; set; }

        public override ContractorContact CreateEntity()
        {
            var data = base.CreateEntity();
            return data;
        }

        public override void UpdateEntity(ContractorContact entity)
        {
            base.UpdateEntity(entity);
        }
    }
}



