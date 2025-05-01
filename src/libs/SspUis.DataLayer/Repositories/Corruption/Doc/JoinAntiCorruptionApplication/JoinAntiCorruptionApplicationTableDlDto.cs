using GenericServices;
using System;
using WEBASE.EF;
using WEBASE.Models;
using WEBASE.Attributes;
using SspUis.DataLayer.EfClasses;

namespace SspUis.DataLayer.Repositories
{
    public class JoinAntiCorruptionApplicationTableDlDto : EntityDto<JoinAntiCorruptionApplicationTableDlDto, JoinAntiCorruptionApplicationTable>, 
        IHaveIdProp<long>, ILinkToEntity<JoinAntiCorruptionApplicationTable>
    {
        public long Id { get; set; }
        [LocalizedRequired]
        [LocalizedRange(1, long.MaxValue)]
        public int OrderNumber { get; set; }
        [LocalizedRequired]
        [LocalizedStringLength(2048)]
        public string Measures { get; set; }
        public DateOnly ExpireOn { get; set; }
        [LocalizedRequired]
        [LocalizedStringLength(250)]
        public string ResponsibleFio { get; set; }
        [LocalizedRequired]
        [LocalizedStringLength(2048)]
        public string MeasuresResult { get; set; }
    }
}
