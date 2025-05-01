using SspUis.DataLayer.EfClasses;
using System;
using WEBASE.Attributes;
using WEBASE.EF;
using WEBASE.Models;

namespace SspUis.DataLayer.Repositories
{
    public class ArbitrationCourtApplicationFileDlDto : EntityDto<
        ArbitrationCourtApplicationFileDlDto, 
        ArbitrationCourtApplicationFile>, IHaveIdProp<Guid>
    {
        [LocalizedRequired]
        public Guid Id { get; set; }
        public string ColumnName { get; set; }
        public bool IsCreatedErp { get; set; }
        public bool CanSign { get; set; }
        public int StepId { get; set; }
    }
}
