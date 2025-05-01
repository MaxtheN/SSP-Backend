using WEBASE.Attributes;
using WEBASE.EF;
using WEBASE.Models;
using SspUis.DataLayer.EfClasses;
using GenericServices;
using System.Text.Json.Serialization;

namespace SspUis.DataLayer.Repositories
{
    public class MonoApplicationItemTableDlDto : EntityDto<MonoApplicationItemTableDlDto, MonoApplicationItemTable>,
        IHaveIdProp<long>,
        ILinkToEntity<MonoApplicationItemTable>
    {
        public long Id { get; set; }
        [LocalizedRequired]
        [LocalizedRange(1, int.MaxValue)]
        public int EducationItemCurrencyId { get; set; }
        [LocalizedRequired]
        [LocalizedRange(1, int.MaxValue)]
        public int EducationItemId { get; set; }
        [LocalizedRequired]
        public decimal EducationItemCount { get; set; }
        [LocalizedRequired]
        public decimal EducationItemPrice { get; set; }
        [LocalizedRequired]
        public decimal EducationItemAmount { get; set; }
    }
}
